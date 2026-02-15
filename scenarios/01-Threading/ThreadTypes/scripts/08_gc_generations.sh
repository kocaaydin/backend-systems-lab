#!/bin/bash

# Configuration
API_URL="http://localhost:8091"
DOCKER_COMPOSE_FILE="$PWD/../docker-compose.yml"

echo "=== Senaryo 8: GC Generations (Gen0 -> Gen1 -> Gen2) Analizi ==="
echo "Building and starting API container..."
docker compose -f "$DOCKER_COMPOSE_FILE" up -d --build thread-types-api

echo "Waiting for API..."
sleep 5

echo "---------------------------------------------------"
ENDPOINT="$API_URL/gc/generations"
echo "Running Generations Test..."
echo "Requesting: POST $ENDPOINT"
echo "---------------------------------------------------"
curl -X POST "$ENDPOINT"
echo ""
