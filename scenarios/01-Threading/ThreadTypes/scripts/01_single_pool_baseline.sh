#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULT_DIR="$BASE_DIR/results"

RUN_COUNT="4"
WARMUP_DURATION="5s"

# Yuksuz (baseline) profil
BASELINE_TEST_DURATION="5s"
BASELINE_FAST_RPS="20"

# Yuklu (loaded) profil
LOADED_TEST_DURATION="5s"
LOADED_FAST_RPS="20"
LOADED_HEAVY_RPS="50"
LOADED_HEAVY_N="2000000"

mkdir -p "$RESULT_DIR"
rm -f "$RESULT_DIR"/01-single-pool-*.json

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
    k6 run --summary-export /results/01-single-pool-warmup-summary.json /scripts/01_single_pool_baseline.js >/dev/null
}

run_test() {
  local mode="$1"
  local run_no="$2"
  local duration="$3"
  local fast_rps="$4"
  local heavy_rps="$5"
  local heavy_n="$6"
  local out_file="/results/01-single-pool-${mode}-run-${run_no}-summary.json"

  echo "[${mode}] run ${run_no}/${RUN_COUNT}"
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e MODE="$mode" \
    -e DURATION="$duration" \
    -e FAST_RPS="$fast_rps" \
    -e HEAVY_RPS="$heavy_rps" \
    -e HEAVY_N="$heavy_n" \
    k6 run --summary-export "$out_file" /scripts/01_single_pool_baseline.js >/dev/null
}

echo "=== Senaryo 1: Tek Pool Baseline ==="
echo "run_count=$RUN_COUNT warmup=$WARMUP_DURATION"
echo "baseline: duration=$BASELINE_TEST_DURATION fast_rps=$BASELINE_FAST_RPS"
echo "loaded  : duration=$LOADED_TEST_DURATION fast_rps=$LOADED_FAST_RPS heavy_rps=$LOADED_HEAVY_RPS heavy_n=$LOADED_HEAVY_N"

echo ""
echo "--- Yüksüz (baseline) testleri ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  run_warmup
  run_test baseline "$i" "$BASELINE_TEST_DURATION" "$BASELINE_FAST_RPS" "1" "20000"
done

echo ""
echo "--- Yük altı (loaded) testleri ---"
echo "Not: Yüksüz testlerden sonra yük testlerine geçerken API restart edilir."
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  run_warmup
  run_test loaded "$i" "$LOADED_TEST_DURATION" "$LOADED_FAST_RPS" "$LOADED_HEAVY_RPS" "$LOADED_HEAVY_N"
done

python3 - <<PY
import glob
import json
from statistics import mean

result_dir = r"${RESULT_DIR}"

def percentile(metric: dict, key: str, fallback_key: str):
    if key in metric:
        return float(metric[key])
    return float(metric[fallback_key])

def parse_runs(mode: str):
    files = sorted(glob.glob(f"{result_dir}/01-single-pool-{mode}-run-*-summary.json"))
    if not files:
        raise SystemExit(f"{mode} icin sonuc dosyasi bulunamadi.")
    rows = []
    for path in files:
        with open(path, "r", encoding="utf-8") as f:
            data = json.load(f)
        m = data["metrics"]
        fast = m["fast_latency"]
        failed = m["http_req_failed"]["value"]
        rows.append({
            "file": path,
            "fast_avg_ms": float(fast["avg"]),
            "fast_p95_ms": float(fast["p(95)"]),
            "fast_p99_ms": percentile(fast, "p(99)", "p(95)"),
            "http_fail_rate": float(failed),
        })
    return rows

baseline = parse_runs("baseline")
loaded = parse_runs("loaded")

summary = {
    "config": {
        "run_count": int("${RUN_COUNT}"),
        "warmup_duration": "${WARMUP_DURATION}",
        "baseline": {
            "duration": "${BASELINE_TEST_DURATION}",
            "fast_rps": int("${BASELINE_FAST_RPS}")
        },
        "loaded": {
            "duration": "${LOADED_TEST_DURATION}",
            "fast_rps": int("${LOADED_FAST_RPS}"),
            "heavy_rps": int("${LOADED_HEAVY_RPS}"),
            "heavy_n": int("${LOADED_HEAVY_N}")
        },
    },
    "baseline_runs": baseline,
    "loaded_runs": loaded,
    "baseline_avg": {
        "fast_avg_ms": round(mean([r["fast_avg_ms"] for r in baseline]), 3),
        "fast_p95_ms": round(mean([r["fast_p95_ms"] for r in baseline]), 3),
        "fast_p99_ms": round(mean([r["fast_p99_ms"] for r in baseline]), 3),
        "http_fail_rate": round(mean([r["http_fail_rate"] for r in baseline]), 6),
    },
    "loaded_avg": {
        "fast_avg_ms": round(mean([r["fast_avg_ms"] for r in loaded]), 3),
        "fast_p95_ms": round(mean([r["fast_p95_ms"] for r in loaded]), 3),
        "fast_p99_ms": round(mean([r["fast_p99_ms"] for r in loaded]), 3),
        "http_fail_rate": round(mean([r["http_fail_rate"] for r in loaded]), 6),
    },
}

summary["delta_loaded_minus_baseline_ms"] = {
    "fast_avg_ms": round(summary["loaded_avg"]["fast_avg_ms"] - summary["baseline_avg"]["fast_avg_ms"], 3),
    "fast_p95_ms": round(summary["loaded_avg"]["fast_p95_ms"] - summary["baseline_avg"]["fast_p95_ms"], 3),
    "fast_p99_ms": round(summary["loaded_avg"]["fast_p99_ms"] - summary["baseline_avg"]["fast_p99_ms"], 3),
}

out_path = f"{result_dir}/01-single-pool-average.json"
with open(out_path, "w", encoding="utf-8") as f:
    json.dump(summary, f, indent=2)

print("")
print("=== Ortalama Sonuclar ===")
print(f"baseline fast avg: {summary['baseline_avg']['fast_avg_ms']} ms | p95: {summary['baseline_avg']['fast_p95_ms']} ms | p99: {summary['baseline_avg']['fast_p99_ms']} ms")
print(f"loaded   fast avg: {summary['loaded_avg']['fast_avg_ms']} ms | p95: {summary['loaded_avg']['fast_p95_ms']} ms | p99: {summary['loaded_avg']['fast_p99_ms']} ms")
print(f"delta    fast avg: {summary['delta_loaded_minus_baseline_ms']['fast_avg_ms']} ms | p95: {summary['delta_loaded_minus_baseline_ms']['fast_p95_ms']} ms | p99: {summary['delta_loaded_minus_baseline_ms']['fast_p99_ms']} ms")
print(f"summary_file: {out_path}")
PY
