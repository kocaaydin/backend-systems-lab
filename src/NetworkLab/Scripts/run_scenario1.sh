#!/bin/bash

# NetworkLab - Scenario 1: Connection Pooling Test

cd "$(dirname "$0")/../.."

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
JSON_FILE="src/NetworkLab/Results/scenario1_${TIMESTAMP}.json"

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

echo "Running Good HttpClient test (50 requests)..."
curl -s -X POST "http://localhost:8085/api/test/run-good-scenario?requestCount=50" > /tmp/good.json

echo "Running Bad HttpClient test (50 requests)..."
curl -s -X POST "http://localhost:8085/api/test/run-bad-scenario?requestCount=50" > /tmp/bad.json

# Process results - JSON only
python3 << 'PYTHON_SCRIPT'
import json
from datetime import datetime
import os

with open('/tmp/good.json') as f:
    good = json.load(f)
with open('/tmp/bad.json') as f:
    bad = json.load(f)

results = {
    'scenario': 'Connection Pooling & TCP Reuse',
    'test_date': datetime.now().strftime('%Y-%m-%d'),
    'test_time': datetime.now().strftime('%H:%M:%S'),
    'timestamp_utc': datetime.utcnow().isoformat(),
    'good_httpclient': good['metrics'],
    'bad_httpclient': bad['metrics'],
    'comparison': {
        'ephemeral_port_reduction': good['metrics']['delta']['ephemeralPorts'] - bad['metrics']['delta']['ephemeralPorts'],
        'timewait_reduction': good['metrics']['delta']['timeWait'] - bad['metrics']['delta']['timeWait']
    }
}

json_file = os.environ.get('JSON_FILE')
with open(json_file, 'w') as f:
    json.dump(results, f, indent=2)

print(f'✅ Results saved: {json_file}')
print(f'Good: +{good["metrics"]["delta"]["ephemeralPorts"]} ports, +{good["metrics"]["delta"]["timeWait"]} TIME_WAIT')
print(f'Bad:  +{bad["metrics"]["delta"]["ephemeralPorts"]} ports, +{bad["metrics"]["delta"]["timeWait"]} TIME_WAIT')
PYTHON_SCRIPT

echo ""
echo "View results: cat $JSON_FILE | jq"
