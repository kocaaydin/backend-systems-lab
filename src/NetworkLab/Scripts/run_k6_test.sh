#!/bin/bash

# NetworkLab - k6 Load Test

cd "$(dirname "$0")/../.."

OUTPUT_FILE="src/NetworkLab/Results/k6_result.json"

echo "Starting NetworkLab API..."
docker compose up -d network-api
sleep 5

echo "Running k6 load test (50 req/s for 60s)..."
docker compose run --rm k6 run /scripts/NetworkLab/k6/tcp-monitoring.js --summary-export=/scripts/k6-summary.json

# Process results
python3 -c "
import json
from datetime import datetime

try:
    with open('k6/k6-summary.json') as f:
        k6_data = json.load(f)
    
    metrics = k6_data['metrics']
    
    results = {
        'scenario': 'k6 Load Test - TCP Monitoring',
        'timestamp': datetime.utcnow().isoformat(),
        'requests_total': int(metrics.get('http_reqs', {}).get('count', 0)),
        'requests_failed': int(metrics.get('http_req_failed', {}).get('count', 0)),
        'avg_duration_ms': metrics.get('http_req_duration', {}).get('avg', 0),
        'p95_duration_ms': metrics.get('http_req_duration', {}).get('p(95)', 0),
        'p99_duration_ms': metrics.get('http_req_duration', {}).get('p(99)', 0)
    }
    
    with open('$OUTPUT_FILE', 'w') as f:
        json.dump(results, f, indent=2)
    
    print(f'Total requests: {results[\"requests_total\"]}')
    print(f'Failed: {results[\"requests_failed\"]}')
    print(f'P95 latency: {results[\"p95_duration_ms\"]:.2f}ms')
    print(f'Results saved to $OUTPUT_FILE')
except Exception as e:
    print(f'Error processing k6 results: {e}')
"

echo "Done. View results: cat $OUTPUT_FILE"
