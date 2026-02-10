#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"

if ! command -v k6 >/dev/null 2>&1; then
  echo "k6 bulunamadi. Kurulum: https://k6.io/docs/get-started/installation/"
  exit 1
fi

BASE_URL="${BASE_URL:-http://localhost:5080}"

if ! curl -s "$BASE_URL/health" >/dev/null 2>&1; then
  echo "Hedef API ayakta degil: $BASE_URL"
  echo "Once su komutu calistir:"
  echo "  ./scripts/run_dotnet_api.sh"
  exit 1
fi

mkdir -p results

echo "[1/3] smoke"
k6 run k6/01_smoke.js --summary-export results/01_smoke_summary.json

echo "[2/3] stages"
k6 run k6/02_stages.js --summary-export results/02_stages_summary.json

echo "[3/3] threshold fail (beklenen: fail)"
set +e
k6 run k6/03_threshold_fail.js --summary-export results/03_threshold_fail_summary.json
set -e

echo "Tamamlandi. Sonuclar: $ROOT_DIR/results"
