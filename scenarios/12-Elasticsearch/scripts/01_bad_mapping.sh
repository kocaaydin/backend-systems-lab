#!/usr/bin/env bash
set -e

ES="http://localhost:9201"

curl -s -X DELETE "$ES/products_bad" >/dev/null || true
curl -s -X PUT "$ES/products_bad" -H 'Content-Type: application/json' -d '{
  "mappings": {
    "properties": {
      "sku": { "type": "text" },
      "title": { "type": "text" }
    }
  }
}' >/dev/null

curl -s -X POST "$ES/products_bad/_doc/1" -H 'Content-Type: application/json' -d '{"sku":"ABC-100","title":"wireless mouse"}' >/dev/null
curl -s -X POST "$ES/products_bad/_doc/2" -H 'Content-Type: application/json' -d '{"sku":"ABC-101","title":"wireless keyboard"}' >/dev/null
curl -s -X POST "$ES/products_bad/_refresh" >/dev/null

echo "[BAD] sku text oldugu icin exact filtrelerde yanlis/esnek eslesme riski artar."
curl -s "$ES/products_bad/_search" -H 'Content-Type: application/json' -d '{"query":{"term":{"sku":"ABC-100"}}}'
echo
