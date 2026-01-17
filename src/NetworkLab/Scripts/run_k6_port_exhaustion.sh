#!/bin/bash

# NetworkLab - Port Exhaustion k6 Stress Test
# Bu test BAD HttpClient kullanımının port exhaustion'a yol açtığını gösterir

cd "$(dirname "$0")/../.."

echo "⚠️  PORT EXHAUSTION k6 STRESS TEST ⚠️"
echo "1200 VU × 15 saniye = ~18,000 istek"
echo "Beklenen: Port exhaustion ve connection errors"
echo ""

# Servisleri temiz başlat
echo "Servisleri yeniden başlatıyor..."
docker compose stop network-api external-api 2>/dev/null || true
docker compose rm -f network-api external-api 2>/dev/null || true
docker compose up -d --build network-api external-api
sleep 10

echo ""
echo "🔥 k6 STRESS TEST BAŞLIYOR..."
echo ""

# k6 test çalıştır
docker compose run --rm k6 run /scripts/NetworkLab/k6/port_exhaustion_stress.js

echo ""
echo "📊 TEST TAMAMLANDI"
echo ""

# Sonuçları göster
if [ -f "src/NetworkLab/Results/k6_port_exhaustion.json" ]; then
    echo "Sonuç özeti:"
    cat src/NetworkLab/Results/k6_port_exhaustion.json | jq '{
        total_requests: .metrics.http_reqs.values.count,
        failed_requests: .metrics.http_req_failed.values.passes,
        avg_duration: .metrics.http_req_duration.values.avg,
        p95_duration: .metrics.http_req_duration.values["p(95)"]
    }'
else
    echo "⚠️  Sonuç dosyası bulunamadı"
fi

echo ""
echo "Docker logs (hatalar):"
docker compose logs network-api --tail 50 | grep -i -E "(error|exception|exhaustion|cannot)" || echo "Hata bulunamadı"
