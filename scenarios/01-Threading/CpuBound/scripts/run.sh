#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
cd "$BASE_DIR"

OUTPUT_FILE="$BASE_DIR/results/cpu_result.json"
SUMMARY_FILE="$BASE_DIR/results/summary-cpu.json"

mkdir -p "$BASE_DIR/results"
echo "[]" > "$OUTPUT_FILE"

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

echo "Rebuilding CpuBound API..."
docker compose -f "$COMPOSE_FILE" up -d --build cpu-bound-api

echo "Waiting API health..."
until curl -sf http://localhost:8085/health >/dev/null; do
  sleep 2
done

run_test() {
  RPS="$1"
  echo "Running CPU Bound test for $RPS RPS..."

  docker compose -f "$COMPOSE_FILE" run --rm --profile tools -e RPS="$RPS" k6 \
    run /scripts/cpu-bound.js \
    --summary-export=/results/summary-cpu.json || true

  python3 -c "
import json

with open('$OUTPUT_FILE', 'r') as f:
    results = json.load(f)

with open('$SUMMARY_FILE', 'r') as f:
    k6_data = json.load(f)

m = k6_data['metrics']
dur = m['http_req_duration']
fail_rate = m.get('http_req_failed', {}).get('rate', 0)

results.append({
    'target_rps': int('$RPS'),
    'avg_latency': dur.get('avg'),
    'p95_latency': dur.get('p(95)'),
    'p99_latency': dur.get('p(99)'),
    'fail_rate': fail_rate
})

with open('$OUTPUT_FILE', 'w') as f:
    json.dump(results, f, indent=2)
"

  echo "Test for $RPS RPS completed."
}

run_test 5
run_test 20
run_test 50

echo "All tests completed. Results:"
cat "$OUTPUT_FILE"
