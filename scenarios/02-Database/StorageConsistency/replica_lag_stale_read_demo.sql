USE StorageConsistencyLab;
GO

PRINT '--- CASE 1: Replica Lag / Stale Read ---';
PRINT 'Adim 1: Main''de stok azaltiliyor (gercek write).';

UPDATE dbo.InventoryMain
SET Stock = Stock - 2,
    UpdatedAt = SYSUTCDATETIME()
WHERE Sku = 'SKU-100';

PRINT 'Adim 2: Simule replica henuz guncellenmedi -> stale read gorulecek.';
SELECT 'MAIN' AS Source, Sku, Stock, UpdatedAt AS Ts
FROM dbo.InventoryMain
WHERE Sku = 'SKU-100';

SELECT 'REPLICA_SHADOW' AS Source, Sku, Stock, ReplicatedAt AS Ts
FROM dbo.InventoryReplicaShadow
WHERE Sku = 'SKU-100';

PRINT 'Adim 3: Gecikmeli replica sync simule ediliyor.';
WAITFOR DELAY '00:00:02';

UPDATE r
SET r.Stock = m.Stock,
    r.SourceUpdatedAt = m.UpdatedAt,
    r.ReplicatedAt = SYSUTCDATETIME()
FROM dbo.InventoryReplicaShadow r
JOIN dbo.InventoryMain m ON m.Id = r.Id
WHERE m.Sku = 'SKU-100';

SELECT 'AFTER_SYNC_MAIN' AS Source, Sku, Stock, UpdatedAt AS Ts
FROM dbo.InventoryMain
WHERE Sku = 'SKU-100';

SELECT 'AFTER_SYNC_REPLICA' AS Source, Sku, Stock, ReplicatedAt AS Ts
FROM dbo.InventoryReplicaShadow
WHERE Sku = 'SKU-100';
GO
