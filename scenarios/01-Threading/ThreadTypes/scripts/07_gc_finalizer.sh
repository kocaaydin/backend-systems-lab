//kontrol edilecek.

#!/bin/bash

# Configuration
API_URL="http://localhost:8091"
DOCKER_COMPOSE_FILE="$PWD/../docker-compose.yml"

echo "=== Senaryo 7: GC & Finalizer Queue Analizi ==="
echo "Building and starting API container..."
docker compose -f "$DOCKER_COMPOSE_FILE" up -d --build thread-types-api

echo "Waiting for API..."
sleep 5

echo "---------------------------------------------------"
ENDPOINT="$API_URL/gc/finalizer"
echo "Running Finalizer GC Test..."
echo "Requesting: POST $ENDPOINT"
echo "---------------------------------------------------"
curl -X POST "$ENDPOINT"
echo ""
