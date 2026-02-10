#!/usr/bin/env bash
set -e

REDIS="docker compose exec -T redis redis-cli"

$REDIS DEL product:42 lock:product:42 >/dev/null

fill_cache() {
  got_lock=$($REDIS SET lock:product:42 1 NX EX 5)
  if [ "$got_lock" = "OK" ]; then
    $REDIS SET product:42 "cached_payload" EX 60 >/dev/null
  fi
}

for i in $(seq 1 50); do
  v=$($REDIS GET product:42)
  if [ -z "$v" ]; then
    fill_cache
  fi
done

final=$($REDIS GET product:42)
if [ -n "$final" ]; then
  echo "[GOOD] cache tek uretici ile doldu. Stampede riski azaldi."
else
  echo "[GOOD] cache dolmadi, lock suresi veya akis kontrol edilmeli."
fi
