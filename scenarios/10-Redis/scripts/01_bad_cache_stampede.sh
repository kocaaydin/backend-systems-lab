#!/usr/bin/env bash
set -e

REDIS="docker compose exec -T redis redis-cli"

$REDIS DEL product:42 >/dev/null

miss_count=0
for i in $(seq 1 50); do
  v=$($REDIS GET product:42)
  if [ -z "$v" ]; then
    miss_count=$((miss_count + 1))
  fi
done

echo "[BAD] cache miss sayisi: $miss_count / 50"
echo "Yorum: Miss yuksekse backend'e toplu yuk biner (stampede)."
