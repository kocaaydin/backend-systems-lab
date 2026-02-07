select DB_NAME(id.database_id) as databaseName,
       id.statement as TableName,
       id.equality_columns,
       id.inequality_columns,
       id.included_columns,
       gs.last_user_seek,
       gs.user_seeks,
       gs.last_user_scan,
       gs.user_scans,
       gs.avg_total_user_cost * gs.avg_user_impact * (gs.user_seeks + gs.user_scans)/100 as ImprovementValue           
from sys.dm_db_missing_index_group_stats gs
INNER JOIN sys.dm_db_missing_index_groups ig on gs.group_handle = ig.index_group_handle
INNER JOIN sys.dm_db_missing_index_details id on id.index_handle = ig.index_handle
where DB_NAME(id.database_id) = 'FarmasiDB'
order by user_seeks desc
