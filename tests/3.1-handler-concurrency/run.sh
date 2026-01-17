#!/bin/bash

# Ensure we are in the project root
cd "$(dirname "$0")/../.."

# Output file
OUTPUT_FILE="tests/3.1-handler-concurrency/result.json"

# Initialize empty JSON array if not exists
if [ ! -f "$OUTPUT_FILE" ]; then
    echo "[]" > $OUTPUT_FILE
fi

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

echo "Ensuring API is running..."
docker compose up -d api external-api
echo "Waiting for API to settle..."
sleep 5

run_test() {
    LIMIT=$1
    echo "Running Handler Concurrency test with MaxConnections=$LIMIT..."
    
    # Run k6 and capture JSON summary (pass connection limit as env var)
    # Using existing http-limit.js which supports CONN_LIMIT env var
    # Increasing rate is handled in k6 script via env var or default? 
    # Let's pass RPS via env var to be safe if script supports it, otherwise reliance on default in script.
    # The existing script had hardcoded 500 RPS. We should update k6 script or trust it.
    # Let's assume we update the JS file separately to 1000 RPS.
    
    docker compose run --rm -e CONN_LIMIT=$LIMIT k6 run /scripts/http-limit.js --summary-export=/scripts/summary-http.json || true
    
    # Update results.json using Python
    sleep 2
    python3 -c "
import json
try:
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    with open('k6/summary-http.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    dur = m['http_req_duration']
    
    new_record = {
        'connection_limit': $LIMIT,
        'avg_latency': dur['avg'],
        'p95_latency': dur['p(95)'],
        'p99_latency': dur['p(99)'],
        'rps': m['http_reqs']['rate']
    }
    
    results.append(new_record)
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing limit=$LIMIT')
except Exception as e:
    print(f'Error processing results: {e}')
"
    
    echo "Test for MaxConnections=$LIMIT completed."
}

# Run tests with Bad Configuration vs Good Configuration
run_test 10    # The "Bug" - Low limit
run_test 1000  # The "Fix" - High limit

echo "All tests completed. Results:"
cat $OUTPUT_FILE
