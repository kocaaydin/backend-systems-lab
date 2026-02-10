#!/usr/bin/env bash
set -e

cd "$(dirname "$0")"
mkdir -p results

docker compose up -d mongo
sleep 3

echo "Seed calisiyor..."
docker compose exec -T mongo mongosh --quiet < scripts/01_seed.js | tee results/01_seed.txt

echo "Bad case (index yok)..."
docker compose exec -T mongo mongosh --quiet < scripts/02_bad_no_index.js | tee results/02_bad_no_index.txt

echo "Good case (index var)..."
docker compose exec -T mongo mongosh --quiet < scripts/03_good_with_index.js | tee results/03_good_with_index.txt

echo "Bitti. COLLSCAN vs IXSCAN farkini results dosyalarinda inceleyebilirsin."
