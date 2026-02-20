#!/bin/bash

cd "$(dirname "$0")"

URL_BASE="https://host.docker.internal"
ITERATIONS=1000
WARMUP_ITERATIONS=200
CONCURRENCY_LEVELS=(1 10 50 100)
RESULTS_DIR="results"

mkdir -p "$RESULTS_DIR"

echo "♻️  Docker ortamı yenileniyor..."
docker-compose down &> /dev/null
docker-compose up -d --build &> /dev/null
echo "✅ Konteynerler hazır! Uygulamanın ısınması için 5sn bekleniyor..."
sleep 5

echo "========================================================="
echo "🔬 HTTP/1.1 KEEP-ALIVE (TLS) TESTİ"
echo "   Hedef Base: $URL_BASE"
echo "   İstek Sayısı: $ITERATIONS (+ $WARMUP_ITERATIONS Isınma)"
echo "========================================================="

run_scenario() {
    local scenario_name=$1
    local script_file=$2
    local vus=$3
    local host_port=$4
    local warmup_host_port=$5

    echo ""
    echo "---------------------------------------------------------"
    echo "▶️  Senaryo: $scenario_name | VU: $vus | Warm-up Port: $warmup_host_port | Ölçüm Port: $host_port"
    echo "---------------------------------------------------------"

    local warmup_target_url="${URL_BASE}:${warmup_host_port}/api/benchmark/fast"
    local target_url="${URL_BASE}:${host_port}/api/benchmark/fast"

    echo "   🔥 Isınıyor ($WARMUP_ITERATIONS istek)..."
    docker run --rm -i grafana/k6 run \
        --vus $vus --iterations $WARMUP_ITERATIONS --quiet \
        --insecure-skip-tls-verify \
        --summary-trend-stats="avg,min,med,max,p(90),p(95),p(99)" \
        -e TARGET_URL=$warmup_target_url \
        -e PHASE=warmup \
        - < $script_file > /dev/null 2>&1

    local initial_timewait=$(netstat -an | grep $host_port | grep TIME_WAIT | wc -l)

    echo "   🚀 Test Başladı ($ITERATIONS istek)..."
    local summary_output_filename="summary_${scenario_name}_vu${vus}.json"

    docker run --rm -i \
        -v "$(pwd)/$RESULTS_DIR:/results" \
        -v "$(pwd)/k6:/scripts" \
        -e TARGET_URL=$target_url \
        -e PHASE=measure \
        grafana/k6 run \
        --insecure-skip-tls-verify \
        --vus $vus \
        --iterations $ITERATIONS \
        --summary-export "/results/$summary_output_filename" \
        --summary-trend-stats="avg,min,med,max,p(50),p(90),p(95),p(99)" \
        "/scripts/$(basename "$script_file")"

    local summary_json="$RESULTS_DIR/$summary_output_filename"
    sleep 1

    local parse_result="0,0,0,0,0"
    if [ -f "$summary_json" ]; then
        parse_result=$(python3 -c '
import json, sys
p = sys.argv[1]
d = json.load(open(p))
m = d["metrics"]["http_req_duration"]
rps = d["metrics"]["http_reqs"]["rate"]
print(f"{m['\''avg'\'']:.3f},{m['\''p(50)'\'']:.3f},{m['\''p(95)'\'']:.3f},{m['\''p(99)'\'']:.3f},{rps:.2f}")
' "$summary_json" 2>/dev/null || echo "0,0,0,0,0")
    fi

    local avg=$(echo $parse_result | cut -d, -f1)
    local p50=$(echo $parse_result | cut -d, -f2)
    local p95=$(echo $parse_result | cut -d, -f3)
    local p99=$(echo $parse_result | cut -d, -f4)
    local rps=$(echo $parse_result | cut -d, -f5)

    local final_timewait=$(netstat -an | grep $host_port | grep TIME_WAIT | wc -l)
    local timewait_diff=$((final_timewait - initial_timewait))
    if [ $timewait_diff -lt 0 ]; then timewait_diff=0; fi

    echo "$scenario_name,$vus,$host_port,$avg,$p50,$p95,$p99,$rps,$timewait_diff" >> "$RESULTS_DIR/report_http1_tls.csv"
}

echo "Senaryo,VU,Port,Ort(ms),P50(ms),P95(ms),P99(ms),RPS,Eklenen_TIME_WAIT" > "$RESULTS_DIR/report_http1_tls.csv"

for vu in "${CONCURRENCY_LEVELS[@]}"; do
    run_scenario "HTTP1KeepAliveOnTLS" "k6/keep_alive_on.js" $vu "15031" "15032"
    sleep 5
done

echo ""
echo "========================================================="
echo "📊 HTTP/1.1 + TLS TEST SONUÇ RAPORU"
echo "========================================================="
column -s, -t "$RESULTS_DIR/report_http1_tls.csv"
