-- =========================================================
-- SENARYO 1: Kötü Tasarım (Bad Design) Analizi
-- Amaç: Yanlış tasarlanmış bir veritabanında sorguların neden yavaş çalıştığını kod satırlarında görmek.
-- =========================================================

USE LabDb;
GO

-- Performans Değerlerini Açalım
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

-- -----------------------------------------------------------------------------------------
-- BÖLÜM 1: "Sipariş Listesi" Sorgusu
-- ÖNEMLİ: Sorguyu çalıştırmak için SELECT kelimesinden noktalı virgüle (;) kadar SEÇİP çalıştırın.
-- DBeaver bazen aradaki yorumlar yüzünden sorguyu bölebilir.
-- -----------------------------------------------------------------------------------------

PRINT '>>> YAVAŞ SORGU ÇALIŞIYOR: Sipariş Listeleme';

SELECT TOP 20
    o.Id AS OrderId,
    o.OrderDate,
    c.FirstName + ' ' + c.LastName AS CustomerName,
    o.TotalAmount,
    o.Status
FROM Orders o -- [SORUN 1: HEAP] Tablo yığın yapıda, her sorguda Full Scan.
INNER JOIN Customers c ON o.CustomerId = c.Id -- [SORUN 4: EKSİK INDEX] CustomerId'de index yok, join yavaş.
WHERE YEAR(o.OrderDate) = 2024 -- [SORUN 3: FONKSİYON] Index olsa bile boşa çıkarır.
AND o.TotalAmount > 100
AND o.ShippingAddress LIKE '%Istanbul%' -- [SORUN 2: LIKE] Başında % var, index kullanamaz.
ORDER BY o.OrderDate DESC;


-- -----------------------------------------------------------------------------------------
-- BÖLÜM 2: Gereksiz Joinler ve Örtülü Dönüşüm
-- -----------------------------------------------------------------------------------------

PRINT '>>> YAVAŞ SORGU ÇALIŞIYOR: Count ve Eksik Index';

SELECT TOP 100
    o.Id,
    (SELECT COUNT(*) FROM OrderItems oi WHERE oi.OrderId = o.Id) as ItemCount -- [SORUN 5: LOOP] Her satır için tekrar çalışır.
FROM Orders o
WHERE o.Status = 'Pending' -- [SORUN 6: SCAN] Status indexsiz.
ORDER BY o.Id DESC;

GO
SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
