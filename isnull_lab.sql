-- =========================================================
-- LABORATUVAR 3: ISNULL (Non-SARGable) vs OR (SARGable)
-- Amaç: "ISNULL" fonksiyonunun index kullanımını nasıl engellediğini kanıtlamak.
-- =========================================================
USE LabDb;
GO
SET NOCOUNT ON;
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
PRINT '>>> [1/4] TEMİZLİK: Tablo siliniyor...';
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
PRINT '>>> [2/4] KURULUM: 2 Milyon satırlık tablo oluşturuluyor...';
CREATE TABLE Users (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    -- Clustered Index otomatik oluşur
    Username VARCHAR(50),
    Status VARCHAR(20) NULL -- NULL olabilir!
);
-- Veri Dağılımı:
-- %90 'Active'
-- %9  'Passive'
-- %1  NULL  (Yaklaşık 20.000 kayıt)
WITH L0 AS (
    SELECT c
    FROM (
            SELECT 1
            UNION ALL
            SELECT 1
        ) AS D(c)
),
L1 AS (
    SELECT 1 AS c
    FROM L0 AS A
        CROSS JOIN L0 AS B
),
L2 AS (
    SELECT 1 AS c
    FROM L1 AS A
        CROSS JOIN L1 AS B
),
L3 AS (
    SELECT 1 AS c
    FROM L2 AS A
        CROSS JOIN L2 AS B
),
-- 256
L4 AS (
    SELECT 1 AS c
    FROM L3 AS A
        CROSS JOIN L3 AS B
),
-- 65K
L5 AS (
    SELECT 1 AS c
    FROM L4 AS A
        CROSS JOIN L2 AS B
),
-- ~1M
Nums AS (
    SELECT ROW_NUMBER() OVER(
            ORDER BY (
                    SELECT NULL
                )
        ) AS n
    FROM L5
        CROSS JOIN L0
) -- 2M
INSERT INTO Users (Username, Status)
SELECT TOP (2000000) 'User_' + CAST(n AS VARCHAR(20)),
    CASE
        WHEN n % 100 = 0 THEN NULL -- %1 NULL
        WHEN n % 10 = 0 THEN 'Passive' -- %9 Passive
        ELSE 'Active' -- %90 Active
    END
FROM Nums;
PRINT '    -> 2 Milyon Kullanıcı eklendi.';
PRINT '>>> [3/4] INDEXLEME: Status kolonu indexleniyor...';
-- Status üzerinde index olmazsa zaten hepsi Scan olur. Farkı görmek için Index şart.
CREATE NONCLUSTERED INDEX IX_Users_Status ON Users(Status);
PRINT '    -> Index IX_Users_Status oluşturuldu.';
GO -- =========================================================
    -- TEST AŞAMASI
    -- =========================================================
    PRINT '>>> [4/4] TEST BAŞLIYOR...';
PRINT '----------------------------------------------------';
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
PRINT 'TEST 1: Non-SARGable (ISNULL Kullanımı)';
PRINT 'Sorgu: WHERE ISNULL(Status, "Empty") = "Empty"';
PRINT 'Senaryo: Status NULL ise veya zaten "Empty" ise getir.';
-- BEKLENTİ: INDEX SCAN. SQL Server her satırı kontrol etmek zorunda.
-- Index olsa bile "ISNULL" fonksiyonunun sonucunu indexte bulamaz.
SELECT COUNT(*) AS [Sayi_Yanlis]
FROM Users
WHERE ISNULL(Status, 'Empty') = 'Empty';
PRINT '----------------------------------------------------';
PRINT 'TEST 2: SARGable (OR Kullanımı)';
PRINT 'Sorgu: WHERE Status = "Empty" OR Status IS NULL';
-- BEKLENTİ: INDEX SEEK. 
-- SQL Server index ağacında "NULL" olan dala ve "Empty" olan dala doğrudan gider.
SELECT COUNT(*) AS [Sayi_Dogru]
FROM Users
WHERE Status = 'Empty'
    OR Status IS NULL;
GO
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
PRINT '>>> BİTTİ.';