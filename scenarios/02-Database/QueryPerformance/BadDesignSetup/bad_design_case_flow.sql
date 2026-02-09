SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

/* =========================================================
   CASE 1: Non-SARGable filtre
   BAD : YEAR(OrderDate) = 2024
   FIX : Range predicate + index

   Diger Non-SARGable ornekler:
   - ISNULL(Status, 'Pending') = 'Pending'
   - COALESCE(Status, 'Pending') = 'Pending'
   - LEFT(Email, 3) = 'abc'
   - CAST(OrderDate AS DATE) = '2024-01-01'
   - LIKE '%Istanbul%'
========================================================= */
PRINT 'CASE 1 / BAD: YEAR(OrderDate)';
SELECT TOP 200
    o.Id,
    o.OrderDate,
    o.TotalAmount
FROM Orders o
WHERE YEAR(o.OrderDate) = 2024
  AND o.TotalAmount > 100
ORDER BY o.OrderDate DESC;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Orders')
      AND name = 'IX_Orders_OrderDate_TotalAmount'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Orders_OrderDate_TotalAmount
    ON dbo.Orders (OrderDate, TotalAmount)
    INCLUDE (Status, CustomerId);
END
GO

PRINT 'CASE 1 / GOOD: Date range predicate';
SELECT TOP 200
    o.Id,
    o.OrderDate,
    o.TotalAmount
FROM Orders o
WHERE o.OrderDate >= '2024-01-01'
  AND o.OrderDate <  '2025-01-01'
  AND o.TotalAmount > 100
ORDER BY o.OrderDate DESC;
GO

/* =========================================================
   CASE 2: Heap etkisi (Orders)
   BAD : Heap tabloda point query
   FIX : Clustered index
========================================================= */
PRINT 'CASE 2 / BAD: Heap table point query';
SELECT TOP 1 *
FROM Orders
WHERE Id = 2500000;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Orders')
      AND type = 1 -- clustered
)
BEGIN
    CREATE CLUSTERED INDEX CX_Orders_Id
    ON dbo.Orders (Id);
END
GO

PRINT 'CASE 2 / GOOD: Clustered table point query';
SELECT TOP 1 *
FROM Orders
WHERE Id = 2500000;
GO

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO
