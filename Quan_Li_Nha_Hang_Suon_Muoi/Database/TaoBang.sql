-- Chạy trong LuaMocDB. Chỉ thêm bảng còn thiếu, giữ nguyên dữ liệu cũ.
SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRANSACTION;
IF OBJECT_ID(N'dbo.Users',N'U') IS NULL
CREATE TABLE dbo.Users(
 Username nvarchar(128) PRIMARY KEY, FullName nvarchar(256) NOT NULL,
 Role nvarchar(16) NOT NULL, PasswordHash varbinary(32) NOT NULL,
 Salt varbinary(16) NOT NULL, Iterations int NOT NULL, IsActive bit NOT NULL);
IF OBJECT_ID(N'dbo.Foods',N'U') IS NULL
CREATE TABLE dbo.Foods(
 FoodId nvarchar(64) PRIMARY KEY,Name nvarchar(256) NOT NULL,Category nvarchar(128) NOT NULL,
 Price decimal(28,8) NOT NULL CHECK(Price>0),IsAvailable bit NOT NULL,
 ImageUrl nvarchar(2048) NOT NULL DEFAULT N'',Instructions nvarchar(max) NOT NULL DEFAULT N'');
IF OBJECT_ID(N'dbo.Customers',N'U') IS NULL
CREATE TABLE dbo.Customers(
 Code nvarchar(128) PRIMARY KEY,Name nvarchar(256) NOT NULL,Phone nvarchar(64) NOT NULL DEFAULT N'',
 Email nvarchar(256) NOT NULL DEFAULT N'',Status nvarchar(40) NOT NULL DEFAULT N'Hoạt động');

IF OBJECT_ID(N'dbo.HoaDon',N'U') IS NULL
CREATE TABLE dbo.HoaDon(
 MaHoaDon int IDENTITY PRIMARY KEY,
 MaDonCu nvarchar(64) NULL,
 NgayLap datetime2(7) NOT NULL DEFAULT SYSDATETIME(),
 MaKhachHang nvarchar(128) NULL REFERENCES dbo.Customers(Code),
 TenKhachHang nvarchar(256) NOT NULL,
 NguoiLap nvarchar(256) NOT NULL,
 TongTien decimal(28,8) NOT NULL CHECK(TongTien>=0),
 TienGiam decimal(28,8) NOT NULL,
 ThanhTien AS (TongTien-TienGiam) PERSISTED,
 TienKhachDua decimal(28,8) NOT NULL CHECK(TienKhachDua>=0),
 PhuongThuc nvarchar(20) NOT NULL CHECK(PhuongThuc IN(N'Tiền mặt',N'Chuyển khoản',N'Thẻ')),
 TrangThai nvarchar(30) NOT NULL CHECK(TrangThai IN(N'Chưa thanh toán',N'Đã thanh toán',N'Đã hủy')),
 NgayThanhToan datetime2(7) NULL,
 GhiChu nvarchar(500) NOT NULL DEFAULT N'',
 PhienBan rowversion NOT NULL,
 CONSTRAINT CK_HoaDon_Giam CHECK(TienGiam>=0 AND TienGiam<=TongTien),
 CONSTRAINT CK_HoaDon_NgayThanhToan CHECK(
  (TrangThai=N'Đã thanh toán' AND NgayThanhToan IS NOT NULL)
  OR (TrangThai<>N'Đã thanh toán' AND NgayThanhToan IS NULL)));
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.HoaDon') AND name=N'UX_HoaDon_MaDonCu')
 CREATE UNIQUE INDEX UX_HoaDon_MaDonCu ON dbo.HoaDon(MaDonCu) WHERE MaDonCu IS NOT NULL;
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.HoaDon') AND name=N'IX_HoaDon_ThanhToan')
 CREATE INDEX IX_HoaDon_ThanhToan ON dbo.HoaDon(TrangThai,NgayThanhToan);

IF OBJECT_ID(N'dbo.ChiTietHoaDon',N'U') IS NULL
CREATE TABLE dbo.ChiTietHoaDon(
 MaChiTiet int IDENTITY PRIMARY KEY,
 MaHoaDon int NOT NULL REFERENCES dbo.HoaDon(MaHoaDon),
 MaMon nvarchar(64) NULL REFERENCES dbo.Foods(FoodId),
 TenMon nvarchar(256) NOT NULL,
 SoLuong int NOT NULL CHECK(SoLuong BETWEEN 1 AND 10000),
 DonGia decimal(28,8) NOT NULL CHECK(DonGia>=0),
 ThanhTien AS (SoLuong*DonGia) PERSISTED);

-- Sao chép đơn cũ một lần, không xóa Orders/OrderItems hay đổi số hóa đơn cũ.
IF OBJECT_ID(N'dbo.Orders',N'U') IS NOT NULL AND OBJECT_ID(N'dbo.OrderItems',N'U') IS NOT NULL
EXEC(N'
 INSERT dbo.HoaDon(MaDonCu,NgayLap,TenKhachHang,NguoiLap,TongTien,TienGiam,TienKhachDua,PhuongThuc,TrangThai,NgayThanhToan,GhiChu)
 SELECT o.OrderId,o.CreatedAt,o.ReceiverName,o.Operator,o.Subtotal,o.Discount,o.Received,
  CASE WHEN o.PaymentMethod IN(N''Tiền mặt'',N''Chuyển khoản'',N''Thẻ'') THEN o.PaymentMethod ELSE N''Tiền mặt'' END,
  CASE WHEN o.Status=N''Đã thanh toán'' THEN N''Đã thanh toán'' WHEN o.Status=N''Đã hủy'' THEN N''Đã hủy'' ELSE N''Chưa thanh toán'' END,
  CASE WHEN o.Status=N''Đã thanh toán'' THEN o.PaidAt ELSE NULL END,
  N''Chuyển từ đơn cũ; bàn: ''+o.TableName
 FROM dbo.Orders o WHERE NOT EXISTS(SELECT 1 FROM dbo.HoaDon h WHERE h.MaDonCu=o.OrderId);
 INSERT dbo.ChiTietHoaDon(MaHoaDon,MaMon,TenMon,SoLuong,DonGia)
 SELECT h.MaHoaDon,i.FoodId,i.ItemName,i.Quantity,i.UnitPrice
 FROM dbo.OrderItems i JOIN dbo.HoaDon h ON h.MaDonCu=i.OrderId
 WHERE NOT EXISTS(SELECT 1 FROM dbo.ChiTietHoaDon c WHERE c.MaHoaDon=h.MaHoaDon);
');
-- Dịch vụ trên hóa đơn lịch sử vẫn được giữ như một dòng chi tiết riêng.
IF OBJECT_ID(N'dbo.SalesOrderServices',N'U') IS NOT NULL
EXEC(N'
 INSERT dbo.ChiTietHoaDon(MaHoaDon,MaMon,TenMon,SoLuong,DonGia)
 SELECT h.MaHoaDon,NULL,N''Dịch vụ cũ: ''+s.Name,s.Quantity,s.UnitPrice
 FROM dbo.SalesOrderServices s JOIN dbo.HoaDon h ON h.MaDonCu=s.OrderId
 WHERE NOT EXISTS(SELECT 1 FROM dbo.ChiTietHoaDon c
  WHERE c.MaHoaDon=h.MaHoaDon AND c.MaMon IS NULL AND c.TenMon=N''Dịch vụ cũ: ''+s.Name AND c.DonGia=s.UnitPrice);
');
COMMIT;
END TRY
BEGIN CATCH
 IF XACT_STATE()<>0 ROLLBACK;
 THROW;
END CATCH;
