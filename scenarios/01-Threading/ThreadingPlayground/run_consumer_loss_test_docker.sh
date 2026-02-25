#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
RESULTS_DIR="$ROOT_DIR/results"
IMAGE_NAME="threading-playground-consumer-loss:local"

ITERATIONS="${1:-10000}"
WORK_MS="${2:-250}"
LINGER_MS="${3:-1500}"

mkdir -p "$RESULTS_DIR"
find "$RESULTS_DIR" -maxdepth 1 -type f -name 'consumer-loss-*' -delete

echo "[build] Docker image olusturuluyor: $IMAGE_NAME"
docker build -t "$IMAGE_NAME" -f "$ROOT_DIR/Dockerfile" "$ROOT_DIR"

echo "[1/2] taskrun senaryosu (docker)"
docker run --rm \
  -v "$RESULTS_DIR:/app/results" \
  "$IMAGE_NAME" consumer-loss \
  --scheduler taskrun \
  --iterations "$ITERATIONS" \
  --work-ms "$WORK_MS" \
  --linger-ms "$LINGER_MS" \
  --results-dir /app/results

echo
echo "[2/2] longrunning senaryosu (docker)"
docker run --rm \
  -v "$RESULTS_DIR:/app/results" \
  "$IMAGE_NAME" consumer-loss \
  --scheduler longrunning \
  --iterations "$ITERATIONS" \
  --work-ms "$WORK_MS" \
  --linger-ms "$LINGER_MS" \
  --results-dir /app/results

echo
echo "Tamamlandi. Ciktilar: $RESULTS_DIR"
