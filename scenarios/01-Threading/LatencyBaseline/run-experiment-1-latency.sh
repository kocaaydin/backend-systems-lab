#!/bin/bash

# Ensure we are in the project root (two levels up from tests/latency/)
cd "$(dirname "$0")/../.."

# Output file (directly in tests/latency)
OUTPUT_FILE="tests/latency/latency_result.json"

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
    echo "Running test for $RPS RPS..."
    
    # Run k6 and capture JSON summary
    docker compose run --rm -e RPS=$RPS k6 run /scripts/script.js --summary-export=/scripts/summary.json > /dev/null
    
    # Update results.json using Python
    python3 -c "
import json
try:
    # Read current results
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    # Read k6 output
    with open('k6/summary.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    dur = m['http_req_duration']
    
    # Create new record
    new_record = {
        'target_rps': $RPS,
        'avg_latency': dur['avg'],
        'p95_latency': dur['p(95)'],
        'p99_latency': dur['p(99)'],
    }
    
    results.append(new_record)
    
    # Save back
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing $RPS RPS')
except Exception as e:
    print(f'Error processing results: {e}')
"
    
    echo "Test for $RPS RPS completed."
}

run_test 10
run_test 300
run_test 1000

echo "All tests completed. Results:"
cat $OUTPUT_FILE
