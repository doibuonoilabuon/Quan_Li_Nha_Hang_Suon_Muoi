-- Run with Windows Authentication in SSMS. Existing data is retained.
IF DB_ID(N'LuaMocDB') IS NULL CREATE DATABASE [LuaMocDB];
GO
USE [LuaMocDB];
GO
-- Run in the application database. Renames preserve rows, IDs and foreign keys.
-- Safe to repeat. Ambiguous old/new table pairs fail before any change.
SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRANSACTION;
DECLARE @map TABLE (OldName sysname NOT NULL, NewName sysname NOT NULL);
INSERT @map VALUES (N'AppState',N'ApplicationState'),(N'CatalogVersions',N'CatalogRevisions'),(N'Users',N'UserAccounts'),(N'Foods',N'MenuItems'),(N'Services',N'ExtraServices'),(N'Recipes',N'RecipeIngredients'),(N'Reservations',N'TableReservations'),(N'Purchases',N'StockReceipts'),(N'PurchaseLines',N'StockReceiptItems'),(N'Orders',N'SalesOrders'),(N'OrderItems',N'SalesOrderItems'),(N'OrderServices',N'SalesOrderServices'),(N'Categories',N'MenuCategories'),(N'FoodOptions',N'MenuItemOptions'),(N'Combos',N'MealCombos'),(N'Vouchers',N'DiscountVouchers'),(N'Banners',N'PromotionalBanners'),(N'OpeningHours',N'BusinessHours'),(N'LegacyFoods',N'LegacyMenuItems'),(N'LegacyOrders',N'LegacySalesOrders');
IF EXISTS(SELECT 1 FROM @map WHERE OBJECT_ID(N'dbo.'+QUOTENAME(OldName),N'U') IS NOT NULL AND OBJECT_ID(N'dbo.'+QUOTENAME(NewName),N'U') IS NOT NULL)
 THROW 51001,'Both old and new table names exist. Resolve the duplicate tables before migration.',1;
DECLARE @old sysname,@new sysname,@object nvarchar(776);
DECLARE table_names CURSOR LOCAL FAST_FORWARD FOR SELECT OldName,NewName FROM @map;
OPEN table_names; FETCH NEXT FROM table_names INTO @old,@new;
WHILE @@FETCH_STATUS=0 BEGIN
 IF OBJECT_ID(N'dbo.'+QUOTENAME(@old),N'U') IS NOT NULL BEGIN
  SET @object=N'dbo.'+QUOTENAME(@old); EXEC sys.sp_rename @object,@new,N'OBJECT';
 END; FETCH NEXT FROM table_names INTO @old,@new; END;
CLOSE table_names; DEALLOCATE table_names;
DECLARE @objects TABLE(OldName sysname,NewName sysname);
INSERT @objects SELECT k.name,N'PK_'+t.name FROM sys.key_constraints k JOIN sys.tables t ON t.object_id=k.parent_object_id WHERE k.type=N'PK' AND t.schema_id=SCHEMA_ID(N'dbo') AND t.name IN (N'ApplicationState',N'CatalogRevisions',N'UserAccounts',N'MenuItems',N'ExtraServices',N'RecipeIngredients',N'TableReservations',N'StockReceipts',N'StockReceiptItems',N'SalesOrders',N'SalesOrderItems',N'SalesOrderServices',N'MenuCategories',N'MenuItemOptions',N'MealCombos',N'DiscountVouchers',N'PromotionalBanners',N'BusinessHours',N'LegacyMenuItems',N'LegacySalesOrders',N'Ingredients',N'DiningTables',N'Customers');
INSERT @objects SELECT o.name,STUFF(o.name,4,LEN(m.OldName),m.NewName) FROM sys.objects o JOIN sys.tables t ON t.object_id=o.parent_object_id JOIN @map m ON t.name=m.NewName WHERE o.type IN (N'F',N'C') AND LEFT(o.name,3) IN (N'FK_',N'CK_') AND SUBSTRING(o.name,4,LEN(m.OldName))=m.OldName AND (LEN(o.name)=3+LEN(m.OldName) OR SUBSTRING(o.name,4+LEN(m.OldName),1)=N'_') AND o.is_ms_shipped=0;
INSERT @objects SELECT name,CASE WHEN parent_column_id=COLUMNPROPERTY(parent_object_id,N'Role',N'ColumnId') THEN N'CK_UserAccounts_Role' ELSE N'CK_UserAccounts_Iterations' END FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.UserAccounts') AND is_system_named=1;
DECLARE constraint_names CURSOR LOCAL FAST_FORWARD FOR SELECT OldName,NewName FROM @objects WHERE OldName<>NewName;
OPEN constraint_names; FETCH NEXT FROM constraint_names INTO @old,@new;
WHILE @@FETCH_STATUS=0 BEGIN
 SET @object=N'dbo.'+QUOTENAME(@old); EXEC sys.sp_rename @object,@new,N'OBJECT';
 FETCH NEXT FROM constraint_names INTO @old,@new; END;
CLOSE constraint_names; DEALLOCATE constraint_names;
IF EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.TableReservations') AND name=N'IX_Reservations_TableTime') EXEC sys.sp_rename N'dbo.TableReservations.IX_Reservations_TableTime',N'IX_TableReservations_TableTime',N'INDEX';
IF EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.SalesOrders') AND name=N'IX_Orders_PaidAt') EXEC sys.sp_rename N'dbo.SalesOrders.IX_Orders_PaidAt',N'IX_SalesOrders_PaidAt',N'INDEX';
IF OBJECT_ID(N'dbo.SalesOrders',N'U') IS NOT NULL BEGIN
 EXEC(N'CREATE OR ALTER VIEW dbo.vOrderTotals AS
SELECT o.*, ISNULL(i.Amount,0)+ISNULL(e.Amount,0) AS Subtotal,
 ISNULL(i.Amount,0)+ISNULL(e.Amount,0)-o.Discount AS Total
FROM dbo.SalesOrders o
OUTER APPLY(SELECT SUM(Quantity*UnitPrice) Amount FROM dbo.SalesOrderItems WHERE OrderId=o.Id) i
OUTER APPLY(SELECT SUM(Quantity*UnitPrice) Amount FROM dbo.SalesOrderServices WHERE OrderId=o.Id) e;');
 EXEC(N'CREATE OR ALTER VIEW dbo.vRevenueMonthly AS
SELECT YEAR(PaidAt) AS [Year],MONTH(PaidAt) AS [Month],COUNT_BIG(*) AS Receipts,SUM(Subtotal) AS Gross,SUM(Discount) AS Discount,SUM(Total) AS Net
FROM dbo.vOrderTotals WHERE Status=N''Đã thanh toán'' GROUP BY YEAR(PaidAt),MONTH(PaidAt);');
 EXEC(N'CREATE OR ALTER VIEW dbo.vRevenueQuarterly AS
SELECT YEAR(PaidAt) AS [Year],DATEPART(quarter,PaidAt) AS [Quarter],COUNT_BIG(*) AS Receipts,SUM(Subtotal) AS Gross,SUM(Discount) AS Discount,SUM(Total) AS Net
FROM dbo.vOrderTotals WHERE Status=N''Đã thanh toán'' GROUP BY YEAR(PaidAt),DATEPART(quarter,PaidAt);');
 EXEC(N'CREATE OR ALTER VIEW dbo.vBestSellingFoods AS
SELECT i.FoodId,i.Name,SUM(CONVERT(bigint,i.Quantity)) AS Quantity,SUM(i.Quantity*i.UnitPrice) AS Revenue
FROM dbo.SalesOrderItems i JOIN dbo.SalesOrders o ON o.Id=i.OrderId WHERE o.Status=N''Đã thanh toán'' GROUP BY i.FoodId,i.Name;');
 EXEC(N'CREATE OR ALTER VIEW dbo.vTableStatus AS
SELECT t.*,CASE WHEN EXISTS(SELECT 1 FROM dbo.SalesOrders o WHERE o.TableId=t.Id AND o.Status=N''Đang phục vụ'') THEN N''Đang phục vụ''
 WHEN EXISTS(SELECT 1 FROM dbo.TableReservations r WHERE r.TableId=t.Id AND r.Status=N''Đã đặt'' AND r.Time BETWEEN DATEADD(hour,-2,GETDATE()) AND DATEADD(hour,2,GETDATE())) THEN N''Đã đặt'' ELSE N''Trống'' END AS Status
FROM dbo.DiningTables t;');
END;
COMMIT;
END TRY
BEGIN CATCH
IF XACT_STATE()<>0 ROLLBACK;
THROW;
END CATCH;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF OBJECT_ID(N'dbo.ApplicationState', N'U') IS NULL
CREATE TABLE dbo.ApplicationState (
 Id tinyint NOT NULL CONSTRAINT PK_ApplicationState PRIMARY KEY CONSTRAINT CK_ApplicationState_Id CHECK (Id=1),
 Version int NOT NULL, Revision bigint NOT NULL, ReceiptSequence int NOT NULL, PurchaseSequence int NOT NULL,
 Initialized bit NOT NULL, ImportedAt datetime2(7) NULL);
IF NOT EXISTS(SELECT 1 FROM dbo.ApplicationState) INSERT dbo.ApplicationState VALUES(1,1,0,0,0,0,NULL);
IF OBJECT_ID(N'dbo.CatalogRevisions',N'U') IS NULL CREATE TABLE dbo.CatalogRevisions (ModuleKey nvarchar(64) NOT NULL CONSTRAINT PK_CatalogRevisions PRIMARY KEY, Revision bigint NOT NULL);
IF OBJECT_ID(N'dbo.UserAccounts',N'U') IS NULL CREATE TABLE dbo.UserAccounts (
 Username nvarchar(128) NOT NULL CONSTRAINT PK_UserAccounts PRIMARY KEY, DisplayName nvarchar(256) NOT NULL,
 Role nvarchar(16) NOT NULL CONSTRAINT CK_UserAccounts_Role CHECK(Role IN (N'Admin',N'Staff')),
 PasswordHash varbinary(32) NOT NULL, Salt varbinary(16) NOT NULL,
 Iterations int NOT NULL CONSTRAINT CK_UserAccounts_Iterations CHECK(Iterations>=600000), Active bit NOT NULL);
IF OBJECT_ID(N'dbo.MenuItems',N'U') IS NULL CREATE TABLE dbo.[MenuItems] (
 [Id] nvarchar(64) NOT NULL,
 [Name] nvarchar(256) NOT NULL,
 [Category] nvarchar(128) NOT NULL,
 [Price] decimal(28,8) NOT NULL,
 [Active] bit NOT NULL,
 [ImagePath] nvarchar(2048) NOT NULL,
 [Instructions] nvarchar(max) NOT NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY ([Id]),
 CONSTRAINT [CK_MenuItems] CHECK (Price > 0)
);
IF OBJECT_ID(N'dbo.Ingredients',N'U') IS NULL CREATE TABLE dbo.[Ingredients] (
 [Id] nvarchar(64) NOT NULL,
 [Name] nvarchar(256) NOT NULL,
 [Unit] nvarchar(32) NOT NULL,
 [Stock] decimal(28,8) NOT NULL,
 [Minimum] decimal(28,8) NOT NULL,
 [AverageCost] decimal(38,18) NOT NULL,
 CONSTRAINT [PK_Ingredients] PRIMARY KEY ([Id]),
 CONSTRAINT [CK_Ingredients] CHECK (Stock >= 0 AND Minimum >= 0 AND AverageCost >= 0)
);
IF OBJECT_ID(N'dbo.ExtraServices',N'U') IS NULL CREATE TABLE dbo.[ExtraServices] (
 [Id] nvarchar(64) NOT NULL,
 [Name] nvarchar(256) NOT NULL,
 [Price] decimal(28,8) NOT NULL,
 [Active] bit NOT NULL,
 CONSTRAINT [PK_ExtraServices] PRIMARY KEY ([Id]),
 CONSTRAINT [CK_ExtraServices] CHECK (Price >= 0)
);
IF OBJECT_ID(N'dbo.DiningTables',N'U') IS NULL CREATE TABLE dbo.[DiningTables] (
 [Id] nvarchar(64) NOT NULL,
 [Name] nvarchar(128) NOT NULL,
 [Seats] int NOT NULL,
 CONSTRAINT [PK_DiningTables] PRIMARY KEY ([Id]),
 CONSTRAINT [CK_DiningTables] CHECK (Seats > 0)
);
IF OBJECT_ID(N'dbo.RecipeIngredients',N'U') IS NULL CREATE TABLE dbo.[RecipeIngredients] (
 [FoodId] nvarchar(64) NOT NULL,
 [IngredientId] nvarchar(64) NOT NULL,
 [Quantity] decimal(28,8) NOT NULL,
 CONSTRAINT [PK_RecipeIngredients] PRIMARY KEY ([FoodId],[IngredientId]),
 CONSTRAINT [FK_RecipeIngredients_FoodId] FOREIGN KEY ([FoodId]) REFERENCES dbo.[MenuItems]([Id]),
 CONSTRAINT [FK_RecipeIngredients_IngredientId] FOREIGN KEY ([IngredientId]) REFERENCES dbo.[Ingredients]([Id]),
 CONSTRAINT [CK_RecipeIngredients] CHECK (Quantity > 0)
);
IF OBJECT_ID(N'dbo.TableReservations',N'U') IS NULL CREATE TABLE dbo.[TableReservations] (
 [Id] nvarchar(64) NOT NULL,
 [TableId] nvarchar(64) NOT NULL,
 [Customer] nvarchar(256) NOT NULL,
 [Phone] nvarchar(64) NOT NULL,
 [Time] datetime2(7) NOT NULL,
 [Guests] int NOT NULL,
 [Status] nvarchar(40) NOT NULL,
 CONSTRAINT [PK_TableReservations] PRIMARY KEY ([Id]),
 CONSTRAINT [FK_TableReservations_TableId] FOREIGN KEY ([TableId]) REFERENCES dbo.[DiningTables]([Id]),
 CONSTRAINT [CK_TableReservations] CHECK (Guests > 0)
);
IF OBJECT_ID(N'dbo.StockReceipts',N'U') IS NULL CREATE TABLE dbo.[StockReceipts] (
 [Id] nvarchar(64) NOT NULL,
 [Date] datetime2(7) NOT NULL,
 [Supplier] nvarchar(256) NOT NULL,
 [Operator] nvarchar(256) NOT NULL,
 CONSTRAINT [PK_StockReceipts] PRIMARY KEY ([Id])
);
IF OBJECT_ID(N'dbo.SalesOrders',N'U') IS NULL CREATE TABLE dbo.[SalesOrders] (
 [Id] nvarchar(64) NOT NULL,
 [TableId] nvarchar(64) NULL,
 [TableName] nvarchar(128) NOT NULL,
 [Customer] nvarchar(256) NOT NULL,
 [Operator] nvarchar(256) NOT NULL,
 [CreatedAt] datetime2(7) NOT NULL,
 [PaidAt] datetime2(7) NULL,
 [Status] nvarchar(40) NOT NULL,
 [Discount] decimal(28,8) NOT NULL,
 [PaymentMethod] nvarchar(40) NOT NULL,
 [Received] decimal(28,8) NOT NULL,
 CONSTRAINT [PK_SalesOrders] PRIMARY KEY ([Id]),
 CONSTRAINT [FK_SalesOrders_TableId] FOREIGN KEY ([TableId]) REFERENCES dbo.[DiningTables]([Id]),
 CONSTRAINT [CK_SalesOrders] CHECK (Discount >= 0 AND Received >= 0)
);
IF OBJECT_ID(N'dbo.StockReceiptItems',N'U') IS NULL CREATE TABLE dbo.[StockReceiptItems] (
 [PurchaseId] nvarchar(64) NOT NULL,
 [LineNo] int NOT NULL,
 [IngredientId] nvarchar(64) NOT NULL,
 [Name] nvarchar(256) NOT NULL,
 [Unit] nvarchar(32) NOT NULL,
 [Quantity] decimal(28,8) NOT NULL,
 [UnitCost] decimal(28,8) NOT NULL,
 CONSTRAINT [PK_StockReceiptItems] PRIMARY KEY ([PurchaseId],[LineNo]),
 CONSTRAINT [FK_StockReceiptItems_PurchaseId] FOREIGN KEY ([PurchaseId]) REFERENCES dbo.[StockReceipts]([Id]),
 CONSTRAINT [FK_StockReceiptItems_IngredientId] FOREIGN KEY ([IngredientId]) REFERENCES dbo.[Ingredients]([Id]),
 CONSTRAINT [CK_StockReceiptItems] CHECK (Quantity > 0 AND UnitCost > 0)
);
IF OBJECT_ID(N'dbo.SalesOrderItems',N'U') IS NULL CREATE TABLE dbo.[SalesOrderItems] (
 [OrderId] nvarchar(64) NOT NULL,
 [LineNo] int NOT NULL,
 [FoodId] nvarchar(64) NOT NULL,
 [Name] nvarchar(256) NOT NULL,
 [Quantity] int NOT NULL,
 [UnitPrice] decimal(28,8) NOT NULL,
 [Note] nvarchar(max) NOT NULL,
 CONSTRAINT [PK_SalesOrderItems] PRIMARY KEY ([OrderId],[LineNo]),
 CONSTRAINT [FK_SalesOrderItems_OrderId] FOREIGN KEY ([OrderId]) REFERENCES dbo.[SalesOrders]([Id]),
 CONSTRAINT [FK_SalesOrderItems_FoodId] FOREIGN KEY ([FoodId]) REFERENCES dbo.[MenuItems]([Id]),
 CONSTRAINT [CK_SalesOrderItems] CHECK (Quantity BETWEEN 1 AND 10000 AND UnitPrice >= 0)
);
IF OBJECT_ID(N'dbo.SalesOrderServices',N'U') IS NULL CREATE TABLE dbo.[SalesOrderServices] (
 [OrderId] nvarchar(64) NOT NULL,
 [LineNo] int NOT NULL,
 [ServiceId] nvarchar(64) NOT NULL,
 [Name] nvarchar(256) NOT NULL,
 [Quantity] int NOT NULL,
 [UnitPrice] decimal(28,8) NOT NULL,
 CONSTRAINT [PK_SalesOrderServices] PRIMARY KEY ([OrderId],[LineNo]),
 CONSTRAINT [FK_SalesOrderServices_OrderId] FOREIGN KEY ([OrderId]) REFERENCES dbo.[SalesOrders]([Id]),
 CONSTRAINT [FK_SalesOrderServices_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES dbo.[ExtraServices]([Id]),
 CONSTRAINT [CK_SalesOrderServices] CHECK (Quantity BETWEEN 1 AND 10000 AND UnitPrice >= 0)
);
IF OBJECT_ID(N'dbo.Customers',N'U') IS NULL CREATE TABLE dbo.[Customers] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY, [Name] nvarchar(256) NOT NULL, [Phone] nvarchar(64) NOT NULL, [Email] nvarchar(256) NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.MenuCategories',N'U') IS NULL CREATE TABLE dbo.[MenuCategories] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_MenuCategories PRIMARY KEY, [Name] nvarchar(256) NOT NULL, [Description] nvarchar(max) NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.MenuItemOptions',N'U') IS NULL CREATE TABLE dbo.[MenuItemOptions] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_MenuItemOptions PRIMARY KEY, [Name] nvarchar(256) NOT NULL, [AppliesTo] nvarchar(256) NOT NULL, [Surcharge] decimal(28,8) NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.MealCombos',N'U') IS NULL CREATE TABLE dbo.[MealCombos] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_MealCombos PRIMARY KEY, [Name] nvarchar(256) NOT NULL, [Price] decimal(28,8) NOT NULL, [ItemCount] int NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.DiscountVouchers',N'U') IS NULL CREATE TABLE dbo.[DiscountVouchers] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_DiscountVouchers PRIMARY KEY, [Name] nvarchar(256) NOT NULL, [DiscountType] nvarchar(40) NOT NULL, [Value] decimal(28,8) NOT NULL, [ExpiresOn] nvarchar(32) NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.PromotionalBanners',N'U') IS NULL CREATE TABLE dbo.[PromotionalBanners] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_PromotionalBanners PRIMARY KEY, [Title] nvarchar(256) NOT NULL, [ImagePath] nvarchar(2048) NOT NULL, [SortOrder] int NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.BusinessHours',N'U') IS NULL CREATE TABLE dbo.[BusinessHours] ([Day] nvarchar(128) NOT NULL CONSTRAINT PK_BusinessHours PRIMARY KEY, [IsOpen] nvarchar(16) NOT NULL, [OpenTime] nvarchar(16) NOT NULL, [CloseTime] nvarchar(16) NOT NULL);
IF OBJECT_ID(N'dbo.LegacyMenuItems',N'U') IS NULL CREATE TABLE dbo.[LegacyMenuItems] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_LegacyMenuItems PRIMARY KEY, [Name] nvarchar(256) NOT NULL, [Category] nvarchar(256) NOT NULL, [Price] decimal(28,8) NOT NULL, [Status] nvarchar(40) NOT NULL);
IF OBJECT_ID(N'dbo.LegacySalesOrders',N'U') IS NULL CREATE TABLE dbo.[LegacySalesOrders] ([Code] nvarchar(128) NOT NULL CONSTRAINT PK_LegacySalesOrders PRIMARY KEY, [Customer] nvarchar(256) NOT NULL, [OrderedOn] nvarchar(32) NOT NULL, [Total] decimal(28,8) NOT NULL, [Status] nvarchar(40) NOT NULL);
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.SalesOrders') AND name=N'IX_SalesOrders_PaidAt') CREATE INDEX IX_SalesOrders_PaidAt ON dbo.SalesOrders(PaidAt,Status);
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.TableReservations') AND name=N'IX_TableReservations_TableTime') CREATE INDEX IX_TableReservations_TableTime ON dbo.TableReservations(TableId,Time);
COMMIT;
GO
CREATE OR ALTER VIEW dbo.vOrderTotals AS
SELECT o.*, ISNULL(i.Amount,0)+ISNULL(e.Amount,0) AS Subtotal,
 ISNULL(i.Amount,0)+ISNULL(e.Amount,0)-o.Discount AS Total
FROM dbo.SalesOrders o
OUTER APPLY(SELECT SUM(Quantity*UnitPrice) Amount FROM dbo.SalesOrderItems WHERE OrderId=o.Id) i
OUTER APPLY(SELECT SUM(Quantity*UnitPrice) Amount FROM dbo.SalesOrderServices WHERE OrderId=o.Id) e;
GO
CREATE OR ALTER VIEW dbo.vRevenueMonthly AS
SELECT YEAR(PaidAt) AS [Year],MONTH(PaidAt) AS [Month],COUNT_BIG(*) AS Receipts,SUM(Subtotal) AS Gross,SUM(Discount) AS Discount,SUM(Total) AS Net
FROM dbo.vOrderTotals WHERE Status=N'Đã thanh toán' GROUP BY YEAR(PaidAt),MONTH(PaidAt);
GO
CREATE OR ALTER VIEW dbo.vRevenueQuarterly AS
SELECT YEAR(PaidAt) AS [Year],DATEPART(quarter,PaidAt) AS [Quarter],COUNT_BIG(*) AS Receipts,SUM(Subtotal) AS Gross,SUM(Discount) AS Discount,SUM(Total) AS Net
FROM dbo.vOrderTotals WHERE Status=N'Đã thanh toán' GROUP BY YEAR(PaidAt),DATEPART(quarter,PaidAt);
GO
CREATE OR ALTER VIEW dbo.vBestSellingFoods AS
SELECT i.FoodId,i.Name,SUM(CONVERT(bigint,i.Quantity)) AS Quantity,SUM(i.Quantity*i.UnitPrice) AS Revenue
FROM dbo.SalesOrderItems i JOIN dbo.SalesOrders o ON o.Id=i.OrderId WHERE o.Status=N'Đã thanh toán' GROUP BY i.FoodId,i.Name;
GO
CREATE OR ALTER VIEW dbo.vTableStatus AS
SELECT t.*,CASE WHEN EXISTS(SELECT 1 FROM dbo.SalesOrders o WHERE o.TableId=t.Id AND o.Status=N'Đang phục vụ') THEN N'Đang phục vụ'
 WHEN EXISTS(SELECT 1 FROM dbo.TableReservations r WHERE r.TableId=t.Id AND r.Status=N'Đã đặt' AND r.Time BETWEEN DATEADD(hour,-2,GETDATE()) AND DATEADD(hour,2,GETDATE())) THEN N'Đã đặt' ELSE N'Trống' END AS Status
FROM dbo.DiningTables t;
GO
