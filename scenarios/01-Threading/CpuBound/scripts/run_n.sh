#!/usr/bin/env bash
set -euo pipefail

if [ $# -lt 1 ]; then
  echo "Usage: $0 <n>"
  exit 1
fi

N_VALUE="$1"
REPEAT_COUNT="${REPEAT_COUNT:-3}"
DURATION="${DURATION:-30s}"
RPS_VALUE="${RPS_VALUE:-20}"

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULT_DIR="$BASE_DIR/results"

mkdir -p "$RESULT_DIR"
rm -f "$RESULT_DIR/k6-n-${N_VALUE}-run-"*-summary.json "$RESULT_DIR/k6-n-${N_VALUE}-average.json"

restart_api() {
  docker compose -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker compose -f "$COMPOSE_FILE" up -d --build cpu-bound-api >/dev/null
  until curl -sf http://localhost:8085/health >/dev/null; do
    sleep 1
  done
}

run_once() {
  local run_no="$1"
  local out_file="/results/k6-n-${N_VALUE}-run-${run_no}-summary.json"
  echo "[N=${N_VALUE}] Run ${run_no}/${REPEAT_COUNT} (RPS=${RPS_VALUE})"
  restart_api

  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e RPS="$RPS_VALUE" \
    -e DURATION="$DURATION" \
    -e N="$N_VALUE" \
    k6 run --summary-export "$out_file" /scripts/cpu-bound.js >/dev/null
}

for i in $(seq 1 "$REPEAT_COUNT"); do
  run_once "$i"
done

python3 - <<PY
import glob
import json
import statistics

n_value = int("${N_VALUE}")
files = sorted(glob.glob(r"${RESULT_DIR}/k6-n-${N_VALUE}-run-*-summary.json"))
if not files:
    raise SystemExit("No summary files found.")

p95_vals = []
avg_vals = []
fail_vals = []
req_rate_vals = []
dropped_vals = []
for f in files:
    with open(f, "r", encoding="utf-8") as fh:
        data = json.load(fh)
    m = data["metrics"]
    dur = m["http_req_duration"]
    fail = m["http_req_failed"]
    p95_vals.append(float(dur["p(95)"]))
    avg_vals.append(float(dur["avg"]))
    fail_vals.append(float(fail["value"]))
    req_rate_vals.append(float(m["http_reqs"]["rate"]))
    dropped_vals.append(int(m.get("dropped_iterations", {}).get("count", 0)))

result = {
    "n": n_value,
    "repeat_count": len(files),
    "duration": "${DURATION}",
    "rps": int("${RPS_VALUE}"),
    "files": files,
    "avg_of_avg_ms": round(sum(avg_vals) / len(avg_vals), 6),
    "avg_of_p95_ms": round(sum(p95_vals) / len(p95_vals), 6),
    "median_p95_ms": round(statistics.median(p95_vals), 6),
    "avg_fail_rate": round(sum(fail_vals) / len(fail_vals), 8),
    "avg_http_reqs_rate": round(sum(req_rate_vals) / len(req_rate_vals), 6),
    "dropped_iterations": dropped_vals,
}

out = r"${RESULT_DIR}/k6-n-${N_VALUE}-average.json"
with open(out, "w", encoding="utf-8") as fh:
    json.dump(result, fh, indent=2)
print(out)
PY

echo "[N=${N_VALUE}] Completed."
