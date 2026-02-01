#!/bin/bash

# Ensure we are in the project root
cd "$(dirname "$0")/../.."

# Output file
OUTPUT_FILE="tests/3.5-proxy-limits/result.json"

# Initialize empty JSON array if not exists
if [ ! -f "$OUTPUT_FILE" ]; then
    echo "[]" > $OUTPUT_FILE
fi

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

# Use specific docker-compose for this test
COMPOSE_FILE="tests/3.5-proxy-limits/docker-compose.yml"

echo "Using Compose File: $COMPOSE_FILE"
echo "Starting API with Nginx Proxy (Rate=50r/s)..."
docker compose -f $COMPOSE_FILE up -d --build api nginx-proxy external-api
echo "Waiting for API to settle..."
sleep 5

run_test() {
    echo "Running Proxy Limit Test (Target 200 RPS, Proxy Limit 50 RPS)..."
    
    # Run k6
    docker compose -f $COMPOSE_FILE run --rm k6 run /scripts/proxy-limit.js --summary-export=/scripts/summary-proxy.json || true
    
    # Update results.json using Python
    sleep 2
    python3 -c "
import json
try:
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    with open('k6/summary-proxy.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    http_reqs = m['http_reqs']['count']
    rate = m['http_reqs']['rate']
    
    new_record = {
        'scenario': 'Proxy Limit 50 RPS',
        'total_requests': http_reqs,
        'actual_rps': rate,
        'note': 'Expect ~50 RPS success, rest failed (503 from Nginx -> 500 from API)'
    }
    
    results.append(new_record)
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing proxy limit test')
except Exception as e:
    print(f'Error processing results: {e}')
"
    
    echo "Test completed."
    
    # Cleanup specialized containers
    docker compose -f $COMPOSE_FILE down
}

run_test

echo "All tests completed. Results:"
cat $OUTPUT_FILE
