#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - Finalizer queue davranisini sayaclarla gormek.
# - create -> stats -> collect -> stats akisini tek scriptte toplamak.

BASE_URL="${BASE_URL:-http://localhost:8091}"
COUNT="${COUNT:-50000}"

echo "1) Create finalizer samples (count=$COUNT)"
curl -s -X POST "$BASE_URL/thread-types/finalizer/create?count=$COUNT"
echo -e "\n"

echo "2) Stats before collect"
curl -s "$BASE_URL/thread-types/finalizer/stats"
echo -e "\n"

echo "3) Force collect + wait finalizers"
curl -s -X POST "$BASE_URL/thread-types/finalizer/collect"
echo -e "\n"

echo "4) Stats after collect"
curl -s "$BASE_URL/thread-types/finalizer/stats"
echo -e "\n"
