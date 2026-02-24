#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULT_DIR="$BASE_DIR/results"

RUN_COUNT="4"
WARMUP_DURATION="5s"
TEST_DURATION="10s"
FAST_RPS="20"
HEAVY_RPS="50"
BLOCK_MS="500"

TUNED_MIN_WORKER="100"
TUNED_MIN_IO="100"

mkdir -p "$RESULT_DIR"
rm -f "$RESULT_DIR"/06-minthreads-*.json

restart_api() {
  echo "[api] restart ediliyor..."
  docker compose -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null

  until curl -sf http://localhost:8091/health >/dev/null; do
    sleep 1
  done
  echo "[api] hazir."
}

run_warmup() {
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e MODE=warmup \
    -e DURATION="$WARMUP_DURATION" \
    k6 run --summary-export /results/06-minthreads-warmup-summary.json /scripts/06_min_threads_comparison.js >/dev/null
}

capture_pool_stats() {
  local tag="$1"
  curl -sf "http://localhost:8091/thread-types/pool-stats" >"$RESULT_DIR/06-minthreads-${tag}-pool-stats.json"
}

run_test() {
  local phase="$1"
  local run_no="$2"
  local out_file="/results/06-minthreads-${phase}-run-${run_no}-summary.json"

  echo "[${phase}] run ${run_no}/${RUN_COUNT}"
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e MODE=load \
    -e DURATION="$TEST_DURATION" \
    -e FAST_RPS="$FAST_RPS" \
    -e HEAVY_RPS="$HEAVY_RPS" \
    -e BLOCK_MS="$BLOCK_MS" \
    k6 run --summary-export "$out_file" /scripts/06_min_threads_comparison.js >/dev/null
}

apply_min_threads() {
  local min_worker="$1"
  local min_io="$2"
  local out_path="$3"

  curl -sf -X POST "http://localhost:8091/thread-types/set-min-threads?minWorker=${min_worker}&minIo=${min_io}" >"$out_path"
}

echo "=== Senaryo 6: SetMinThreads Karsilastirmasi (Default vs Tuned) ==="
echo "run_count=$RUN_COUNT warmup=$WARMUP_DURATION"
echo "duration=$TEST_DURATION fast_rps=$FAST_RPS heavy_rps=$HEAVY_RPS block_ms=$BLOCK_MS"
echo "tuned_min_worker=$TUNED_MIN_WORKER tuned_min_io=$TUNED_MIN_IO"

echo ""
echo "--- Phase A: Default MinThreads ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  capture_pool_stats "default-run-${i}-before"
  run_warmup
  run_test default "$i"
  capture_pool_stats "default-run-${i}-after"
done

echo ""
echo "--- Phase B: Tuned MinThreads ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  apply_min_threads "$TUNED_MIN_WORKER" "$TUNED_MIN_IO" "$RESULT_DIR/06-minthreads-tuned-run-${i}-set-response.json"
  capture_pool_stats "tuned-run-${i}-before"
  run_warmup
  run_test tuned "$i"
  capture_pool_stats "tuned-run-${i}-after"
done

python3 - <<PY
import glob
import json
from statistics import mean

result_dir = r"${RESULT_DIR}"

def percentile(metric: dict, candidates):
    for key in candidates:
        if key in metric:
            return float(metric[key])
    return float(metric["max"])

def parse_runs(phase: str):
    files = sorted(glob.glob(f"{result_dir}/06-minthreads-{phase}-run-*-summary.json"))
    if not files:
        raise SystemExit(f"{phase} icin sonuc dosyasi bulunamadi.")

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
            "fast_p95_ms": percentile(fast, ["p(95)", "p(95.00)", "p(90)", "max"]),
            "fast_p99_ms": percentile(fast, ["p(99)", "p(99.00)", "p(95)", "max"]),
            "http_fail_rate": float(failed),
        })

    return rows

def avg(rows, key):
    return round(mean([r[key] for r in rows]), 3)

def summarize(rows):
    return {
        "fast_avg_ms": avg(rows, "fast_avg_ms"),
        "fast_p95_ms": avg(rows, "fast_p95_ms"),
        "fast_p99_ms": avg(rows, "fast_p99_ms"),
        "http_fail_rate": round(mean([r["http_fail_rate"] for r in rows]), 6),
    }

def load_pool_stat(tag: str):
    path = f"{result_dir}/06-minthreads-{tag}-pool-stats.json"
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)

def load_set_response(run_no: int):
    path = f"{result_dir}/06-minthreads-tuned-run-{run_no}-set-response.json"
    with open(path, "r", encoding="utf-8") as f:
        return json.load(f)

default_runs = parse_runs("default")
tuned_runs = parse_runs("tuned")

default_avg = summarize(default_runs)
tuned_avg = summarize(tuned_runs)

delta = {
    "fast_avg_ms": round(tuned_avg["fast_avg_ms"] - default_avg["fast_avg_ms"], 3),
    "fast_p95_ms": round(tuned_avg["fast_p95_ms"] - default_avg["fast_p95_ms"], 3),
    "fast_p99_ms": round(tuned_avg["fast_p99_ms"] - default_avg["fast_p99_ms"], 3),
}

pool_stats = {
    "default_before": load_pool_stat("default-run-1-before"),
    "tuned_before": load_pool_stat("tuned-run-1-before"),
}

set_min_threads = {
    "run_1": load_set_response(1)
}

summary = {
    "config": {
        "run_count": int("${RUN_COUNT}"),
        "warmup_duration": "${WARMUP_DURATION}",
        "test_duration": "${TEST_DURATION}",
        "fast_rps": int("${FAST_RPS}"),
        "heavy_rps": int("${HEAVY_RPS}"),
        "block_ms": int("${BLOCK_MS}"),
        "tuned_min_worker": int("${TUNED_MIN_WORKER}"),
        "tuned_min_io": int("${TUNED_MIN_IO}"),
    },
    "default_runs": default_runs,
    "tuned_runs": tuned_runs,
    "default_avg": default_avg,
    "tuned_avg": tuned_avg,
    "delta_tuned_minus_default_ms": delta,
    "pool_stats": pool_stats,
    "set_min_threads": set_min_threads,
}

out_path = f"{result_dir}/06-minthreads-summary.json"
with open(out_path, "w", encoding="utf-8") as f:
    json.dump(summary, f, indent=2)

print("")
print("=== ThreadPool Snapshot (run1 / before load) ===")
default_worker = pool_stats["default_before"]["workerThreads"]
default_iocp = pool_stats["default_before"]["completionPortThreads"]
tuned_worker = pool_stats["tuned_before"]["workerThreads"]
tuned_iocp = pool_stats["tuned_before"]["completionPortThreads"]
print(
    "default worker[min/max/active/available]: "
    f"{default_worker['min']}/{default_worker['max']}/{default_worker['active']}/{default_worker['available']}"
)
print(
    "default iocp  [min/max/active/available]: "
    f"{default_iocp['min']}/{default_iocp['max']}/{default_iocp['active']}/{default_iocp['available']}"
)
print(
    "tuned   worker[min/max/active/available]: "
    f"{tuned_worker['min']}/{tuned_worker['max']}/{tuned_worker['active']}/{tuned_worker['available']}"
)
print(
    "tuned   iocp  [min/max/active/available]: "
    f"{tuned_iocp['min']}/{tuned_iocp['max']}/{tuned_iocp['active']}/{tuned_iocp['available']}"
)

print("")
print("=== Ortalama Sonuclar ===")
print("mode    | fast_avg_ms | fast_p95_ms | fast_p99_ms | fail_rate")
print("--------|-------------|-------------|-------------|----------")
print(
    f"default | {default_avg['fast_avg_ms']:>11.3f} | {default_avg['fast_p95_ms']:>11.3f} | "
    f"{default_avg['fast_p99_ms']:>11.3f} | {default_avg['http_fail_rate']*100:>7.3f}%"
)
print(
    f"tuned   | {tuned_avg['fast_avg_ms']:>11.3f} | {tuned_avg['fast_p95_ms']:>11.3f} | "
    f"{tuned_avg['fast_p99_ms']:>11.3f} | {tuned_avg['http_fail_rate']*100:>7.3f}%"
)
print(
    f"delta   | {delta['fast_avg_ms']:>11.3f} | {delta['fast_p95_ms']:>11.3f} | "
    f"{delta['fast_p99_ms']:>11.3f} | {'n/a':>8}"
)

print("")
print("=== set-min-threads response (run1) ===")
print(json.dumps(set_min_threads["run_1"], indent=2))
print(f"summary_file: {out_path}")
PY
