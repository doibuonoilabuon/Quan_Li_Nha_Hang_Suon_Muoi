USE [LuaMocDB];
GO
SELECT DB_NAME() AS DatabaseName, @@SERVERNAME AS ServerName;
SELECT 'MenuItems' AS TableName, COUNT(*) AS [Rows] FROM dbo.MenuItems
UNION ALL SELECT 'Ingredients',COUNT(*) FROM dbo.Ingredients
UNION ALL SELECT 'RecipeIngredients',COUNT(*) FROM dbo.RecipeIngredients
UNION ALL SELECT 'Customers',COUNT(*) FROM dbo.Customers
UNION ALL SELECT 'SalesOrders',COUNT(*) FROM dbo.SalesOrders
UNION ALL SELECT 'SalesOrderItems',COUNT(*) FROM dbo.SalesOrderItems;
SELECT TOP (100) * FROM dbo.MenuItems;
SELECT TOP (100) * FROM dbo.Customers;
SELECT TOP (100) * FROM dbo.vOrderTotals ORDER BY CreatedAt DESC;
SELECT TOP (100) * FROM dbo.SalesOrderItems;
SELECT * FROM dbo.vRevenueMonthly;
SELECT * FROM dbo.vRevenueQuarterly;
SELECT * FROM dbo.vBestSellingFoods;
SELECT Username,DisplayName,Role,Active FROM dbo.UserAccounts;
