#!/bin/bash

# Ensure we are in the project root
cd "$(dirname "$0")/../.."

# Output file
OUTPUT_FILE="tests/3.4-os-limits/result.json"

# Initialize empty JSON array if not exists
if [ ! -f "$OUTPUT_FILE" ]; then
    echo "[]" > $OUTPUT_FILE
fi

echo "Environment: $(pwd)"
echo "Output: $OUTPUT_FILE"

# Use specific docker-compose for this test
COMPOSE_FILE="tests/3.4-os-limits/docker-compose.yml"

echo "Using Compose File: $COMPOSE_FILE"
echo "Starting Constrained API (ulimit=50)..."
docker compose -f $COMPOSE_FILE up -d --build api external-api
echo "Waiting for API to settle..."
sleep 5

run_test() {
    echo "Running OS Limit Test (Limit=50 Open Files)..."
    
    # Run k6
    # Note: Using default k6 image from main docker-compose is tricky if networks differ.
    # The tests/3.4.../docker-compose.yml defines its own k6 service correctly.
    docker compose -f $COMPOSE_FILE run --rm k6 run /scripts/os-limits.js --summary-export=/scripts/summary-os.json || true
    
    # Update results.json using Python
    sleep 2
    python3 -c "
import json
try:
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    with open('k6/summary-os.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    fail_rate = m.get('http_req_failed', {}).get('rate', 0)
    
    new_record = {
        'scenario': 'OS Limit (ulimit=50)',
        'fail_rate': fail_rate,
        'note': 'Expect high failure rate due to socket exhaustion at OS level'
    }
    
    results.append(new_record)
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing OS limit test')
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
