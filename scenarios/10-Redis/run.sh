#!/usr/bin/env bash
set -e

cd "$(dirname "$0")"
mkdir -p results

echo "Redis container baslatiliyor..."
docker compose up -d redis
sleep 2

echo "Case A: bad cache stampede"
./scripts/01_bad_cache_stampede.sh | tee results/01_bad_cache_stampede.txt

echo "Case B: good cache stampede"
./scripts/02_good_cache_stampede.sh | tee results/02_good_cache_stampede.txt

echo "Case C: merkezi rate limit"
./scripts/03_rate_limit_demo.sh | tee results/03_rate_limit_demo.txt

echo "Bitti. Sonuclar: scenarios/10-Redis/results"
