#!/usr/bin/env bash
set -e

TARGET="${1:-https://example.com}"

echo "TLS kontrol: $TARGET"
if command -v openssl >/dev/null 2>&1; then
  cert_output=$(echo | openssl s_client -servername "${TARGET#https://}" -connect "${TARGET#https://}:443" 2>/dev/null || true)
  if [ -n "$cert_output" ] && echo "$cert_output" | openssl x509 -noout -subject -issuer -dates >/dev/null 2>&1; then
    echo "$cert_output" | openssl x509 -noout -subject -issuer -dates
  else
    echo "Sertifika okunamadi. Local ag/TLS erisimi kisitli olabilir."
  fi
else
  echo "openssl yok, TLS kontrolu atlandi"
fi
