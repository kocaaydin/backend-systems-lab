#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - ThreadPool'da bloklayici islerin /fast endpoint'ini nasil yavaslattigini gormek.
# - Birden fazla blocking request'i paralel basip sonra /fast suresine bakmak.

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
source "${SCRIPT_DIR}/_common.sh"

BLOCK_COUNT="${BLOCK_COUNT:-20}"
BLOCK_MS="${BLOCK_MS:-4000}"

start_api
trap stop_api EXIT

step "Starvation senaryosunu baslatma"
echo "Starting starvation demo: BLOCK_COUNT=$BLOCK_COUNT BLOCK_MS=$BLOCK_MS"

for _ in $(seq 1 "$BLOCK_COUNT"); do
  curl -s "$BASE_URL/thread-types/starvation/blocking?blockMs=$BLOCK_MS" >/dev/null &
done

sleep 1

step "/fast latency kontrolu"
echo "Checking /fast latency under blocking pressure..."
for i in 1 2 3 4 5; do
  echo "fast call $i"
  curl -s -w "\nTOTAL_TIME=%{time_total}\n" "$BASE_URL/thread-types/fast"
  echo
done

wait
step "Starvation tamamlandi"
echo "Starvation demo completed."
