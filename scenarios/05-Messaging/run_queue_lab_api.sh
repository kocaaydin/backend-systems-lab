#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$ROOT_DIR/QueueLabApi"

echo "QueueLabApi starting on http://localhost:8090"
dotnet run
