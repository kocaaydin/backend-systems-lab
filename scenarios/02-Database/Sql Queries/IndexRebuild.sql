Use FarmasiDB
GO
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

	  	  ALTER INDEX ZincirID
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX Eposta1_FirmaID_Durum
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX DurumCariID
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX Durum
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX Eposta1
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX FaturaSayi_sil
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX DurumKayitTarihi
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

	  	  ALTER INDEX BolgeID
      ON Mellivo_Cari  REBUILD   WITH (ONLINE = ON)

