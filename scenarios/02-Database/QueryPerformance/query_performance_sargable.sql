-- =========================================================
-- LABORATUVAR 2: SARGable OLMAYAN SORGULAR (TAM RESET)
-- Amaç: Tabloyu sıfırdan kurup, Index Seek vs Scan farkını görmek.
-- =========================================================
USE LabDb;
GO
SET NOCOUNT ON;
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
PRINT '>>> [1/4] TEMİZLİK: Orders tablosu siliniyor...';
IF OBJECT_ID('Orders', 'U') IS NOT NULL DROP TABLE Orders;
PRINT '>>> [2/4] KURULUM: Tablo oluşturuluyor ve veri basılıyor...';
CREATE TABLE Orders (
    Id INT IDENTITY(1, 1),
    -- Constraint YOK (Başlangıçta Heap)
    CustomerId INT,
    OrderDate DATETIME,
    TotalAmount DECIMAL(18, 2),
    Status NVARCHAR(50),
    ShippingAddress NVARCHAR(MAX)
);
-- Hızlı Veri Basma (500 Bin Satır - Test için yeterli ve hızlı)
-- Tarihleri 2023, 2024, 2025 yıllarına dağıtıyoruz.
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
        CROSS JOIN L0 AS B
),
-- 131K (2 ile çarp)
Nums AS (
    SELECT ROW_NUMBER() OVER(
            ORDER BY (
                    SELECT NULL
                )
        ) AS n
    FROM L5
        CROSS JOIN L0
) -- ~260K
INSERT INTO Orders (
        CustomerId,
        OrderDate,
        TotalAmount,
        Status,
        ShippingAddress
    )
SELECT TOP (500000) ABS(CHECKSUM(NEWID()) % 10000) + 1,
    -- Tarih Dağılımı: Son 3 yıl (Gün bazlı rastgele)
    DATEADD(DAY, - ABS(CHECKSUM(NEWID()) % 1000), GETDATE()),
    CAST(
        ABS(CHECKSUM(NEWID()) % 5000) / 10.0 AS DECIMAL(18, 2)
    ),
    'Pending',
    'Dummy Address'
FROM Nums
    CROSS JOIN L0;
-- Sayıyı artırmak için çarpraz birleşim
PRINT '    -> 500.000 Sipariş eklendi.';
PRINT '>>> [3/4] OPTİMİZASYON: Indexler oluşturuluyor...';
-- 1. Clustered Index
CREATE CLUSTERED INDEX CIX_Orders_Id ON Orders(Id);
PRINT '    -> Clustered Index oluşturuldu.';
-- 2. Non-Clustered Index (Tarih için)
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate ON Orders(OrderDate);
PRINT '    -> OrderDate Index oluşturuldu.';
GO -- =========================================================
    -- TEST AŞAMASI
    -- =========================================================
    PRINT '>>> [4/4] TEST BAŞLIYOR...';
PRINT '----------------------------------------------------';
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
PRINT 'TEST 1: YANLIŞ KULLANIM (Non-SARGable)';
PRINT 'Sorgu: WHERE YEAR(OrderDate) = 2024';
-- Bu sorgu INDEX SCAN yapmak zorundadır çünkü fonksiyon var.
SELECT COUNT(*) AS [2024_Siparisleri_Yanlis]
FROM Orders
WHERE YEAR(OrderDate) = 2024;
PRINT '----------------------------------------------------';
PRINT 'TEST 2: DOĞRU KULLANIM (SARGable)';
PRINT 'Sorgu: WHERE OrderDate >= 2024-01-01 ...';
-- Bu sorgu INDEX SEEK yapar, sadece ilgili aralığı okur.
SELECT COUNT(*) AS [2024_Siparisleri_Dogru]
FROM Orders
WHERE OrderDate >= '2024-01-01'
    AND OrderDate < '2025-01-01';
GO
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
PRINT '>>> BİTTİ.';