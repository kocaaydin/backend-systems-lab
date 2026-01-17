#!/bin/bash

# NetworkLab - Scenario 2: HTTP/1.1 vs HTTP/2

cd "$(dirname "$0")/../.."

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
JSON_FILE="src/NetworkLab/Results/scenario2_${TIMESTAMP}.json"

echo "Stopping existing services for clean state..."
docker compose stop network-api external-api 2>/dev/null || true
docker compose rm -f network-api external-api 2>/dev/null || true

echo "Starting fresh NetworkLab API and external-api..."
docker compose up -d --build network-api external-api
echo "Waiting for services to be ready..."
sleep 10

if ! curl -s http://localhost:8085/api/connection/health > /dev/null 2>&1; then
    echo "ERROR: NetworkLab API failed to start!"
    exit 1
fi

PARALLEL_REQUESTS=20

echo "Testing HTTP/1.1 ($PARALLEL_REQUESTS parallel requests)..."
curl -s "http://localhost:8085/api/protocol/http1-test?parallelRequests=$PARALLEL_REQUESTS" > /tmp/http1.json

sleep 3

echo "Testing HTTP/2 ($PARALLEL_REQUESTS parallel requests)..."
curl -s "http://localhost:8085/api/protocol/http2-test?parallelRequests=$PARALLEL_REQUESTS" > /tmp/http2.json

# Process results - JSON only
python3 << 'PYTHON_SCRIPT'
import json
from datetime import datetime
import os

with open('/tmp/http1.json') as f:
    http1 = json.load(f)
with open('/tmp/http2.json') as f:
    http2 = json.load(f)

improvement = ((http1['durationMs'] - http2['durationMs']) / http1['durationMs']) * 100

results = {
    'scenario': 'HTTP/1.1 vs HTTP/2 Multiplexing',
    'test_date': datetime.now().strftime('%Y-%m-%d'),
    'test_time': datetime.now().strftime('%H:%M:%S'),
    'timestamp_utc': datetime.utcnow().isoformat(),
    'http1': http1,
    'http2': http2,
    'improvement_percent': round(improvement, 2)
}

json_file = os.environ.get('JSON_FILE')
with open(json_file, 'w') as f:
    json.dump(results, f, indent=2)

print(f'✅ Results saved: {json_file}')
print(f'HTTP/1.1: {http1["durationMs"]:.0f}ms')
print(f'HTTP/2:   {http2["durationMs"]:.0f}ms')
print(f'HTTP/2 is {improvement:.1f}% faster')
PYTHON_SCRIPT

echo ""
echo "View results: cat $JSON_FILE | jq"
