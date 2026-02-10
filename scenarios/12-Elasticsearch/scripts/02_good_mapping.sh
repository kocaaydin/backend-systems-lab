#!/usr/bin/env bash
set -e

ES="http://localhost:9201"

curl -s -X DELETE "$ES/products_good" >/dev/null || true
curl -s -X PUT "$ES/products_good" -H 'Content-Type: application/json' -d '{
  "mappings": {
    "properties": {
      "sku": { "type": "keyword" },
      "title": { "type": "text" }
    }
  }
}' >/dev/null

curl -s -X POST "$ES/products_good/_doc/1" -H 'Content-Type: application/json' -d '{"sku":"ABC-100","title":"wireless mouse"}' >/dev/null
curl -s -X POST "$ES/products_good/_doc/2" -H 'Content-Type: application/json' -d '{"sku":"ABC-101","title":"wireless keyboard"}' >/dev/null
curl -s -X POST "$ES/products_good/_refresh" >/dev/null

echo "[GOOD] sku keyword oldugu icin exact filtre beklenen gibi calisir."
curl -s "$ES/products_good/_search" -H 'Content-Type: application/json' -d '{"query":{"term":{"sku":"ABC-100"}}}'
echo
