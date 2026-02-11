#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - Tek seferde hizli bir genel tur atmak.
# - Thread bilgisi, ThreadPool vs Dedicated farki ve temel queue akisina bakmak.
# - Derin senaryolar (starvation, cancellation, finalizer collect) ayri scriptlerde.

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
source "${SCRIPT_DIR}/_common.sh"

start_api
trap stop_api EXIT

step "1) info  (Hedef: mevcut request hangi thread tipinde calisiyor?)"
echo "1) info  (Hedef: mevcut request hangi thread tipinde calisiyor?)"
curl -s "$BASE_URL/thread-types/info"
echo -e "\n"

step "2) threadpool cpu-heavy  (Hedef: CPU isi ThreadPool'da nasil sure aliyor?)"
echo "2) threadpool cpu-heavy  (Hedef: CPU isi ThreadPool'da nasil sure aliyor?)"
curl -s "$BASE_URL/thread-types/cpu-heavy-threadpool?n=200000"
echo -e "\n"

step "3) dedicated cpu-heavy  (Hedef: Ayni is dedicated thread ile nasil fark ediyor?)"
echo "3) dedicated cpu-heavy  (Hedef: Ayni is dedicated thread ile nasil fark ediyor?)"
curl -s "$BASE_URL/thread-types/cpu-heavy-dedicated?n=200000"
echo -e "\n"

step "4) enqueue queue  (Hedef: Backpressure'i tek endpointte gormek)"
echo "4) enqueue queue  (Hedef: Backpressure'i tek endpointte gormek)"
curl -s -X POST "$BASE_URL/thread-types/queue/enqueue?items=20&capacity=5&workMs=400"
echo -e "\n"

step "5) finalizer stats  (Hedef: mevcut finalizer sayac durumunu gormek)"
echo "5) finalizer stats  (Hedef: mevcut finalizer sayac durumunu gormek)"
curl -s "$BASE_URL/thread-types/finalizer/stats"
echo -e "\n"

step "Sonraki adimlar"
echo "Ayrica calistir:"
echo "  - scripts/starvation_observation.sh   (ThreadPool darbogazi)"
echo "  - scripts/cancellation_observation.sh (Iptal edilen uzun CPU isi)"
echo "  - scripts/finalizer_observation.sh    (Create + Collect + Stats)"
