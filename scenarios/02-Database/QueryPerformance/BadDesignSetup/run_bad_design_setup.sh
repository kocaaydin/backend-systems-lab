#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
COMPOSE_FILE="${SCRIPT_DIR}/../../docker-compose.yml"
SETUP_SQL="${SCRIPT_DIR}/bad_design_setup.sql"

# Ensure SQL Server is running, then execute seed script.
docker compose -f "${COMPOSE_FILE}" up -d mssql
docker compose -f "${COMPOSE_FILE}" run --rm \
  -v "${SETUP_SQL}:/scripts/bad_design_setup.sql:ro" \
  sql-runner /bin/bash -lc '
  set -e

  if [ -x /opt/mssql-tools18/bin/sqlcmd ]; then
    SQLCMD=/opt/mssql-tools18/bin/sqlcmd
  elif [ -x /opt/mssql-tools/bin/sqlcmd ]; then
    SQLCMD=/opt/mssql-tools/bin/sqlcmd
  else
    echo "sqlcmd binary not found in mssql-tools image"
    exit 1
  fi

  echo "Waiting for SQL Server to accept connections..."
  until $SQLCMD -S mssql -U sa -P "VeryStrongPassword123!" -Q "SELECT 1" -C >/dev/null 2>&1; do
    sleep 3
  done

  echo "Running bad_design_setup.sql..."
  $SQLCMD -S mssql -U sa -P "VeryStrongPassword123!" -d master -i /scripts/bad_design_setup.sql -C
  echo "Lab seed completed."
'
