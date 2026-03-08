#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULT_DIR="$BASE_DIR/results"

RUN_COUNT="${RUN_COUNT:-3}" 
TEST_DURATION="${TEST_DURATION:-10s}"
FAST_RPS="${FAST_RPS:-20}"
LOAD_RPS="${LOAD_RPS:-40}"
N_VALUE="${N_VALUE:-2000000}"
CHECK_EVERY="${CHECK_EVERY:-200}"
REQ_TIMEOUT="${REQ_TIMEOUT:-50ms}"

mkdir -p "$RESULT_DIR"
rm -f "$RESULT_DIR"/10-fireforget-*.json

restart_api() {
  echo "[api] restart ediliyor..."
  docker compose -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null
  until curl -sf http://localhost:8091/health >/dev/null; do
    sleep 1
  done
  echo "[api] hazir."
}

warmup_fast() {
  for _ in $(seq 1 30); do
    curl -sf "http://localhost:8091/thread-types/fast" >/dev/null || true
  done
}

run_phase() {
  local phase="$1"
  local mode="$2"
  local run_no="$3"
  local out_file="/results/10-fireforget-${phase}-run-${run_no}.json"

  echo "[${phase}] run ${run_no}/${RUN_COUNT}"
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e MODE="$mode" \
    -e DURATION="$TEST_DURATION" \
    -e FAST_RPS="$FAST_RPS" \
    -e LOAD_RPS="$LOAD_RPS" \
    -e N_VALUE="$N_VALUE" \
    -e CHECK_EVERY="$CHECK_EVERY" \
    -e REQ_TIMEOUT="$REQ_TIMEOUT" \
    k6 run --summary-export "$out_file" /scripts/10_fire_and_forget_risk.js >/dev/null
}

echo "=== Senaryo 10: Fire-and-Forget Risk Simulasyonu ==="
echo "run_count=$RUN_COUNT duration=$TEST_DURATION fast_rps=$FAST_RPS load_rps=$LOAD_RPS"
echo "n_value=$N_VALUE check_every=$CHECK_EVERY req_timeout=$REQ_TIMEOUT"
echo "A: noncancellable(cpu-timeout-risk?cancellable=false) | B: cancellable(cpu-timeout-risk?cancellable=true)"

echo ""
echo "--- Phase A: Non-Cancellable Timeout ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  warmup_fast
  run_phase noncancellable noncancellable "$i"
done

echo ""
echo "--- Phase B: Cancellable Timeout ---"
for i in $(seq 1 "$RUN_COUNT"); do
  restart_api
  warmup_fast
  run_phase cancellable cancellable "$i"
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
    return float(metric.get("max", 0))

def parse_runs(phase: str):
    files = sorted(glob.glob(f"{result_dir}/10-fireforget-{phase}-run-*.json"))
    if not files:
        raise SystemExit(f"{phase} icin sonuc dosyasi bulunamadi.")
    rows = []
    for path in files:
        with open(path, "r", encoding="utf-8") as f:
            data = json.load(f)
        m = data["metrics"]
        fast = m["fast_latency"]
        timeouts = m.get("load_timeouts", {})
        non2xx = m.get("load_non_2xx", {})
        rows.append({
            "file": path,
            "fast_avg_ms": float(fast["avg"]),
            "fast_p95_ms": percentile(fast, ["p(95)", "p(95.00)", "p(90)", "max"]),
            "fast_p99_ms": percentile(fast, ["p(99)", "p(99.00)", "p(95)", "max"]),
            "load_timeouts": float(timeouts.get("count", 0)),
            "load_non_2xx": float(non2xx.get("count", 0)),
        })
    return rows

def avg(rows, key):
    return round(mean([r[key] for r in rows]), 3)

def summarize(rows):
    return {
        "fast_avg_ms": avg(rows, "fast_avg_ms"),
        "fast_p95_ms": avg(rows, "fast_p95_ms"),
        "fast_p99_ms": avg(rows, "fast_p99_ms"),
        "load_timeouts_avg": avg(rows, "load_timeouts"),
        "load_non_2xx_avg": avg(rows, "load_non_2xx"),
    }

non_rows = parse_runs("noncancellable")
can_rows = parse_runs("cancellable")

non_avg = summarize(non_rows)
can_avg = summarize(can_rows)

delta = {
    "fast_avg_ms": round(can_avg["fast_avg_ms"] - non_avg["fast_avg_ms"], 3),
    "fast_p95_ms": round(can_avg["fast_p95_ms"] - non_avg["fast_p95_ms"], 3),
    "fast_p99_ms": round(can_avg["fast_p99_ms"] - non_avg["fast_p99_ms"], 3),
}

summary = {
    "config": {
        "run_count": int("${RUN_COUNT}"),
        "test_duration": "${TEST_DURATION}",
        "fast_rps": int("${FAST_RPS}"),
        "load_rps": int("${LOAD_RPS}"),
        "n_value": int("${N_VALUE}"),
        "check_every": int("${CHECK_EVERY}"),
        "request_timeout": "${REQ_TIMEOUT}",
    },
    "noncancellable_runs": non_rows,
    "cancellable_runs": can_rows,
    "noncancellable_avg": non_avg,
    "cancellable_avg": can_avg,
    "delta_cancellable_minus_noncancellable_ms": delta,
}

out_path = f"{result_dir}/10-fireforget-summary.json"
with open(out_path, "w", encoding="utf-8") as f:
    json.dump(summary, f, indent=2)

print("")
print("=== Ortalama Sonuclar ===")
print("mode           | fast_avg_ms | fast_p95_ms | fast_p99_ms | timeout_avg | non2xx_avg")
print("---------------|-------------|-------------|-------------|------------|----------")
print(f"noncancellable | {non_avg['fast_avg_ms']:>11.3f} | {non_avg['fast_p95_ms']:>11.3f} | {non_avg['fast_p99_ms']:>11.3f} | {non_avg['load_timeouts_avg']:>10.1f} | {non_avg['load_non_2xx_avg']:>8.1f}")
print(f"cancellable    | {can_avg['fast_avg_ms']:>11.3f} | {can_avg['fast_p95_ms']:>11.3f} | {can_avg['fast_p99_ms']:>11.3f} | {can_avg['load_timeouts_avg']:>10.1f} | {can_avg['load_non_2xx_avg']:>8.1f}")
print(f"delta(ms)      | {delta['fast_avg_ms']:>11.3f} | {delta['fast_p95_ms']:>11.3f} | {delta['fast_p99_ms']:>11.3f} | {'n/a':>10} | {'n/a':>8}")
print("")
print(f"summary_file: {out_path}")
PY
