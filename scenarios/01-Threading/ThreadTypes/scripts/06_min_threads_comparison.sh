#!/usr/bin/env bash
set -u

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
BASE_DIR="$(cd "$ROOT_DIR/.." && pwd)"
COMPOSE_FILE="$BASE_DIR/docker-compose.yml"
RESULTS_DIR="$BASE_DIR/results"

mkdir -p "$RESULTS_DIR"

echo "=== Senaryo 6: MinThreads Karşılaştırması (Default vs Tuned) ==="
echo "Amaç: Default (Min=1) ayarlar ile Tuned (Min=100) ayarların 'Hill Climbing' gecikmesine etkisini ölçmek."

restart_api() {
  echo "[api] Başlatılıyor..."
  docker compose -f "$COMPOSE_FILE" down --remove-orphans >/dev/null 2>&1 || true
  docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null

  until curl -sf http://localhost:8091/health >/dev/null; do
    sleep 1
  done
  echo "[api] Hazır."
}

run_test() {
  local MODE_NAME=$1
  local RESULT_FILE="$RESULTS_DIR/06-minthreads-$MODE_NAME.json"
  
  echo ""
  echo "--- Test Modu: $MODE_NAME ---"
  
  docker compose -f "$COMPOSE_FILE" --profile tools run --rm \
    -e SCENARIO_TYPE="pool" \
    -e DURATION="10s" \
    -e FAST_RPS="20" \
    -e HEAVY_RPS="50" \
    -e BLOCK_MS="500" \
    k6 run /scripts/03_io_bound_pool_vs_dedicated.js --summary-export "/results/06-minthreads-$MODE_NAME.json" >/dev/null
    
    echo "Test bitti. Sonuçlar analiz ediliyor..."
    # Python ile JSON'dan değerleri okuyalım
    python3 -c "
import json
import sys

try:
    with open('$RESULT_FILE', 'r') as f:
        data = json.load(f)
        
    metrics = data['metrics']
    
    # http_req_duration
    avg = metrics['http_req_duration']['avg']
    p95 = metrics['http_req_duration']['p(95)']
    
    print(f'   -> Avg Latency: {avg:.2f} ms')
    print(f'   -> P95 Latency: {p95:.2f} ms')
except Exception as e:
    print(f'Hata: {e}')
"
}

# --- Phase A: Default Settings (Min=1) ---
restart_api
echo "Varsayılan (Default) ayarlar ile test ediliyor..."
# Stats kontrol
curl -s http://localhost:8091/thread-types/pool-stats | jq .workerThreads || echo "(jq yok, ham json)"
run_test "default"


# --- Phase B: Tuned Settings (Min=100) ---
restart_api # Temiz başlangıç
echo ""
echo "Ayarlar optimize ediliyor (MinWorker=100)..."
curl -X POST "http://localhost:8091/thread-types/set-min-threads?minWorker=100&minIo=100"
echo ""
echo "Yeni ayarlar:"
curl -s http://localhost:8091/thread-types/pool-stats | jq .workerThreads || echo "(jq yok, ham json)"

run_test "tuned"

echo ""
echo "=== Karşılaştırma Tamamlandı ==="
