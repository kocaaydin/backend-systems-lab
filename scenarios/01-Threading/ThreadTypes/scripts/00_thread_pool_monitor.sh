#!/usr/bin/env bash
set -u

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"

echo "=== Thread Pool Monitor & Saturation Test ==="
echo "Amaç: Yük altında Thread Pool'un nasıl büyüdüğünü (Hill Climbing) ve thread sayılarının değişimini gözlemlemek."

restart_api() {
  echo "[api] Başlatılıyor..."
  docker compose -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null

  until curl -sf http://localhost:8091/health >/dev/null; do
    sleep 1
  done
  echo "[api] Hazır."
}

restart_api

echo "Monitor başlatılıyor (CTRL+C ile durdurun)..."
echo "TS | Worker(Min/Max) | Active | Avail | Total | Pending | IO(Active/Avail)"
echo "---|---|---|---|---|---|---"

# Background monitoring loop
(
  while true; do
    TIMESTAMP=$(date +%H:%M:%S)
    RESPONSE=$(curl -s http://localhost:8091/thread-types/pool-stats)
    
    # Parse JSON (using python for reliability if jq is missing, but assuming python3 is available)
    # Output format: Min, Max, Active, Avail, Total, Pending
    STATS=$(echo "$RESPONSE" | python3 -c "import sys, json; 
try:
    d = json.load(sys.stdin); 
    w = d['workerThreads']; 
    io = d['completionPortThreads'];
    # Calculate Active (Max - Avail seems wrong if Max is huge, usually Active = Used. 
    # Actually GetAvailableThreads returns 'threads available to be started?'.
    # No, GetAvailableThreads = Max - CurrentlyActive. So Active = Max - Available.
    # Let's trust the controller's calculation: active = max - available (logic in controller was max-avail)
    # Wait, Controller logic: active = maxWorker - availWorker.
    # This logic assumes 'Max' is the hard limit.
    # Correct.
    
    # Let's just print what we have.
    print(f\"{w['min']}/{w['max']} {w['active']} {w['available']} {d['threadCount']} {d['pendingWorkItemCount']} {io['active']}/{io['available']}\")
except: print('Error')")

    if [ "$STATS" != "Error" ]; then
        printf "%s | %s\n" "$TIMESTAMP" "$STATS"
    fi
    sleep 1
  done
) &
MONITOR_PID=$!

trap "kill $MONITOR_PID 2>/dev/null" EXIT INT TERM

echo "--- Yük Testi Başlıyor (10s) ---"
echo "Senaryo: 50 RPS x 300ms Bloklama (ThreadPool'u tüketmek için)"

docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e SCENARIO_TYPE="pool" \
    -e DURATION="15s" \
    -e FAST_RPS="10" \
    -e HEAVY_RPS="50" \
    -e BLOCK_MS="1000" \
    k6 run /scripts/03_io_bound_pool_vs_dedicated.js >/dev/null

echo "Test bitti. Monitor durduruluyor..."
kill $MONITOR_PID
wait $MONITOR_PID 2>/dev/null
