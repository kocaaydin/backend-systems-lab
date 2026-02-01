#!/bin/bash

# Ensure we are in the script's directory
cd "$(dirname "$0")"

echo "=========================================="
echo " Connection Pool Exhaustion Test "
echo "=========================================="
echo "This test will start an API and MSSQL database."
echo "It will run a K6 load test to exhaust the DB connection pool."
echo "You should see connection timeouts or errors once the pool (size=10) is exhausted."
echo "LOGS ARE FILTERED TO SHOW ONLY RELEVANT INFO"
echo "=========================================="

# Build the API image (to include curl if needed)
echo "Building docker images..."
docker compose build -q api

# Start the environment and show logs
# We use grep to filter the noisy output, but we need to ensure exit code is preserved.
# Using a subshell or trap might be needed, but for simplicity let's just grep the stream.
# We will show:
# - [Pool] logs from API
# - [Monitor] logs from K6
# - k6-1 logs (general progress)
# - Any WARN or ERR logs from API/MSSQL

echo "Starting test. Output filtered for clarity..."
echo "---------------------------------------------------"

docker compose up --abort-on-container-exit --exit-code-from k6 2>&1 | \
    grep --line-buffered -E "\[Pool\]|\[Monitor\]|error|Error|WARN|fail|k6-1" | \
    grep -v "The attribute" | \
     grep -v "The requested image" 

# Note: PIPING changes the exit code to the last command (grep). 
# But visually this is what user wants.
# To be cleaner we could run clean up regardless.

# Cleanup
echo
echo "---------------------------------------------------"
echo "Test finished. Cleaning up..."
docker compose down > /dev/null 2>&1
