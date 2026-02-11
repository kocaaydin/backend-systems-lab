#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - Tek seferde hizli bir genel tur atmak.
# - Thread bilgisi, ThreadPool vs Dedicated farki ve temel queue akisina bakmak.
# - Derin senaryolar (starvation, cancellation, finalizer collect) ayri scriptlerde.

BASE_URL="${BASE_URL:-http://localhost:8091}"

echo "1) info  (Hedef: mevcut request hangi thread tipinde calisiyor?)"
curl -s "$BASE_URL/thread-types/info"
echo -e "\n"

echo "2) threadpool cpu-heavy  (Hedef: CPU isi ThreadPool'da nasil sure aliyor?)"
curl -s "$BASE_URL/thread-types/cpu-heavy-threadpool?n=200000"
echo -e "\n"

echo "3) dedicated cpu-heavy  (Hedef: Ayni is dedicated thread ile nasil fark ediyor?)"
curl -s "$BASE_URL/thread-types/cpu-heavy-dedicated?n=200000"
echo -e "\n"

echo "4) enqueue queue  (Hedef: Queue birikimi olusturmak)"
curl -s -X POST "$BASE_URL/thread-types/queue/enqueue?items=20&workMs=400"
echo -e "\n"

echo "5) queue status  (Hedef: queued / processed / activeWorkers gormek)"
curl -s "$BASE_URL/thread-types/queue/status"
echo -e "\n"

echo "6) finalizer stats  (Hedef: mevcut finalizer sayac durumunu gormek)"
curl -s "$BASE_URL/thread-types/finalizer/stats"
echo -e "\n"

echo "Ayrica calistir:"
echo "  - scripts/starvation_observation.sh   (ThreadPool darbogazi)"
echo "  - scripts/cancellation_observation.sh (Iptal edilen uzun CPU isi)"
echo "  - scripts/finalizer_observation.sh    (Create + Collect + Stats)"
