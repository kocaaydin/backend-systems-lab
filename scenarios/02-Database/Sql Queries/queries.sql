Diğer index_lerin hangi kolonlar üzerinde olduğunu görmek için aşağıdaki sorguyu çalıştırabilirsiniz:
exec('sp_helpindex ''Mellivo_FaturaDetay'' ')


sp_configure 'optimize for ad hoc workloads',
1 RECONFIGURE


Tabloların boyutunu görmek için aşağıdaki sorguyu çalıştırabilirsiniz: (Kb)
SELECT 
    t.NAME AS TableName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS ToplamKB, 
    SUM(a.used_pages) * 8 AS KullanilmisKB, 
    (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS KalanKB
FROM 
    sys.tables t
INNER JOIN      
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN 
    sys.allocation_units a ON p.partition_id = a.container_id
LEFT OUTER JOIN 
    sys.schemas s ON t.schema_id = s.schema_id
WHERE 
    t.NAME NOT LIKE 'dt%' 
    AND t.is_ms_shipped = 0
    AND i.OBJECT_ID > 255 
GROUP BY 
    t.Name, s.Name, p.Rows
ORDER BY 
    SUM(a.total_pages) * 8 desc



--Top cpu queries

select 

     q.[text],

     SUBSTRING(q.text, (qs.statement_start_offset/2)+1, 

        ((CASE qs.statement_end_offset

          WHEN -1 THEN DATALENGTH(q.text)

         ELSE qs.statement_end_offset

         END - qs.statement_start_offset)/2) + 1) AS statement_text,        

     qs.last_execution_time,

     qs.execution_count,

     qs.total_worker_time/1000000 as total_cpu_time_sn,

     qs.total_worker_time/qs.execution_count/1000 as avg_cpu_time_ms,

     qp.query_plan,

     DB_NAME(q.dbid) as database_name,

     q.objectid,

     q.number,

     q.encrypted

from 

    (select top 50 

          qs.last_execution_time,

          qs.execution_count,

		  qs.plan_handle, 

          qs.total_worker_time,

          qs.statement_start_offset,

          qs.statement_end_offset

    from sys.dm_exec_query_stats qs

    order by qs.total_worker_time desc) qs

cross apply sys.dm_exec_sql_text(plan_handle) q

cross apply sys.dm_exec_query_plan(plan_handle) qp

order by qs.total_worker_time desc 

--Gereksiz indexleri görmek için aşağıdaki sorguyu çalıştırabilirsiniz:

SELECT TOP 25
o.name AS ObjectName
, i.name AS IndexName
, i.index_id AS IndexID
, dm_ius.user_seeks AS UserSeek
, dm_ius.user_scans AS UserScans
, dm_ius.user_lookups AS UserLookups
, dm_ius.user_updates AS UserUpdates
, p.TableRows
, 'DROP INDEX ' + QUOTENAME(i.name)
+ ' ON ' + QUOTENAME(s.name) + '.' + QUOTENAME(OBJECT_NAME(dm_ius.OBJECT_ID)) AS 'drop statement'
FROM sys.dm_db_index_usage_stats dm_ius
INNER JOIN sys.indexes i ON i.index_id = dm_ius.index_id AND dm_ius.OBJECT_ID = i.OBJECT_ID
INNER JOIN sys.objects o ON dm_ius.OBJECT_ID = o.OBJECT_ID
INNER JOIN sys.schemas s ON o.schema_id = s.schema_id
INNER JOIN (SELECT SUM(p.rows) TableRows, p.index_id, p.OBJECT_ID
FROM sys.partitions p GROUP BY p.index_id, p.OBJECT_ID) p
ON p.index_id = dm_ius.index_id AND dm_ius.OBJECT_ID = p.OBJECT_ID
WHERE OBJECTPROPERTY(dm_ius.OBJECT_ID,'IsUserTable') = 1
AND dm_ius.database_id = DB_ID()
AND i.type_desc = 'nonclustered'
AND i.is_primary_key = 0
AND i.is_unique_constraint = 0
ORDER BY (dm_ius.user_seeks + dm_ius.user_scans + dm_ius.user_lookups) ASC



--İndex rebuild
SELECT
      ps.object_id,
      i.name as IndexName,
      OBJECT_SCHEMA_NAME(ps.object_id) as ObjectSchemaName,
      OBJECT_NAME (ps.object_id) as ObjectName,
      ps.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL , NULL, 'LIMITED') ps
INNER JOIN sys.indexes i ON i.object_id=ps.object_id and i.index_id=ps.index_id
WHERE avg_fragmentation_in_percent > 30 AND ps.index_id > 0  
--AND OBJECT_NAME (ps.object_id) = 'Mellivo_ParUlke'
ORDER BY avg_fragmentation_in_percent desc



ALTER INDEX PK_Mellivo_CariBankaHesap
      ON Mellivo_CariAdres  REBUILD   WITH (ONLINE = ON)

	  ALTER INDEX PK_DEpo_FaturaKoli
      ON Depo_FaturaKoli  REBUILD   WITH (ONLINE = ON)


	  	  ALTER INDEX CariIDDonem
      ON Mellivo_AktiviteRaporuDetayli  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX BolgeIDDonem
      ON Mellivo_AktiviteRaporuDetayli  REBUILD   WITH (ONLINE = ON)

--Missing indexes

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

SELECT TOP 250
dm_mid.database_id AS DatabaseID,
dm_migs.avg_user_impact*(dm_migs.user_seeks+dm_migs.user_scans) Avg_Estimated_Impact,
dm_migs.last_user_seek AS Last_User_Seek,
OBJECT_NAME(dm_mid.OBJECT_ID,dm_mid.database_id) AS [TableName],
'CREATE INDEX [IX_' + OBJECT_NAME(dm_mid.OBJECT_ID,dm_mid.database_id) + '_'
+ REPLACE(REPLACE(REPLACE(ISNULL(dm_mid.equality_columns,''),', ','_'),'[',''),']','') +
CASE
WHEN dm_mid.equality_columns IS NOT NULL AND dm_mid.inequality_columns IS NOT NULL THEN '_'
ELSE ''
END
+ REPLACE(REPLACE(REPLACE(ISNULL(dm_mid.inequality_columns,''),', ','_'),'[',''),']','')
+ ']'
+ ' ON ' + dm_mid.statement
+ ' (' + ISNULL (dm_mid.equality_columns,'')
+ CASE WHEN dm_mid.equality_columns IS NOT NULL AND dm_mid.inequality_columns IS NOT NULL THEN ',' ELSE
'' END
+ ISNULL (dm_mid.inequality_columns, '')
+ ')'
+ ISNULL (' INCLUDE (' + dm_mid.included_columns + ')', '') AS Create_Statement
FROM sys.dm_db_missing_index_groups dm_mig
INNER JOIN sys.dm_db_missing_index_group_stats dm_migs
ON dm_migs.group_handle = dm_mig.index_group_handle
INNER JOIN sys.dm_db_missing_index_details dm_mid
ON dm_mig.index_handle = dm_mid.index_handle
WHERE dm_mid.database_ID = DB_ID()
ORDER BY Avg_Estimated_Impact DESC
GO

--indexnlerin kaç defa okunduğunu ve yazıldığını görmek için aşağıdaki sorguyu çalıştırabilirsiniz:
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

--Tablo satır sayıları 
SELECT [TableName] = so.name, [RowCount] = MAX(si.rows) FROM sysobjects so,  sysindexes si WHERE so.xtype = 'U' AND si.id = OBJECT_ID(so.name) GROUP BY so.name ORDER BY 2 DESC

-- Diğer indexlerinin index usage statisticslerine bakılır
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

-- Timeout 0
EXEC sp_configure 'remote query timeout', 0 