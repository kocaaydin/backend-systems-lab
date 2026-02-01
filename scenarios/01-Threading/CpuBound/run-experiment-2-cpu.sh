#!/bin/bash

# Ensure we are in the project root (two levels up from tests/cpu/)
cd "$(dirname "$0")/../.."

# Output file (directly in tests/cpu)
OUTPUT_FILE="tests/cpu/cpu_result.json"

# Initialize empty JSON array if not exists
if [ ! -f "$OUTPUT_FILE" ]; then
    echo "[]" > $OUTPUT_FILE
fi

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

echo "Rebuilding API..."
docker compose up -d --build api
echo "Waiting for API to settle..."
sleep 5

run_test() {
    RPS=$1
    echo "Running CPU Bound test for $RPS RPS..."
    
    # Run k6 and capture JSON summary
    # Run k6 and capture JSON summary (allow failure for thresholds)
    docker compose run --rm -e RPS=$RPS k6 run /scripts/cpu-bound.js --summary-export=/scripts/summary-cpu.json || true
    
    # Update results.json using Python
    sleep 2
    python3 -c "
import json

def get_metric(k6_data, metric_name, tag=None):
    try:
        metrics = k6_data.get('metrics', {})
        if tag:
            # Look for specific scenario metrics if k6 provides them in this format
            # Note: k6 summary export might flatten tags differently depending on version/config
            # For standard JSON summary, we might need to rely on the general http_req_duration 
            # if we didn't setup complex thresholds. Without 'thresholds', separate scenario stats 
            # might not be in the root 'metrics'.
            # However, let's assume valid JSON structure or stick to global values for simplicity 
            # unless we parse raw data. 
            pass
        
        # Fallback to global metric for this simplified lab
        m = metrics.get(metric_name)
        if not m: return None
        return m
    except:
        return None

try:
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    with open('k6/summary-cpu.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    dur = m['http_req_duration']
    
    # Try to find specific scenario failure rates if possible, otherwise global
    fail_rate = m.get('http_req_failed', {}).get('rate', 0)
    
    new_record = {
        'target_rps': $RPS,
        'avg_latency': dur['avg'],
        'p95_latency': dur['p(95)'],
        'p99_latency': dur['p(99)'],
        'fail_rate': fail_rate,
        'note': 'Metrics are global average of CPU stress + Health check'
    }
    
    # Check if we have specific trend stats for health check from thresholds?
    # k6 summary json usually combines them unless we use specific outputs.
    # We will trust the global average for now as high CPU usage affects ALL requests.
    
    results.append(new_record)
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing $RPS RPS')
except Exception as e:
    print(f'Error processing results: {e}')
"
    
    echo "Test for $RPS RPS completed."
}

# Run tests with increasing load
# We expect low RPS to be fine, but higher RPS to saturate CPU quickly due to calculation
run_test 5
run_test 20
run_test 50

echo "All tests completed. Results:"
cat $OUTPUT_FILE
