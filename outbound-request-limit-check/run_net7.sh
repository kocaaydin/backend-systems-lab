#!/bin/bash
# Script to run .NET 7 Throughput Test
cd "$(dirname "$0")/../outbound-request-limit-check"

# Ensure clean state
docker compose down --remove-orphans

echo "--- Setup for .NET 7 ---"
docker compose up -d --build target-api app-net7

echo "---------------------------------------------------"
echo "Running .NET 7 Throughput Test..."
docker compose run --rm -e APP_URL=http://app-net7:80/benchmark?count=2000 k6 run /scripts/benchmark.js

echo "Fetching logs for .NET 7..."
docker logs outbound-request-limit-check-app-net7-1

# Cleanup
docker compose down
