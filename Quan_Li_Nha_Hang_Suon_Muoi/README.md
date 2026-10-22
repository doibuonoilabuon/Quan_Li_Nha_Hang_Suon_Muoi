# SƯỜN MƯỜI — Quản lý nhà hàng

Ứng dụng C# WinForms / .NET 9, chỉnh giao diện bằng Visual Studio 2022 View Designer, lưu dữ liệu và xác thực tài khoản bằng SQL Server.

## Chạy trên máy hiện tại

1. SQL Server default instance đang dùng: `.` / `LAPTOP-V6HOH5QF`; database: `LuaMocDB`.
2. Mở solution `Quan_Li_Nha_Hang_Suon_Muoi.slnx`, chờ NuGet restore `Microsoft.Data.SqlClient`, nhấn F5.
3. Dùng tài khoản đã có trong bảng UserAccounts. Tài khoản ban đầu cho DB mới: admin / admin123, staff / staff123. Mật khẩu trong DB được băm PBKDF2 với salt.

Chuỗi kết nối Windows Authentication nằm trong `App.config`. Hướng dẫn cài DB trên máy khác và sao lưu: [Database/README_SQL_SERVER.md](Database/README_SQL_SERVER.md). Script SSMS: [Database/CreateDatabase.sql](Database/CreateDatabase.sql).

## Chức năng

Tổng quan theo mẫu SƯỜN MƯỜI, món/ảnh/công thức, nguyên liệu và tồn kho, phiếu nhập, bàn/đặt bàn, đơn hàng và dịch vụ, thanh toán/trừ kho theo transaction, khách hàng, danh mục, tùy chọn, combo, voucher, banner và giờ mở cửa. Doanh thu tháng/quý và món bán chạy chỉ tính đơn đã thanh toán.

Khách hàng, Món ăn, Đơn hàng có Xuất Excel (.xlsx). Đơn hàng có In đơn hàng cho dòng đang chọn. Các nghiệp vụ bổ sung nằm trong menu Nghiệp vụ khác.

Hướng dẫn chi tiết: [HUONG_DAN_NGHIEP_VU.md](HUONG_DAN_NGHIEP_VU.md).

## Thông tin dự án Sườn Mười

Solution: `Quan_Li_Nha_Hang_Suon_Muoi.slnx`. Project và namespace: `Quan_Li_Nha_Hang_Suon_Muoi`.

Ứng dụng tiếp tục dùng database `LuaMocDB` và thư mục dữ liệu đã có để giữ nguyên đơn hàng, tài khoản và ảnh. Tên nhà hàng hiển thị trên giao diện, hóa đơn, báo cáo và bản sao lưu là **SƯỜN MƯỜI**.

Quy tắc trình bày C# được lưu trong `.editorconfig`: mỗi câu lệnh xuống dòng, khối lệnh có dấu ngoặc và thụt lề 4 khoảng trắng. Trong Visual Studio có thể dùng Ctrl+K, Ctrl+D để định dạng tài liệu.
