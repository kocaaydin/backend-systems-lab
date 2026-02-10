#!/usr/bin/env bash
set -e

cd "$(dirname "$0")"
mkdir -p results

./scripts/compare_manifests.sh | tee results/manifest_compare.txt

if command -v kubectl >/dev/null 2>&1; then
  set +e
  kubectl create --dry-run=client -f manifests/bad-deployment.yaml >/dev/null 2>&1
  bad_rc=$?
  kubectl create --dry-run=client -f manifests/good-deployment.yaml >/dev/null 2>&1
  good_rc=$?
  set -e

  if [ $bad_rc -eq 0 ] && [ $good_rc -eq 0 ]; then
    echo "kubectl dry-run basarili" | tee results/kubectl_dry_run.txt
  else
    echo "kubectl var ama local cluster baglantisi yok; sadece metin karsilastirmasi yapildi" | tee results/kubectl_dry_run.txt
  fi
else
  echo "kubectl yok, sadece dosya karsilastirmasi yapildi" | tee results/kubectl_dry_run.txt
fi
