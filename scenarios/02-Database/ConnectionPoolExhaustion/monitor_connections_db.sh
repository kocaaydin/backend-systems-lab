#!/bin/bash

# Ensure we are in the script's directory
cd "$(dirname "$0")"

echo "Monitoring active connections directly from MSSQL..."
echo "Press [CTRL+C] to stop."
echo "---------------------------------------------------"

while true; do
    date_str=$(date '+%H:%M:%S')
    echo "[$date_str] Connection Count by Program:"
    
    # Run SQLCMD inside the docker container
    docker compose exec -T mssql /bin/sh -c "
        if [ -x /opt/mssql-tools18/bin/sqlcmd ]; then
          SQLCMD=/opt/mssql-tools18/bin/sqlcmd
          TLS='-C'
        elif [ -x /opt/mssql-tools/bin/sqlcmd ]; then
          SQLCMD=/opt/mssql-tools/bin/sqlcmd
          TLS=''
        else
          echo 'sqlcmd bulunamadi'
          exit 1
        fi

        \$SQLCMD \$TLS \
        -S localhost \
        -U sa \
        -P 'VeryStrongPassword123!' \
        -Y 30 \
        -Q \"
            SELECT count(*) as [Count], 
                   CASE 
                     WHEN program_name LIKE '%Core .Net SqlClient Data Provider%' THEN '.NET Client'
                     WHEN program_name LIKE 'SQLCMD%' THEN 'SQLCMD Monitor'
                     ELSE program_name 
                   END as [Program]
            FROM sys.dm_exec_sessions 
            WHERE is_user_process = 1
            GROUP BY program_name
            ORDER BY [Count] DESC
        \"
    " | grep -v "rows affected" | grep -v "^$"
    
    echo "---------------------------------------------------"
    sleep 2
done
