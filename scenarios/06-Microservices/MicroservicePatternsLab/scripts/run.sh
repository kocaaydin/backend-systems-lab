#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"
mkdir -p results

echo "MicroservicePatternsLab baslatiliyor..."
docker compose up -d --build

echo "Gateway health bekleniyor..."
until curl -sf http://localhost:8090/health >/dev/null; do
  sleep 2
done

echo "Ilk siparis olusturuluyor..."
ORDER_RESP=$(curl -sf -X POST "http://localhost:8090/api/orders" \
  -H "Content-Type: application/json" \
  -H "Idempotency-Key: demo-order-1" \
  -d '{"customerId":"CUST-1","sku":"SKU-1","quantity":2,"unitPrice":25.5}')
echo "$ORDER_RESP" | tee results/order_create.json

sleep 3

echo "Payment listesi aliniyor..."
curl -sf "http://localhost:8092/payments" | tee results/payments.json

echo "Inventory rezervasyonlari aliniyor..."
curl -sf "http://localhost:8093/inventory/reservations" | tee results/reservations.json

echo "Stock durumu..."
curl -sf "http://localhost:8093/inventory/stock/SKU-1" | tee results/stock.json

echo "k6 order load testi..."
docker compose run --rm --profile tools k6 run /scripts/create-orders.js --summary-export=/scripts/create-orders-summary.json
cp k6/MicroservicePatternsLab/create-orders-summary.json results/create-orders-summary.json

echo "k6 idempotency testi..."
docker compose run --rm --profile tools k6 run /scripts/idempotency.js --summary-export=/scripts/idempotency-summary.json
cp k6/MicroservicePatternsLab/idempotency-summary.json results/idempotency-summary.json

echo "Tamamlandi. Sonuclar: $ROOT_DIR/results"
