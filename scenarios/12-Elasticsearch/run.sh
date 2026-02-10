#!/usr/bin/env bash
set -e

cd "$(dirname "$0")"
mkdir -p results

docker compose up -d elasticsearch

echo "Elasticsearch baslangici bekleniyor..."
until curl -s http://localhost:9201 >/dev/null; do
  sleep 2
done

./scripts/01_bad_mapping.sh | tee results/01_bad_mapping.txt
./scripts/02_good_mapping.sh | tee results/02_good_mapping.txt

if command -v k6 >/dev/null 2>&1; then
  k6 run k6/ElasticLab/search_load.js --summary-export results/03_k6_summary.json
else
  echo "k6 kurulu degil, k6 adimi atlandi."
fi

echo "Bitti. Sonuclar: scenarios/12-Elasticsearch/results"
