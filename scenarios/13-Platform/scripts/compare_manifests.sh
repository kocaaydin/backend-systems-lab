#!/usr/bin/env bash
set -e

echo "=== KARSILASTIRMA ==="
echo "[BAD] readiness/startup yok, liveness endpoint hatali."
echo "[GOOD] readiness+liveness+startup var, request/limit dengeli."

echo
echo "Bad probe satirlari:"
rg -n "Probe|path|resources|limits|requests" manifests/bad-deployment.yaml || true

echo
echo "Good probe satirlari:"
rg -n "Probe|path|resources|limits|requests" manifests/good-deployment.yaml || true
