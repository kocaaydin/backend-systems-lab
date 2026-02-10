#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT_DIR"

if ! command -v docker >/dev/null 2>&1; then
  echo "docker bulunamadi."
  exit 1
fi

echo "k6 target api compose ile baslatiliyor..."
docker compose up -d --build k6-target-api

echo "API health bekleniyor..."
until curl -sf http://localhost:5080/health >/dev/null; do
  sleep 1
done

echo "API hazir: http://localhost:5080"
