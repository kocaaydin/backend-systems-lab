#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"

# Usage:
#   bash scripts/run.sh                 -> default N compare (20000 vs 200000)
#   bash scripts/run.sh 20000           -> single N
#   bash scripts/run.sh 20000 200000    -> multiple N
if [ "$#" -gt 0 ]; then
  for n_value in "$@"; do
    "$ROOT_DIR/run_n.sh" "$n_value"
  done
else
  exec "$ROOT_DIR/run_n_compare.sh"
fi
