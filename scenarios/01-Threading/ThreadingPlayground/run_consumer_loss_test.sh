#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_PATH="$ROOT_DIR/ThreadingPlayground.csproj"
RESULTS_DIR="$ROOT_DIR/results"

ITERATIONS="${1:-10000}"
WORK_MS="${2:-250}"
LINGER_MS="${3:-1500}"

mkdir -p "$RESULTS_DIR"
find "$RESULTS_DIR" -maxdepth 1 -type f -name 'consumer-loss-*' -delete

echo "[1/2] taskrun senaryosu"
dotnet run --project "$PROJECT_PATH" -- consumer-loss \
  --scheduler taskrun \
  --iterations "$ITERATIONS" \
  --work-ms "$WORK_MS" \
  --linger-ms "$LINGER_MS" \
  --results-dir "$RESULTS_DIR"

echo
echo "[2/2] longrunning senaryosu"
dotnet run --project "$PROJECT_PATH" -- consumer-loss \
  --scheduler longrunning \
  --iterations "$ITERATIONS" \
  --work-ms "$WORK_MS" \
  --linger-ms "$LINGER_MS" \
  --results-dir "$RESULTS_DIR"

echo
echo "Tamamlandi. Ciktilar: $RESULTS_DIR"
