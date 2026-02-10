#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
APP_DIR="$ROOT_DIR/dotnet/K6TargetApi"

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet bulunamadi. .NET SDK gerekli."
  exit 1
fi

cd "$APP_DIR"
dotnet run
