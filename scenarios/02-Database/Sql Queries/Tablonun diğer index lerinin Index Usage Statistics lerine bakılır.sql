select OBJECT_SCHEMA_NAME(us.object_id),OBJECT_NAME(us.object_id) as tableName,

    i.index_id,

                i.name as indexName,

                us.last_user_seek,

                us.user_seeks,

                CASE us.user_seeks WHEN 0 THEN 0

                               ELSE us.user_seeks*1.0 /(us.user_scans + us.user_seeks) * 100.0 END AS SeekPercentage,

                us.last_user_scan,

                us.user_scans,

                CASE us.user_scans WHEN 0 THEN 0

                               ELSE us.user_scans*1.0 /(us.user_scans + us.user_seeks) * 100.0 END AS ScanPercentage,

                us.last_user_lookup,

                us.user_lookups,

                us.last_user_update,

                us.user_updates,

                CASE us.user_scans + us.user_seeks WHEN 0 THEN 0

                               ELSE us.user_updates*1.0/(us.user_scans + us.user_seeks)*100.0 END as UpdatesPercentage            

FROM sys.dm_db_index_usage_stats us

INNER JOIN sys.indexes i ON i.object_id=us.object_id and i.index_id = us.index_id

where OBJECT_NAME(us.object_id) = 'Mellivo_FaturaDetay'
--WHERE us.database_id = DB_ID() and OBJECT_SCHEMA_NAME(us.object_id)+'.'+OBJECT_NAME(us.object_id) = 'Mellivo_FaturaDetay'