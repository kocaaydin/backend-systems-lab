#!/usr/bin/env bash
set -e

cd "$(dirname "$0")"

if ! command -v k6 >/dev/null 2>&1; then
  echo "k6 bulunamadi"
  exit 1
fi

echo "Not: scriptler varsayilan olarak http://localhost:8090 endpointlerini bekler."

for s in k6/QueueLab/backpressure.js k6/QueueLab/poison.js k6/QueueLab/rebalance.js k6/QueueLab/hol.js k6/QueueLab/burst.js k6/QueueLab/tcp_saturation.js k6/QueueLab/backlog.js k6/QueueLab/churn.js; do
  echo "Calisiyor: $s"
  k6 run "$s" || true
  echo
  sleep 1
done
