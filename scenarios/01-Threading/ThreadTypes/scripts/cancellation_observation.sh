#!/usr/bin/env bash
set -euo pipefail

# Hedef:
# - Uzun CPU isini client tarafinda iptal ettigimizde ne oldugunu gormek.
# - max-time ile istek kesilip API'nin iptal akisi gozlemlenir.

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
source "${SCRIPT_DIR}/_common.sh"

N_VALUE="${N_VALUE:-450000}"
MAX_TIME="${MAX_TIME:-1}"

start_api
trap stop_api EXIT

step "Cancellation senaryosu"
echo "Cancellation demo: N=$N_VALUE, curl max-time=$MAX_TIME sec"

set +e
curl -sS --max-time "$MAX_TIME" "$BASE_URL/thread-types/cpu-cancellable?n=$N_VALUE"
EXIT_CODE=$?
set -e

step "Sonuc"
echo "curl process exit code: $EXIT_CODE"
echo "If exit code is non-zero, request was cut by client timeout as expected."
