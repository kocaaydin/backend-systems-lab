USE StorageConsistencyLab;
GO

PRINT '--- CASE 2: Lost Update (Bad) vs RowVersion (Good) ---';

PRINT 'Adim 1: Lost update simulasyonu (son yazan kazanir).';
DECLARE @initial INT;
SELECT @initial = Stock FROM dbo.InventoryMain WHERE Sku = 'SKU-200';

-- Tx1 read
DECLARE @tx1_read INT = @initial;
-- Tx2 read
DECLARE @tx2_read INT = @initial;

-- Tx1 write: -1
UPDATE dbo.InventoryMain
SET Stock = @tx1_read - 1,
    UpdatedAt = SYSUTCDATETIME()
WHERE Sku = 'SKU-200';

-- Tx2 write: -1 (tx1 yazdigini ezebilir)
UPDATE dbo.InventoryMain
SET Stock = @tx2_read - 1,
    UpdatedAt = SYSUTCDATETIME()
WHERE Sku = 'SKU-200';

SELECT 'LOST_UPDATE_BAD' AS CaseName, Sku, Stock
FROM dbo.InventoryMain
WHERE Sku = 'SKU-200';

PRINT 'Adim 2: RowVersion ile optimistic concurrency (good).';
UPDATE dbo.InventoryMain
SET Stock = 3,
    UpdatedAt = SYSUTCDATETIME()
WHERE Sku = 'SKU-200';

DECLARE @rv BINARY(8);
DECLARE @stock INT;
SELECT @rv = RowVersion, @stock = Stock
FROM dbo.InventoryMain
WHERE Sku = 'SKU-200';

-- Tx1 başarılı update
UPDATE dbo.InventoryMain
SET Stock = @stock - 1,
    UpdatedAt = SYSUTCDATETIME()
WHERE Sku = 'SKU-200'
  AND RowVersion = @rv;

DECLARE @tx1Rows INT = @@ROWCOUNT;

-- Tx2 eski rowversion ile update denemesi (0 row update beklenir)
UPDATE dbo.InventoryMain
SET Stock = @stock - 1,
    UpdatedAt = SYSUTCDATETIME()
WHERE Sku = 'SKU-200'
  AND RowVersion = @rv;

DECLARE @tx2Rows INT = @@ROWCOUNT;

SELECT
    'OPTIMISTIC_CONCURRENCY_GOOD' AS CaseName,
    @tx1Rows AS Tx1Affected,
    @tx2Rows AS Tx2Affected,
    Sku,
    Stock
FROM dbo.InventoryMain
WHERE Sku = 'SKU-200';
GO

PRINT '--- CASE 3: Write Skew (doktor nöbeti) simulasyonu ---';
PRINT 'Kural: En az 1 doktor nobette kalmali.';

UPDATE dbo.OnCallDoctors SET OnCall = 1;

-- Tx1 ve Tx2 ayni anda toplam nobette sayisini 2 goruyor gibi simule edilir.
DECLARE @onCallSnapshotTx1 INT = (SELECT COUNT(*) FROM dbo.OnCallDoctors WHERE OnCall = 1);
DECLARE @onCallSnapshotTx2 INT = (SELECT COUNT(*) FROM dbo.OnCallDoctors WHERE OnCall = 1);

IF @onCallSnapshotTx1 >= 2
    UPDATE dbo.OnCallDoctors SET OnCall = 0 WHERE DoctorName = 'Dr. A';

IF @onCallSnapshotTx2 >= 2
    UPDATE dbo.OnCallDoctors SET OnCall = 0 WHERE DoctorName = 'Dr. B';

SELECT 'WRITE_SKEW_BAD' AS CaseName, DoctorName, OnCall
FROM dbo.OnCallDoctors;

SELECT 'WRITE_SKEW_BAD_SUMMARY' AS CaseName, COUNT(*) AS OnCallCount
FROM dbo.OnCallDoctors
WHERE OnCall = 1;
GO
