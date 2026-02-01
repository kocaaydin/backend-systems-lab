#!/bin/bash

# Ensure we are in the project root
cd "$(dirname "$0")/../.."

# Output file
OUTPUT_FILE="tests/3.2-outbound-rate-limiter/result.json"

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
    echo "Running In-App Rate Limiter Test (Target 500 RPS, Limit 100 RPS)..."
    
    # Run k6
    docker compose run --rm k6 run /scripts/outbound-rate-limit.js --summary-export=/scripts/summary-rate.json || true
    
    # Update results.json using Python
    sleep 2
    python3 -c "
import json
try:
    with open('$OUTPUT_FILE', 'r') as f:
        results = json.load(f)
        
    with open('k6/summary-rate.json', 'r') as f:
        k6_data = json.load(f)
        
    m = k6_data['metrics']
    http_reqs = m['http_reqs']['count']
    rate = m['http_reqs']['rate']
    
    # Check custom checks for 200 vs 429
    # k6 summary structure for checks might vary, defaulting to analyzing fails 
    # But usually checks are under root_group -> checks
    # Let's trust the k6 output printed to console for check percentages
    
    new_record = {
        'scenario': 'Outbound Rate Limiter (Limit 100 RPS)',
        'total_requests': http_reqs,
        'actual_rps': rate,
        'note': 'Check console output for 200 vs 429 split'
    }
    
    results.append(new_record)
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
        
    print(f'Done processing rate limiter test')
except Exception as e:
    print(f'Error processing results: {e}')
"
    
    echo "Test completed."
}

run_test

echo "All tests completed. Results:"
cat $OUTPUT_FILE
