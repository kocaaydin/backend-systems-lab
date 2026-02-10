#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$ROOT_DIR"

OUTPUT_FILE="$ROOT_DIR/cpu_result.json"
SUMMARY_FILE="$ROOT_DIR/k6/summary-cpu.json"

if [ ! -f "$OUTPUT_FILE" ]; then
  echo "[]" > "$OUTPUT_FILE"
fi

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

echo "Rebuilding CpuBound API..."
docker compose up -d --build cpu-bound-api

echo "Waiting API health..."
until curl -sf http://localhost:8085/health >/dev/null; do
  sleep 2
done

run_test() {
  RPS="$1"
  echo "Running CPU Bound test for $RPS RPS..."

  docker compose run --rm --profile tools -e RPS="$RPS" k6 \
    run /scripts/cpu-bound.js \
    --summary-export=/scripts/summary-cpu.json || true

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
