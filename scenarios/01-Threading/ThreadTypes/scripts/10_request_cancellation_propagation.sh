#!/usr/bin/env bash
set -u

API_URL="http://localhost:8091"
COMPOSE_FILE="$(cd "$(dirname "$0")/.." && pwd)/docker-compose.yml"

N_VALUE="${N_VALUE:-200000000}"
CHECK_EVERY="${CHECK_EVERY:-1}"
TIMEOUT_CANCEL_SECONDS="${TIMEOUT_CANCEL_SECONDS:-0.05}"
TIMEOUT_OK_SECONDS="${TIMEOUT_OK_SECONDS:-5}"

echo "=== Senaryo 10: Request Cancellation Propagation ==="
echo "Ayni endpoint icin iki test:"
echo "1) Dusuk timeout => iptal beklenir"
echo "2) Yeterli timeout => iptal olmamasi beklenir"
echo "Config: n=${N_VALUE} checkEvery=${CHECK_EVERY}"
echo "Timeout(cancel)=${TIMEOUT_CANCEL_SECONDS}s | Timeout(ok)=${TIMEOUT_OK_SECONDS}s"

echo "[1/3] API container başlatılıyor..."
docker compose -f "$COMPOSE_FILE" up -d --build thread-types-api >/dev/null

echo "[2/3] Health kontrolü..."
until curl -sf "$API_URL/health" >/dev/null; do
  sleep 1
done

echo "[3/3] Testler calisiyor..."
ENDPOINT="$API_URL/thread-types/cpu-cancellable?n=${N_VALUE}&checkEvery=${CHECK_EVERY}"

# Test A: Cancel beklenen senaryo
BODY_CANCEL_FILE="$(mktemp)"
echo ""
echo "--- Test A (Cancel Beklenen) ---"
echo "Endpoint: $ENDPOINT"
HTTP_CODE_CANCEL="$(curl -sS --max-time "$TIMEOUT_CANCEL_SECONDS" -o "$BODY_CANCEL_FILE" -w "%{http_code}" "$ENDPOINT" || true)"
BODY_CANCEL="$(cat "$BODY_CANCEL_FILE")"

if [ "$HTTP_CODE_CANCEL" = "000" ]; then
  echo "[KANIT] Iptal senaryosu: client timeout oldu (HTTP code=000)."
else
  echo "[BELIRSIZ] Beklenen timeout olmadi."
  echo "HTTP code: $HTTP_CODE_CANCEL"
  echo "Body: $BODY_CANCEL"
fi

# Test B: Cancel olmayan senaryo
BODY_OK_FILE="$(mktemp)"
echo ""
echo "--- Test B (Cancel Olmayan / Basarili) ---"
echo "Endpoint: $ENDPOINT"
HTTP_CODE_OK="$(curl -sS --max-time "$TIMEOUT_OK_SECONDS" -o "$BODY_OK_FILE" -w "%{http_code}" "$ENDPOINT" || true)"
BODY_OK="$(cat "$BODY_OK_FILE")"

if [ "$HTTP_CODE_OK" = "200" ] && echo "$BODY_OK" | grep -q '"cancelled":false'; then
  echo "[KANIT] Basarili senaryo: HTTP 200 ve cancelled=false."
  echo "$BODY_OK"
else
  echo "[BELIRSIZ] Basarili senaryo beklenen sonucu vermedi."
  echo "HTTP code: $HTTP_CODE_OK"
  echo "Body: $BODY_OK"
fi
