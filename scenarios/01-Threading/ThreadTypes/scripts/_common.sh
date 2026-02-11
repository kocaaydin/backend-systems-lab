#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_PATH="${SCRIPT_DIR}/../ThreadTypesApi/ThreadTypesApi.csproj"
BASE_URL="${BASE_URL:-http://localhost:8091}"
API_PID=""
API_LOG="/tmp/threadtypes_api.log"

step() {
  echo
  echo "==> $1"
}

cleanup_existing_api() {
  step "Kalan eski API process'lerini temizleme"
  if command -v lsof >/dev/null 2>&1; then
    local pids
    pids="$(lsof -ti tcp:8091 || true)"
    if [ -n "${pids}" ]; then
      echo "Port 8091 kullanan process bulundu: ${pids}"
      kill ${pids} >/dev/null 2>&1 || true
      sleep 1
      pids="$(lsof -ti tcp:8091 || true)"
      if [ -n "${pids}" ]; then
        echo "Process hala acik, force kill: ${pids}"
        kill -9 ${pids} >/dev/null 2>&1 || true
      fi
    else
      echo "Port 8091 temiz."
    fi
  else
    echo "lsof yok, port temizligi atlandi."
  fi
}

start_api() {
  cleanup_existing_api
  step "ThreadTypes API baslatma"
  echo "Project: ${PROJECT_PATH}"
  dotnet run --project "${PROJECT_PATH}" > "${API_LOG}" 2>&1 &
  API_PID=$!
  echo "API PID: ${API_PID}"

  step "Health kontrol"
  for i in {1..60}; do
    if curl -sf "${BASE_URL}/health" >/dev/null; then
      echo "API hazir: ${BASE_URL}"
      return
    fi
    sleep 1
  done

  echo "API health timeout. Log:"
  tail -n 80 "${API_LOG}" || true
  exit 1
}

stop_api() {
  step "ThreadTypes API kapatma"
  if [ -n "${API_PID}" ]; then
    kill "${API_PID}" >/dev/null 2>&1 || true
    wait "${API_PID}" 2>/dev/null || true
  fi
  cleanup_existing_api
}
