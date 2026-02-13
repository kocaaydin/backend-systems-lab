#!/bin/bash

# Scriptin bulunduğu dizine geç (Böylece nereden çağırırsan çağır k6/ klasörünü bulur)
cd "$(dirname "$0")"

# Yapılandırma
# macOS'te Docker Desktop User-Proxy sorunundan kaçınmak için
# k6 konteynerini doğrudan uygulamanın ağına (Bridge) dahil ediyoruz.
# Böylece trafik Konteyner -> Konteyner akar (Gerçek Pod-to-Pod simülasyonu)
URL_BASE="http://connection-api"
ITERATIONS=1000
WARMUP_ITERATIONS=200
CONCURRENCY_LEVELS=(1)
RESULTS_DIR="results"

# Docker Ağı (docker-compose'da tanımladığımız explicit network)
DOCKER_NETWORK="bench-net"

mkdir -p $RESULTS_DIR

# 0. Ortamı Hazırla (Her test öncesi temiz ortam)
echo "♻️  Docker ortamı yenileniyor..."
docker-compose down &> /dev/null
docker-compose up -d --build &> /dev/null
echo "✅ Konteynerler hazır! Uygulamanın ısınması için 5sn bekleniyor..."
sleep 5

echo "========================================================="
echo "🔬 BAĞLANTI YÖNETİMİ TESTİ (HTTP/1.1)"
echo "   Hedef Base: $URL_BASE"
echo "   İstek Sayısı: $ITERATIONS (+ $WARMUP_ITERATIONS Isınma)"
echo "========================================================="

# Belirli bir senaryoyu çalıştırma fonksiyonu
run_scenario() {
    local scenario_name=$1
    local script_file=$2
    local vus=$3
    local keep_alive_status=$4 # "ACIK" veya "KAPALI"

    echo ""
    echo "---------------------------------------------------------"
    echo "▶️  Senaryo: $scenario_name | VU: $vus | Keep-Alive: $keep_alive_status"
    echo "---------------------------------------------------------"

    local target_port="5001"
    # Container içinde her zaman 5001'dir. Port mapping sadece dışarıyı etkiler.
    # Keep-Alive mantığı script içindeki Header ile kontrol edilir.
    
    local target_url="${URL_BASE}:${target_port}/api/benchmark/fast"

    # 1. Isınma (JIT/Pool ısınması için ön koşu)
    echo "   🔥 Isınıyor ($WARMUP_ITERATIONS istek)..."
    # Docker üzerinden çalıştır (Aynı Network İçinde)
    # --add-host gerekmez çünkü aynı networkte DNS çalışır.
    docker run --rm -i --network=$DOCKER_NETWORK grafana/k6 run \
        --vus $vus --iterations $WARMUP_ITERATIONS --quiet \
        --summary-trend-stats="avg,min,med,max,p(90),p(95),p(99)" \
        -e TARGET_URL=$target_url \
        - < $script_file > /dev/null 2>&1

    # 2. Başlangıç soket durumunu kaydet
    local initial_sockets=$(netstat -an | grep $target_port | wc -l)
    local initial_timewait=$(netstat -an | grep $target_port | grep TIME_WAIT | wc -l)

    # 3. Testi Başlat (JSON çıktısı alarak)
    echo "   🚀 Test Başladı ($ITERATIONS istek)..."
    local json_output_filename="${scenario_name}_vu${vus}.json"
    local summary_output_filename="summary_${scenario_name}_vu${vus}.json"
    
    # En temiz yöntem: Current directory'i mount et
    docker run --rm -i --network=$DOCKER_NETWORK \
        -v $(pwd)/$RESULTS_DIR:/results \
        -v $(pwd)/k6:/scripts \
        -e TARGET_URL=$target_url \
        grafana/k6 run \
        --vus $vus \
        --iterations $ITERATIONS \
        --summary-export /results/$summary_output_filename \
        --summary-trend-stats="avg,min,med,max,p(50),p(90),p(95),p(99)" \
        /scripts/$(basename $script_file)
    
    # JSON dosyasının yolu (host tarafında)
    local summary_json="$RESULTS_DIR/$summary_output_filename"

    # 4. Metrikleri Ayrıştır (Parse) - Python Script ile
    local parse_result=$(python3 $(dirname "$0")/parse_results.py $summary_json)
    
    # Python çıktısı: avg,p50,p95,p99,rps
    local avg=$(echo $parse_result | cut -d, -f1)
    local p50=$(echo $parse_result | cut -d, -f2)
    local p95=$(echo $parse_result | cut -d, -f3)
    local p99=$(echo $parse_result | cut -d, -f4)
    local rps=$(echo $parse_result | cut -d, -f5)

    # 5. Bitiş soket durumunu kaydet
    local final_sockets=$(netstat -an | grep $target_port | wc -l)
    local final_timewait=$(netstat -an | grep $target_port | grep TIME_WAIT | wc -l)
    local timewait_diff=$((final_timewait - initial_timewait))
    if [ $timewait_diff -lt 0 ]; then timewait_diff=0; fi

    # Sonuçları CSV raporuna ekle
    echo "$scenario_name,$vus,$keep_alive_status,$target_port,$avg,$p50,$p95,$p99,$rps,$timewait_diff" >> $RESULTS_DIR/report.csv
}

# Rapor başlığını hazırla
echo "Senaryo,VU,KeepAlive,Port,Ort(ms),P50(ms),P95(ms),P99(ms),RPS,Eklenen_TIME_WAIT" > $RESULTS_DIR/report.csv

# Senaryoları Döngüye Sok
for vu in "${CONCURRENCY_LEVELS[@]}"; do
    run_scenario "KeepAliveOff" "k6/keep_alive_off.js" $vu "KAPALI"
    
    # Soketlerin temizlenmesi için bekle
    sleep 5
    
    run_scenario "KeepAliveOn" "k6/keep_alive_on.js" $vu "ACIK"
    
    # Bekle
    sleep 5
done

echo ""
echo "========================================================="
echo "📊 TEST SONUC RAPORU"
echo "========================================================="
# CSV'yi tablo olarak bas
column -s, -t $RESULTS_DIR/report.csv

echo ""
echo "📌 Teknik Notlar:"
echo "   - Keep-Alive KAPALI: Her istekte yeni el sıkışma (SYN/SYN-ACK/ACK) maliyeti."
echo "   - Keep-Alive ACIK:   Sadece ilk istekte el sıkışma, sonrası saf veri transferi."
echo "   - Latency Farkı:     Network gidip-gelme (RTT) ve handshake süresidir."
echo "   - TIME_WAIT:         KAPALI senaryosunda yüksek çıkar (Kernel kapanan soketleri tutar)."
echo "========================================================="
