#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
"$ROOT_DIR/run_n_20000.sh"
"$ROOT_DIR/run_n_200000.sh"

echo "N comparison completed: 20000 vs 200000"
