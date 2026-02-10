#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"

if ! command -v docker >/dev/null 2>&1; then
  echo "docker bulunamadi."
  exit 1
fi

./scripts/run_dotnet_api.sh

mkdir -p results

echo "[1/3] smoke"
docker compose --profile tools run --rm -e BASE_URL=http://k6-target-api:5080 k6 \
  run /scripts/01_smoke.js --summary-export /results/01_smoke_summary.json

echo "[2/3] stages"
docker compose --profile tools run --rm -e BASE_URL=http://k6-target-api:5080 k6 \
  run /scripts/02_stages.js --summary-export /results/02_stages_summary.json

echo "[3/3] threshold fail (beklenen: fail)"
set +e
docker compose --profile tools run --rm -e BASE_URL=http://k6-target-api:5080 k6 \
  run /scripts/03_threshold_fail.js --summary-export /results/03_threshold_fail_summary.json
set -e

echo "Tamamlandi. Sonuclar: $ROOT_DIR/results"
