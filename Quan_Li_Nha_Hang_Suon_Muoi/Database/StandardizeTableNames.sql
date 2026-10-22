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
