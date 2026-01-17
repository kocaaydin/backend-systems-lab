#!/bin/bash
# Script to run .NET 8 Throughput Test
cd "$(dirname "$0")/../outbound-request-limit-check"

# Ensure clean state
docker compose down --remove-orphans

echo "--- Setup for .NET 8 ---"
docker compose up -d --build target-api app-net8

echo "---------------------------------------------------"
echo "Running .NET 8 Throughput Test..."
docker compose run --rm -e APP_URL=http://app-net8:8080/benchmark?count=2000 k6 run /scripts/benchmark.js

echo "Fetching logs for .NET 8..."
docker logs outbound-request-limit-check-app-net8-1

# Cleanup
docker compose down
