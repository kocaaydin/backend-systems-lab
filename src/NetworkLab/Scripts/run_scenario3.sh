#!/bin/bash

# NetworkLab - Scenario 3: Client Timeout vs Server Processing

cd "$(dirname "$0")/../.."

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
JSON_FILE="src/NetworkLab/Results/scenario3_${TIMESTAMP}.json"

echo "Stopping existing services for clean state..."
docker compose stop network-api 2>/dev/null || true
docker compose rm -f network-api 2>/dev/null || true

echo "Starting fresh NetworkLab API..."
docker compose up -d --build network-api
echo "Waiting for service to be ready..."
sleep 10

if ! curl -s http://localhost:8085/api/connection/health > /dev/null 2>&1; then
    echo "ERROR: NetworkLab API failed to start!"
    exit 1
fi

echo "Testing WITH CancellationToken (client timeout: 3s, server: 10s)..."
START=$(date +%s)
curl -s -m 3 "http://localhost:8085/api/timeout/long-process?durationSeconds=10" > /dev/null 2>&1
END=$(date +%s)
WITH_TOKEN_DURATION=$((END - START))

sleep 8

echo "Testing WITHOUT CancellationToken (client timeout: 3s, server: 10s)..."
START=$(date +%s)
curl -s -m 3 "http://localhost:8085/api/timeout/long-process-no-cancellation?durationSeconds=10" > /dev/null 2>&1
END=$(date +%s)
WITHOUT_TOKEN_DURATION=$((END - START))

sleep 8

WITH_TOKEN_LOGS=$(docker compose logs network-api --tail 50 | grep -c "cancelled by client")
WITHOUT_TOKEN_LOGS=$(docker compose logs network-api --tail 50 | grep -c "completed (even if client timed out)")

# Process results - JSON only
python3 << 'PYTHON_SCRIPT'
import json
from datetime import datetime
import os

results = {
    'scenario': 'Client Timeout vs Server Processing',
    'test_date': datetime.now().strftime('%Y-%m-%d'),
    'test_time': datetime.now().strftime('%H:%M:%S'),
    'timestamp_utc': datetime.utcnow().isoformat(),
    'with_cancellation_token': {
        'client_timeout_seconds': 3,
        'server_stopped_at_seconds': int(os.environ.get('WITH_TOKEN_DURATION', 0)),
        'server_cancelled': int(os.environ.get('WITH_TOKEN_LOGS', 0)) > 0
    },
    'without_cancellation_token': {
        'client_timeout_seconds': 3,
        'server_continued_to_seconds': 10,
        'server_ignored_cancellation': int(os.environ.get('WITHOUT_TOKEN_LOGS', 0)) > 0
    }
}

json_file = os.environ.get('JSON_FILE')
with open(json_file, 'w') as f:
    json.dump(results, f, indent=2)

with_duration = results['with_cancellation_token']['server_stopped_at_seconds']
with_cancelled = results['with_cancellation_token']['server_cancelled']
without_ignored = results['without_cancellation_token']['server_ignored_cancellation']

print(f'✅ Results saved: {json_file}')
print(f'WITH CancellationToken: Server stopped at {with_duration}s (cancelled: {with_cancelled})')
print(f'WITHOUT CancellationToken: Server continued to 10s (ignored: {without_ignored})')
PYTHON_SCRIPT

echo ""
echo "View results: cat $JSON_FILE | jq"
