-- =========================================================
-- CLUSTered INDEX PERFORMANS LABORATUVARI (Heap vs Clustered)
-- Amaç: 10-20 Milyon satırlık bir tabloda Index'in etkisini görmek.
-- Not: 100 Milyon satır yaklaşık 10-15GB disk alanı ve ciddi süre gerektirir.
-- Bu lab için 10 Milyon satır (yaklaşık 1.5GB) performans farkını görmek için yeterlidir.
-- =========================================================

USE LabDb;
GO

SET NOCOUNT ON;

-- 1. TEMİZLİK ve TABLO OLUŞTURMA (Yapı: HEAP - Index Yok)
IF OBJECT_ID('BigTable', 'U') IS NOT NULL DROP TABLE BigTable;

CREATE TABLE BigTable (
    Id INT IDENTITY(1,1), -- Constraint YOK (Primary Key yok, Clustered Index yok) -> HEAP
    CustomerId INT,
    Amount DECIMAL(18,2),
    Padding CHAR(100),    -- Satırı şişirmek için (Page sayısını artırır, IO'yu patlatır)
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- 2. VERİ YÜKLEME (10 Milyon Satır)
-- Set-based yaklaşım (CTE) ile saniyeler içinde ekler.
PRINT '>>> Veri yükleniyor (10 Milyon Satır)... Tahmini süre: 1-2 dakika.';

WITH 
    L0   AS (SELECT c FROM (SELECT 1 UNION ALL SELECT 1) AS D(c)), -- 2
    L1   AS (SELECT 1 AS c FROM L0 AS A CROSS JOIN L0 AS B),       -- 4
    L2   AS (SELECT 1 AS c FROM L1 AS A CROSS JOIN L1 AS B),       -- 16
    L3   AS (SELECT 1 AS c FROM L2 AS A CROSS JOIN L2 AS B),       -- 256
    L4   AS (SELECT 1 AS c FROM L3 AS A CROSS JOIN L3 AS B),       -- 65,536
    L5   AS (SELECT 1 AS c FROM L4 AS A CROSS JOIN L3 AS B),       -- ~16 Milyon (256 * 65536)
    Nums AS (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM L5)

INSERT INTO BigTable (CustomerId, Amount, Padding, CreatedDate)
SELECT TOP (10000000) -- 10 Milyon (Disk dolmaması için 100M yerine 10M önerilir)
    ABS(CHECKSUM(NEWID()) % 100000) + 1, -- 1-100k arası CustomerId
    CAST(ABS(CHECKSUM(NEWID()) % 10000) / 100.0 AS DECIMAL(18,2)),
    'DUMMY DATA ' + CAST(n AS VARCHAR(20)),
    DATEADD(MINUTE, -n, GETDATE()) -- Geçmişe doğru tarih
FROM Nums;

PRINT '>>> Veri Yükleme Tamamlandı!';
GO

-- =========================================================
-- TEST 1: HEAP TABLO (Clustered Index YOK)
-- =========================================================
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

PRINT '------------------------------------------------';
PRINT 'TEST 1: HEAP TABLO (Tekil Kayıt Getirme)';
PRINT '------------------------------------------------';

-- Senaryo: ID'si 5.456.789 olan kaydı getir.
-- Beklenen: Table Scan (Tüm tabloyu okur). 
-- Logical Reads çok yüksek çıkacak (Örn: 150,000 page).
-- Süre: 1-3 saniye (Data RAM'de değilse daha uzun).

SELECT * FROM BigTable WHERE Id = 5456789;

PRINT '------------------------------------------------';
PRINT 'TEST 2: HEAP TABLO (Aralık Sorgusu)';
PRINT '------------------------------------------------';

-- Senaryo: Son 100 ID'yi getir.
-- Beklenen: Yine Table Scan! Sıralama olmadığı için son kayıtların nerede olduğunu bilemez.

SELECT * FROM BigTable WHERE Id BETWEEN 5456000 AND 5456100;

GO

-- =========================================================
-- 3. INDEX EKLEME AŞAMASI
-- (Sorguları çalıştırdıktan sonra bu satırı seçip çalıştırın)
-- Bu işlem tabloyu tamamen yeniden yazar ve sıraya dizer.
-- =========================================================

-- CREATE CLUSTERED INDEX CIX_BigTable_Id ON BigTable(Id);
-- GO

-- =========================================================
-- TEST 2: CLUSTERED INDEX VARKEN
-- =========================================================

-- PRINT '------------------------------------------------';
-- PRINT 'TEST 3: CLUSTERED INDEX (Tekil Kayıt Getirme)';
-- PRINT '------------------------------------------------';

-- -- Beklenen: Clustered Index Seek. 
-- -- Logical Reads: 3 veya 4 (Root -> Intermediate -> Leaf).
-- -- Süre: 0 ms (Anlık).

-- SELECT * FROM BigTable WHERE Id = 5456789;

-- PRINT '------------------------------------------------';
-- PRINT 'TEST 4: CLUSTERED INDEX (Aralık Sorgusu)';
-- PRINT '------------------------------------------------';

-- -- Beklenen: Clustered Index Seek + Partial Scan.
-- -- Sadece ilgili sayfaları okur.

-- SELECT * FROM BigTable WHERE Id BETWEEN 5456000 AND 5456100;

GO
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
