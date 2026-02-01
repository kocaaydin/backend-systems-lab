-- =============================================
-- 1. Veritabanı Kurulumu
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LabDb')
BEGIN
    CREATE DATABASE LabDb;
END
GO

USE LabDb;
GO

-- 2. Önceki Çalıştırmalardan Kalanları Temizle
IF OBJECT_ID('OrderItems', 'U') IS NOT NULL DROP TABLE OrderItems;
IF OBJECT_ID('Payments', 'U') IS NOT NULL DROP TABLE Payments;
IF OBJECT_ID('Orders', 'U') IS NOT NULL DROP TABLE Orders;
IF OBJECT_ID('Customers', 'U') IS NOT NULL DROP TABLE Customers;
IF OBJECT_ID('Products', 'U') IS NOT NULL DROP TABLE Products;
GO

-- =============================================
-- 3. Tabloları Oluştur (BİLEREK YAPILMIŞ KÖTÜ TASARIMLAR)
-- =============================================

-- Customers (Müşteriler): 1 Milyon satır. 
-- PK var ancak Address alanı NVARCHAR(MAX). CreatedDate üzerinde index yok.
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(200),
    Address NVARCHAR(MAX), -- DİKKAT: Memory grant sorunlarına yol açan "Performans Katili"
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Products (Ürünler): 200 Bin satır.
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200),
    Price DECIMAL(18,2),
    Stock INT,
    IsActive BIT
);

-- Orders (Siparişler): 5 Milyon satır. 
-- HEAP TABLO (Clustered Index yok!).
-- Bu durum, "Clustered Index Scan/Seek" yerine her zaman maliyetli "Table Scan" yapılmasına neden olur.
CREATE TABLE Orders (
    Id INT IDENTITY(1,1), -- Constraint yok! Sadece numaratör.
    CustomerId INT,
    OrderDate DATETIME,
    TotalAmount DECIMAL(18,2), -- Para birimi
    Status NVARCHAR(50),       -- Durum
    ShippingAddress NVARCHAR(MAX) -- Kötü tasarım: Text aramalarda çok yavaşlatır
);

-- OrderItems (Sipariş Detayları): 20 Milyon satır. 
-- PK yok (Primary Key yok). Bu tabloda joinler çok maliyetli olacak.
CREATE TABLE OrderItems (
    OrderId INT,
    ProductId INT,
    Quantity INT,
    UnitPrice DECIMAL(18,2)
);

-- Payments (Ödemeler): 5 Milyon satır.
CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    PaymentDate DATETIME,
    Amount DECIMAL(18,2),
    Provider NVARCHAR(50),
    TransactionDetail NVARCHAR(MAX)
);
GO

-- =============================================
-- 4. T-SQL ile Hızlı Veri Yükleme (Data Seeding)
-- Döngü kullanmadan, CTE (Common Table Expression) ile milyonlarca satırı saniyeler içinde oluşturuyoruz.
-- =============================================

SET NOCOUNT ON;

-- Yardımcı CTE: 1'den 4 Milyar'a kadar sayı üreten yapı (Loop kullanmadan).
WITH 
    L0   AS (SELECT c FROM (SELECT 1 UNION ALL SELECT 1) AS D(c)), -- 2 satır
    L1   AS (SELECT 1 AS c FROM L0 AS A CROSS JOIN L0 AS B),       -- 4 satır
    L2   AS (SELECT 1 AS c FROM L1 AS A CROSS JOIN L1 AS B),       -- 16 satır
    L3   AS (SELECT 1 AS c FROM L2 AS A CROSS JOIN L2 AS B),       -- 256 satır
    L4   AS (SELECT 1 AS c FROM L3 AS A CROSS JOIN L3 AS B),       -- 65K satır
    L5   AS (SELECT 1 AS c FROM L4 AS A CROSS JOIN L4 AS B),       -- 4 Milyar satır (Bunu limitleyeceğiz)
    Nums AS (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM L5)

-- A. Ürünleri Ekle (200K)
INSERT INTO Products (Name, Price, Stock, IsActive)
SELECT TOP (200000)
    'Urun ' + CAST(n AS NVARCHAR(20)),
    CAST(ABS(CHECKSUM(NEWID()) % 10000) / 100.0 AS DECIMAL(18,2)), -- Rastgele Fiyat
    ABS(CHECKSUM(NEWID()) % 1000), -- Rastgele Stok
    CASE WHEN (n % 10) = 0 THEN 0 ELSE 1 END -- %90 Aktif ürün
FROM Nums;

PRINT 'Ürünler (Products) eklendi.';

-- B. Müşterileri Ekle (1M)
WITH 
    L0 AS (SELECT 1 c UNION ALL SELECT 1),
    L1 AS (SELECT 1 c FROM L0 AS A CROSS JOIN L0 AS B),
    L2 AS (SELECT 1 c FROM L1 AS A CROSS JOIN L1 AS B),
    L3 AS (SELECT 1 c FROM L2 AS A CROSS JOIN L2 AS B),
    L4 AS (SELECT 1 c FROM L3 AS A CROSS JOIN L3 AS B),
    L5 AS (SELECT 1 c FROM L4 AS A CROSS JOIN L4 AS B),
    Nums AS (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM L5)

INSERT INTO Customers (FirstName, LastName, Email, Address, CreatedDate)
SELECT TOP (1000000)
    'Musteri' + CAST(n AS NVARCHAR(20)),
    'Soyad' + CAST(n AS NVARCHAR(20)),
    'kullanici' + CAST(n AS NVARCHAR(20)) + '@ornek.com',
    REPLICATE('Uzun Adres Bilgisi ', 10), -- NVARCHAR(MAX) maliyetini simüle etmek için veri şişiriyoruz
    DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 730), GETDATE()) -- Son 2 yıl içinden rastgele tarih
FROM Nums;

PRINT 'Müşteriler (Customers) eklendi.';

-- C. Siparişleri Ekle (5M)
-- Müşteri ID'leri 1 ile 1M arasında rastgele dağıtılır.
WITH 
    L0 AS (SELECT 1 c UNION ALL SELECT 1),
    L1 AS (SELECT 1 c FROM L0 AS A CROSS JOIN L0 AS B),
    L2 AS (SELECT 1 c FROM L1 AS A CROSS JOIN L1 AS B),
    L3 AS (SELECT 1 c FROM L2 AS A CROSS JOIN L2 AS B),
    L4 AS (SELECT 1 c FROM L3 AS A CROSS JOIN L3 AS B),
    L5 AS (SELECT 1 c FROM L4 AS A CROSS JOIN L4 AS B),
    Nums AS (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM L5)

INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, Status, ShippingAddress)
SELECT TOP (5000000)
    ABS(CHECKSUM(NEWID()) % 1000000) + 1,
    DATEADD(MINUTE, -ABS(CHECKSUM(NEWID()) % (365*24*60*2)), GETDATE()), -- Son 2 yıl içinde dakikalar
    CAST(ABS(CHECKSUM(NEWID()) % 5000) / 10.0 AS DECIMAL(18,2)),
    CASE (ABS(CHECKSUM(NEWID())) % 5) 
        WHEN 0 THEN 'Pending'    -- Bekliyor
        WHEN 1 THEN 'Shipped'    -- Kargolandı
        WHEN 2 THEN 'Delivered'  -- Teslim Edildi
        WHEN 3 THEN 'Cancelled'  -- İptal
        ELSE 'Returned'          -- İade
    END,
    REPLICATE('Teslimat Adresi ', 5)
FROM Nums;

PRINT 'Siparişler (Orders) eklendi.';

-- D. Sipariş Detaylarını Ekle (20M)
-- Her biri rastgele bir OrderId (1-5M) ve ProductId (1-200K) ile eşleşir.
WITH 
    L0 AS (SELECT 1 c UNION ALL SELECT 1),
    L1 AS (SELECT 1 c FROM L0 AS A CROSS JOIN L0 AS B),
    L2 AS (SELECT 1 c FROM L1 AS A CROSS JOIN L1 AS B),
    L3 AS (SELECT 1 c FROM L2 AS A CROSS JOIN L2 AS B),
    L4 AS (SELECT 1 c FROM L3 AS A CROSS JOIN L3 AS B),
    L5 AS (SELECT 1 c FROM L4 AS A CROSS JOIN L4 AS B),
    Nums AS (SELECT ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM L5)

INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
SELECT TOP (20000000)
    ABS(CHECKSUM(NEWID()) % 5000000) + 1, -- Rastgele OrderId
    ABS(CHECKSUM(NEWID()) % 200000) + 1,  -- Rastgele ProductId
    ABS(CHECKSUM(NEWID()) % 10) + 1,      -- Miktar 1-10
    CAST(ABS(CHECKSUM(NEWID()) % 1000) + 10 AS DECIMAL(18,2)) -- Rastgele Fiyat
FROM Nums;

PRINT 'Sipariş Detayları (OrderItems) eklendi.';
GO
