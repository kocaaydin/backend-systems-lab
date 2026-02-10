#!/usr/bin/env bash
set -e

cd "$(dirname "$0")"
mkdir -p results

./scripts/01_authn_authz_examples.sh | tee results/01_authn_authz.txt
./scripts/02_cors_examples.sh | tee results/02_cors.txt
./scripts/03_tls_check.sh https://example.com | tee results/03_tls_check.txt

if command -v k6 >/dev/null 2>&1; then
  echo "k6 script hazir: k6 run k6/SecurityLab/auth_load.js"
else
  echo "k6 kurulu degil, sadece guvenlik kontrol scriptleri calisti"
fi
