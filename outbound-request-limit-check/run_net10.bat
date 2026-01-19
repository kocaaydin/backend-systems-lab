@echo off
REM Script to run .NET 10 Throughput Test
cd /d "%~dp0"

REM Ensure clean state
docker compose down --remove-orphans

echo --- Setup for .NET 10 ---
docker compose up -d --build target-api app-net10

echo ---------------------------------------------------
echo Running .NET 10 Throughput Test...
docker compose run --rm -e APP_URL=http://app-net10:8080/benchmark?count=2000 k6 run /scripts/benchmark.js

echo Fetching logs for .NET 10...
docker logs outbound-request-limit-check-app-net10-1

REM Cleanup
docker compose down

pause
