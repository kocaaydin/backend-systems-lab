#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - Finalizer queue davranisini sayaclarla gormek.
# - create -> stats -> collect -> stats akisini tek scriptte toplamak.

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
source "${SCRIPT_DIR}/_common.sh"

COUNT="${COUNT:-50000}"

start_api
trap stop_api EXIT

step "1) Create finalizer samples (count=$COUNT)"
echo "1) Create finalizer samples (count=$COUNT)"
curl -s -X POST "$BASE_URL/thread-types/finalizer/create?count=$COUNT"
echo -e "\n"

step "2) Stats before collect"
echo "2) Stats before collect"
curl -s "$BASE_URL/thread-types/finalizer/stats"
echo -e "\n"

step "3) Force collect + wait finalizers"
echo "3) Force collect + wait finalizers"
curl -s -X POST "$BASE_URL/thread-types/finalizer/collect"
echo -e "\n"

step "4) Stats after collect"
echo "4) Stats after collect"
curl -s "$BASE_URL/thread-types/finalizer/stats"
echo -e "\n"
