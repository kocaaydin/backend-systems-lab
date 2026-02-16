#!/bin/bash

# Configuration
API_URL="http://localhost:8091"
DOCKER_COMPOSE_FILE="$PWD/../docker-compose.yml"

echo "=== Senaryo 9: Full GC Freeze (Stop-The-World) Testi ==="
echo "Building and starting API container..."
docker compose -f "$DOCKER_COMPOSE_FILE" up -d --build thread-types-api

echo "Waiting for API..."
sleep 5

echo "Test 1: Sadece Kucuk Objelerle Donma Testi"
echo "----------------------------------------------------------"
curl -X POST "$API_URL/gc/freeze/small"
echo ""
echo ""

echo "Test 2: Sadece Buyuk Objelerle (LOH) Donma Testi"
echo "----------------------------------------------------------"
curl -X POST "$API_URL/gc/freeze/large"
echo ""
