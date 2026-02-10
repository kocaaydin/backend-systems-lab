#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULT_DIR="$BASE_DIR/results"

SMALL_N="${SMALL_N:-20000}"
LARGE_N="${LARGE_N:-200000}"
REQUEST_COUNT="${REQUEST_COUNT:-40}"
TARGET_URL="${TARGET_URL:-http://localhost:8085}"

mkdir -p "$RESULT_DIR"

echo "Starting cpu-bound-api..."
docker compose -f "$COMPOSE_FILE" up -d --build cpu-bound-api >/dev/null

echo "Waiting health endpoint..."
until curl -sf "$TARGET_URL/health" >/dev/null; do
  sleep 1
done

run_phase() {
  local label="$1"
  local n_value="$2"
  local time_file cpu_file

  time_file="$(mktemp)"
  cpu_file="$(mktemp)"

  echo "Running phase: $label (n=$n_value, requests=$REQUEST_COUNT)" >&2
  for _ in $(seq 1 "$REQUEST_COUNT"); do
    curl -s -o /dev/null -w '%{time_total}\n' "$TARGET_URL/experiments/cpu?n=$n_value" >> "$time_file"
    docker stats --no-stream --format '{{.CPUPerc}}' cpu-bound-api \
      | head -n1 \
      | tr -d '%' \
      >> "$cpu_file"
  done

  python3 - <<PY
import json
import math

label = "${label}"
n_value = int("${n_value}")

with open("${time_file}", "r", encoding="utf-8") as fh:
    times = [float(x.strip()) for x in fh if x.strip()]

with open("${cpu_file}", "r", encoding="utf-8") as fh:
    cpu_vals = []
    for x in fh:
        x = x.strip()
        if not x:
            continue
        try:
            cpu_vals.append(float(x))
        except ValueError:
            pass

times_sorted = sorted(times)
idx = max(0, math.ceil(0.95 * len(times_sorted)) - 1)
p95 = times_sorted[idx] if times_sorted else 0.0

result = {
    "label": label,
    "n": n_value,
    "requests": len(times),
    "avg_ms": round((sum(times) / len(times)) * 1000, 3) if times else 0.0,
    "p95_ms": round(p95 * 1000, 3),
    "cpu_avg_pct": round(sum(cpu_vals) / len(cpu_vals), 3) if cpu_vals else 0.0,
    "cpu_max_pct": round(max(cpu_vals), 3) if cpu_vals else 0.0,
}

print(json.dumps(result))
PY

  rm -f "$time_file" "$cpu_file"
}

small_result="$(run_phase "small" "$SMALL_N")"
large_result="$(run_phase "large" "$LARGE_N")"

timestamp="$(date +%Y%m%d-%H%M%S)"
out_file="$RESULT_DIR/simple-cpu-check-$timestamp.json"

python3 - <<PY
import json

small = json.loads('''${small_result}''')
large = json.loads('''${large_result}''')

summary = {
    "small": small,
    "large": large,
    "delta_avg_ms": round(large["avg_ms"] - small["avg_ms"], 3),
    "delta_p95_ms": round(large["p95_ms"] - small["p95_ms"], 3),
    "delta_cpu_avg_pct": round(large["cpu_avg_pct"] - small["cpu_avg_pct"], 3),
}

with open("${out_file}", "w", encoding="utf-8") as fh:
    json.dump(summary, fh, indent=2)

print("small:", small)
print("large:", large)
print("delta:", {
    "delta_avg_ms": summary["delta_avg_ms"],
    "delta_p95_ms": summary["delta_p95_ms"],
    "delta_cpu_avg_pct": summary["delta_cpu_avg_pct"],
})
print("result_file:", "${out_file}")
PY
