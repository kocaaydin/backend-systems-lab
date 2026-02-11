#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - ThreadPool'da bloklayici islerin /fast endpoint'ini nasil yavaslattigini gormek.
# - Birden fazla blocking request'i paralel basip sonra /fast suresine bakmak.

BASE_URL="${BASE_URL:-http://localhost:8091}"
BLOCK_COUNT="${BLOCK_COUNT:-20}"
BLOCK_MS="${BLOCK_MS:-4000}"

echo "Starting starvation demo: BLOCK_COUNT=$BLOCK_COUNT BLOCK_MS=$BLOCK_MS"

for _ in $(seq 1 "$BLOCK_COUNT"); do
  curl -s "$BASE_URL/thread-types/starvation/blocking?blockMs=$BLOCK_MS" >/dev/null &
done

sleep 1

echo "Checking /fast latency under blocking pressure..."
for i in 1 2 3 4 5; do
  echo "fast call $i"
  curl -s -w "\nTOTAL_TIME=%{time_total}\n" "$BASE_URL/thread-types/fast"
  echo
done

wait
echo "Starvation demo completed."
