#!/bin/bash

# NetworkLab - Port Exhaustion Stress Test
# Bu test BAD HttpClient kullanımının port exhaustion'a yol açtığını gösterir

cd "$(dirname "$0")/../.."

echo "⚠️  PORT EXHAUSTION STRESS TEST ⚠️"
echo "Bu test KASITLI OLARAK sistemi zorlar!"
echo ""

# Servisleri temiz başlat
echo "Servisleri yeniden başlatıyor..."
docker compose stop network-api external-api 2>/dev/null || true
docker compose rm -f network-api external-api 2>/dev/null || true
docker compose up -d --build network-api external-api
sleep 10

# Başlangıç metrikleri
INITIAL_PORTS=$(docker compose exec -T network-api sh -c "netstat -an | grep -c '49152:65535'" 2>/dev/null || echo "0")
echo "Başlangıç ephemeral port: $INITIAL_PORTS"
echo ""

# Agresif test - 500 istek (port exhaustion için)
echo "🔥 AGRESIF TEST BAŞLIYOR: 500 istek (Bad HttpClient)"
echo "Beklenen: Port exhaustion ve crash"
echo ""

START_TIME=$(date +%s)

# Test çalıştır ve hataları yakala
curl -s -X POST "http://localhost:8085/api/test/run-bad-scenario?requestCount=500" > /tmp/stress_test.json 2>&1
EXIT_CODE=$?

END_TIME=$(date +%s)
DURATION=$((END_TIME - START_TIME))

# Sonuç metrikleri
FINAL_PORTS=$(docker compose exec -T network-api sh -c "netstat -an | grep -c '49152:65535'" 2>/dev/null || echo "0")
TIME_WAIT=$(docker compose exec -T network-api sh -c "netstat -an | grep -c TIME_WAIT" 2>/dev/null || echo "0")

echo ""
echo "📊 TEST SONUÇLARI:"
echo "=================="
echo "Süre: ${DURATION}s"
echo "Exit Code: $EXIT_CODE"
echo "Başlangıç Ports: $INITIAL_PORTS"
echo "Final Ports: $FINAL_PORTS"
echo "TIME_WAIT: $TIME_WAIT"
echo ""

# Hata kontrolü
if [ $EXIT_CODE -ne 0 ]; then
    echo "❌ TEST BAŞARISIZ - Port exhaustion veya timeout!"
    echo ""
    echo "Hata detayı:"
    cat /tmp/stress_test.json | head -20
    echo ""
    echo "✅ BEKLENEN SONUÇ: Port exhaustion gerçekleşti!"
else
    echo "✅ Test tamamlandı (crash olmadı, daha fazla istek gerekebilir)"
    cat /tmp/stress_test.json | jq '.metrics.delta' 2>/dev/null || cat /tmp/stress_test.json
fi

echo ""
echo "Docker logs (son 20 satır):"
docker compose logs network-api --tail 20

echo ""
echo "Sonuç: src/NetworkLab/Results/port_exhaustion_test.json"

# Sonuçları kaydet
cat > src/NetworkLab/Results/port_exhaustion_test.json << EOF
{
  "test": "Port Exhaustion Stress Test",
  "timestamp": "$(date -u +%Y-%m-%dT%H:%M:%SZ)",
  "request_count": 500,
  "duration_seconds": $DURATION,
  "exit_code": $EXIT_CODE,
  "initial_ports": $INITIAL_PORTS,
  "final_ports": $FINAL_PORTS,
  "time_wait_connections": $TIME_WAIT,
  "crashed": $([ $EXIT_CODE -ne 0 ] && echo "true" || echo "false")
}
EOF
