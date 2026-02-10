#!/usr/bin/env bash
set -e

REDIS="docker compose exec -T redis redis-cli"
KEY="ratelimit:user:42"
LIMIT=10
WINDOW=5

$REDIS DEL "$KEY" >/dev/null
allowed=0
blocked=0

for i in $(seq 1 20); do
  count=$($REDIS INCR "$KEY")
  if [ "$count" -eq 1 ]; then
    $REDIS EXPIRE "$KEY" "$WINDOW" >/dev/null
  fi

  if [ "$count" -le "$LIMIT" ]; then
    allowed=$((allowed + 1))
  else
    blocked=$((blocked + 1))
  fi
done

echo "[RATE LIMIT] allowed=$allowed blocked=$blocked limit=$LIMIT/$WINDOW sn"
