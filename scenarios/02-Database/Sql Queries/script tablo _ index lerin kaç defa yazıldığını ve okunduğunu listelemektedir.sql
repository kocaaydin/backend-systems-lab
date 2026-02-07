SELECT CAST(OBJECT_NAME(usg.OBJECT_ID) AS VARCHAR(30)) AS 'Table',
       CAST(idx.name AS VARCHAR(30)) AS 'Index',
       usg.user_seeks + usg.user_scans + usg.user_lookups AS 'Reads',
       usg.user_updates AS 'Writes'
FROM sys.dm_db_index_usage_stats AS usg
INNER join sys.indexes AS idx
ON usg.OBJECT_ID = idx.OBJECT_ID
and idx.index_id = usg.index_id
WHERE OBJECTPROPERTY(usg.OBJECT_ID,'isusertable') = 1
ORDER BY 'Table';