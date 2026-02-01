#!/bin/bash

# Ensure we are in the project root
cd "$(dirname "$0")/../.."

# Output file
OUTPUT_FILE="tests/3.3-socket-exhaustion/result.json"

# Initialize empty JSON array if not exists
if [ ! -f "$OUTPUT_FILE" ]; then
    echo "[]" > $OUTPUT_FILE
fi

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

echo "Rebuilding API (New Endpoint)..."
docker compose up -d --build api
echo "Waiting for API to settle..."
sleep 5

run_test() {
    echo "Running Socket Exhaustion Test (Target 200 RPS for 60s)..."
    
    # Run k6
    # We hope to see socket errors in the logs or k6 failing
    docker compose run --rm k6 run /scripts/socket-exhaustion.js --summary-export=/scripts/summary-socket.json || true
    
    # Update results.json using Python
    sleep 2
    python3 -c "
import json
try:
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    with open('k6/summary-socket.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    fail_rate = m.get('http_req_failed', {}).get('rate', 0)
    p95 = m.get('http_req_duration', {}).get('p(95)', 0)
    
    new_record = {
        'scenario': 'Socket Exhaustion',
        'fail_rate': fail_rate,
        'p95_latency': p95,
        'note': 'High failure rate or latency spike indicates ephemeral port exhaustion'
    }
    
    results.append(new_record)
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing socket exhaustion test')
except Exception as e:
    print(f'Error processing results: {e}')
"
    
    echo "Test completed."
}

run_test

echo "All tests completed. Results:"
cat $OUTPUT_FILE
