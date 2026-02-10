#!/usr/bin/env bash
set -e

echo "AuthN vs AuthZ kontrol listesi"
echo "- 401: kimlik dogrulanmadi (token yok/gecersiz)"
echo "- 403: kimlik var ama yetki yok"

echo
echo "Ornek curl (API'ne gore URL degistir):"
echo "curl -i http://localhost:5000/api/admin/report"
echo "curl -i -H 'Authorization: Bearer <token>' http://localhost:5000/api/admin/report"
