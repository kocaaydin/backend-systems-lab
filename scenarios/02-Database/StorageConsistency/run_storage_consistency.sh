#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
SCENARIO_DIR="$ROOT_DIR/StorageConsistency"

cd "$ROOT_DIR"

echo "MSSQL container baslatiliyor..."
docker compose up -d mssql

SQLCMD=""
if docker compose exec -T mssql /bin/sh -c "[ -x /opt/mssql-tools18/bin/sqlcmd ]"; then
  SQLCMD="/opt/mssql-tools18/bin/sqlcmd -C"
elif docker compose exec -T mssql /bin/sh -c "[ -x /opt/mssql-tools/bin/sqlcmd ]"; then
  SQLCMD="/opt/mssql-tools/bin/sqlcmd"
else
  echo "sqlcmd bulunamadi"
  exit 1
fi

run_sql() {
  local file="$1"
  echo "Calisiyor: $file"
  docker compose exec -T mssql /bin/sh -c "$SQLCMD -S localhost -U sa -P 'VeryStrongPassword123!' -d master -i /tmp/$file"
}

copy_sql() {
  local file="$1"
  docker cp "$SCENARIO_DIR/$file" mssql_bad_design_lab:/tmp/$file
}

copy_sql setup_storage_consistency.sql
copy_sql replica_lag_stale_read_demo.sql
copy_sql write_skew_lost_update_demo.sql

run_sql setup_storage_consistency.sql
run_sql replica_lag_stale_read_demo.sql
run_sql write_skew_lost_update_demo.sql

echo "StorageConsistency senaryolari tamamlandi."
