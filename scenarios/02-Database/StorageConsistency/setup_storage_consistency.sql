USE master;
GO

IF DB_ID('StorageConsistencyLab') IS NULL
BEGIN
    CREATE DATABASE StorageConsistencyLab;
END
GO

USE StorageConsistencyLab;
GO

IF OBJECT_ID('dbo.InventoryMain', 'U') IS NOT NULL DROP TABLE dbo.InventoryMain;
IF OBJECT_ID('dbo.InventoryReplicaShadow', 'U') IS NOT NULL DROP TABLE dbo.InventoryReplicaShadow;
IF OBJECT_ID('dbo.OnCallDoctors', 'U') IS NOT NULL DROP TABLE dbo.OnCallDoctors;
GO

CREATE TABLE dbo.InventoryMain
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Sku NVARCHAR(50) NOT NULL UNIQUE,
    Stock INT NOT NULL,
    RowVersion ROWVERSION NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE dbo.InventoryReplicaShadow
(
    Id INT PRIMARY KEY,
    Sku NVARCHAR(50) NOT NULL UNIQUE,
    Stock INT NOT NULL,
    SourceUpdatedAt DATETIME2 NOT NULL,
    ReplicatedAt DATETIME2 NOT NULL
);

CREATE TABLE dbo.OnCallDoctors
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DoctorName NVARCHAR(100) NOT NULL,
    OnCall BIT NOT NULL
);
GO

INSERT INTO dbo.InventoryMain(Sku, Stock)
VALUES ('SKU-100', 10), ('SKU-200', 3);

INSERT INTO dbo.InventoryReplicaShadow(Id, Sku, Stock, SourceUpdatedAt, ReplicatedAt)
SELECT Id, Sku, Stock, UpdatedAt, SYSUTCDATETIME()
FROM dbo.InventoryMain;

INSERT INTO dbo.OnCallDoctors(DoctorName, OnCall)
VALUES ('Dr. A', 1), ('Dr. B', 1);
GO

PRINT 'StorageConsistencyLab setup tamamlandi.';
