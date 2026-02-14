#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULT_DIR="$BASE_DIR/results"

RUN_COUNT="3"
WARMUP_DURATION="5s"

# Test Parametreleri
TEST_DURATION="10s"
FAST_RPS="20"
HEAVY_RPS="50"
HEAVY_N="2000000"

mkdir -p "$RESULT_DIR"
rm -f "$RESULT_DIR"/03-comparison-*.json

restart_api() {
  echo "[api] restart ediliyor..."
  docker compose -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null

  until curl -sf http://localhost:8091/health >/dev/null; do
    sleep 1
  done
  echo "[api] hazır."
}

run_warmup() {
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e MODE=warmup \
    -e DURATION="$WARMUP_DURATION" \
    k6 run --summary-export /results/03-cpu-pool-vs-dedicated-warmup-summary.json /scripts/03_cpu_bound_pool_vs_dedicated.js >/dev/null
}

run_test() {
  local scenario_type="$1" # 'pool' or 'dedicated'
  local run_no="$2"
  local out_file="/results/03-cpu-pool-vs-dedicated-${scenario_type}-run-${run_no}-summary.json"

  echo "[${scenario_type}] run ${run_no}/${RUN_COUNT}"
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e SCENARIO_TYPE="$scenario_type" \
    -e DURATION="$TEST_DURATION" \
    -e FAST_RPS="$FAST_RPS" \
    -e HEAVY_RPS="$HEAVY_RPS" \
    -e HEAVY_N="$HEAVY_N" \
    k6 run --summary-export "$out_file" /scripts/03_cpu_bound_pool_vs_dedicated.js >/dev/null
}

echo "=== Senaryo 3: ThreadPool vs Dedicated Thread Karşılaştırması ==="
echo "run_count=$RUN_COUNT warmup=$WARMUP_DURATION duration=$TEST_DURATION"
echo "fast_rps=$FAST_RPS heavy_rps=$HEAVY_RPS heavy_n=$HEAVY_N"

echo ""
echo "--- Phase A: ThreadPool (Gürültülü Komşu = Pool Thread) ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  run_warmup
  run_test pool "$i"
done

echo ""
echo "--- Phase B: Dedicated Thread (Gürültülü Komşu = New Thread) ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  run_warmup
  run_test dedicated "$i"
done

python3 - <<PY
import glob
import json
from statistics import mean

result_dir = r"${RESULT_DIR}"

def parse_runs(scenario_type: str):
    files = sorted(glob.glob(f"{result_dir}/03-cpu-pool-vs-dedicated-{scenario_type}-run-*-summary.json"))
    if not files:
        raise SystemExit(f"{scenario_type} icin sonuc dosyasi bulunamadi.")
    rows = []
    for path in files:
        with open(path, "r", encoding="utf-8") as f:
            data = json.load(f)
        m = data["metrics"]
        
        # fast_latency metric might not exist if no requests succeeded, safe access
        if "fast_latency" not in m:
             rows.append({
                "file": path,
                "fast_avg_ms": 0.0,
                "fast_p95_ms": 0.0,
                "http_fail_rate": 1.0, 
            })
             continue

        fast = m["fast_latency"]
        failed = m["http_req_failed"]["value"]
        rows.append({
            "file": path,
            "fast_avg_ms": float(fast["avg"]),
            "fast_p95_ms": float(fast["p(95)"]),
            "http_fail_rate": float(failed),
        })
    return rows

pool_runs = parse_runs("pool")
dedicated_runs = parse_runs("dedicated")

def calc_avg(runs):
    if not runs: return {}
    return {
        "fast_avg_ms": round(mean([r["fast_avg_ms"] for r in runs]), 3),
        "fast_p95_ms": round(mean([r["fast_p95_ms"] for r in runs]), 3),
        "http_fail_rate": round(mean([r["http_fail_rate"] for r in runs]), 6),
    }

pool_avg = calc_avg(pool_runs)
dedicated_avg = calc_avg(dedicated_runs)

summary = {
    "pool_runs": pool_runs,
    "dedicated_runs": dedicated_runs,
    "pool_avg": pool_avg,
    "dedicated_avg": dedicated_avg
}

out_path = f"{result_dir}/03-cpu-pool-vs-dedicated-results.json"
with open(out_path, "w", encoding="utf-8") as f:
    json.dump(summary, f, indent=2)

print("")
print("=== Karşılaştırma Sonuçları ===")
print("Senaryo              | Avg Latency | P95 Latency | Fail Rate")
print("---------------------|-------------|-------------|-----------")
print(f"ThreadPool (Heavy)   | {pool_avg['fast_avg_ms']:>8} ms | {pool_avg['fast_p95_ms']:>8} ms | {pool_avg['http_fail_rate']*100:.1f}%")
print(f"Dedicated (Heavy)    | {dedicated_avg['fast_avg_ms']:>8} ms | {dedicated_avg['fast_p95_ms']:>8} ms | {dedicated_avg['http_fail_rate']*100:.1f}%")
print("")
print(f"Detaylı dosya: {out_path}")
PY
