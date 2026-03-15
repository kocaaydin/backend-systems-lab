#!/usr/bin/env bash
set -u

API_URL="http://localhost:8091"
COMPOSE_FILE="$(cd "$(dirname "$0")/.." && pwd)/docker-compose.yml"

echo "=== Senaryo 11: Graceful Shutdown + Queue Drain ==="
echo "Uzun queue isteği çalışırken API'ye SIGTERM göndereceğiz."
echo "Ne gözlemleyeceğiz: ASP.NET Core varsayılan olarak ~5 saniye (HostOptions.ShutdownTimeout)"
echo "kapanmayı geciktirip aktif isteklerin bitmesini bekler."

start_api() {
  echo "> API container başlatılıyor..."
  docker compose -f "$COMPOSE_FILE" down -t 2 >/dev/null 2>&1 || true
  sleep 2
  docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null
  echo "> Health kontrolü bekleniyor..."
  until curl -sf "$API_URL/health" >/dev/null; do
    sleep 1
  done
  echo "> API hazır."
}

# -------------------------------------------------------------------------
# Test 1: 3 Saniyelik Kısa İstek (Graceful Drain Tamamlanır)
# -------------------------------------------------------------------------
start_api
echo ""
echo "--- TEST 1: 3 Saniyelik İstek (Graceful Drain) ---"
echo "Beklenti: İstek çalışırken TERM gönderilir, ancak istek süresi"
echo "          5 saniyelik graceful limitinden az olduğu için tam sonuç alınır."

ENDPOINT="$API_URL/thread-types/queue/enqueue?items=15&capacity=5&workMs=200"

BODY_FILE_1="$(mktemp)"
T_START=$SECONDS

# Arkaplanda CURL çağrısı (yaklaşık 3 saniye sürecek)
curl -sS --max-time 15 -o "$BODY_FILE_1" -w "\nHTTP Code: %{http_code}\n" -X POST "$ENDPOINT" &
CURL_PID_1=$!

sleep 0.5
echo ">> API'ye SIGTERM (docker stop) gönderiliyor..."
docker stop -t 15 thread-types-api >/dev/null &
STOP_PID_1=$!

wait $CURL_PID_1 || echo "CURL Hata koduyla döndü."
T_END=$SECONDS
echo ">> Test 1 Bitiş Süresi: $((T_END - T_START)) saniye"
cat "$BODY_FILE_1"

# Stop komutunun tamamlanmasını bekle
wait $STOP_PID_1


# -------------------------------------------------------------------------
# Test 2: 24 Saniyelik Uzun İstek (Timeout/Force Kill Yaşanır)
# -------------------------------------------------------------------------
start_api
echo ""
echo "--- TEST 2: 24 Saniyelik İstek (Graceful Timeout Aşılır) ---"
echo "Beklenti: İstek çalışırken TERM gönderilecek. İstek süresi (.NET'in"
echo "          varsayılan 5s wait süresini) aşacağı için, süreç 5s sonra"
echo "          zorla kesilecek. Sonuçta CURL hatası (veya HTTP 499/empty)"
echo "          alınacaktır."

ENDPOINT="$API_URL/thread-types/queue/enqueue?items=120&capacity=5&workMs=200"

BODY_FILE_2="$(mktemp)"
T_START=$SECONDS

# Arkaplanda CURL çağrısı (yaklaşık 24 saniye sürecek)
curl -sS --max-time 30 -o "$BODY_FILE_2" -w "\nHTTP Code: %{http_code}\n" -X POST "$ENDPOINT" &
CURL_PID_2=$!

sleep 0.5
echo ">> API'ye SIGTERM (docker stop) gönderiliyor..."
docker stop -t 15 thread-types-api >/dev/null &
STOP_PID_2=$!

# CURL komutunun bitmesini bekle
wait $CURL_PID_2 || echo ">> CURL Hata koduyla döndü (Normal: Process zorla kapatıldı)."
T_END=$SECONDS
echo ">> Test 2 Bitiş Süresi: $((T_END - T_START)) saniye"
cat "$BODY_FILE_2"

# Stop komutunun tamamlanmasını bekle
wait $STOP_PID_2

echo ""
echo "=== Testler Tamamlandı ==="
