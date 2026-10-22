# Quản lý nhà hàng Sườn Mười

Ứng dụng WinForms quản lý nhà hàng, được tích hợp trong solution `Quan_Li_Nha_Hang_Suon_Muoi.slnx` và project cùng tên. `Form1` là màn hình chính; ứng dụng bắt đầu bằng form đăng nhập.

## Chạy dự án

1. Cài Visual Studio 2022 với workload .NET desktop development và .NET 9 SDK.
2. Mở `Quan_Li_Nha_Hang_Suon_Muoi.slnx`, chờ NuGet restore rồi nhấn F5.
3. Trên máy hiện tại, ứng dụng dùng SQL Server `.` và database `LuaMocDB`, kết nối Windows Authentication.
4. Trên máy khác, chạy `Quan_Li_Nha_Hang_Suon_Muoi/Database/CreateDatabase.sql` trong SSMS rồi chỉnh `Server=` trong `App.config`. Lần mở đầu, DB trống được khởi tạo bằng dữ liệu mẫu hoặc các tệp nhập đã có.

Tài khoản khởi tạo cho DB mới: `admin / admin123`, `staff / staff123`. Tài khoản hiện có trong DB tiếp tục được sử dụng.

## Chức năng

Tổng quan; món ăn, ảnh và công thức; nguyên liệu, tồn kho và phiếu nhập; bàn, đặt bàn; đơn hàng, dịch vụ, thanh toán và trừ kho; khách hàng, danh mục, tùy chọn, combo, voucher, banner và giờ mở cửa. Báo cáo doanh thu tháng/quý và món bán chạy chỉ tính đơn đã thanh toán.

Khách hàng, Món ăn và Đơn hàng có xuất Excel (.xlsx). Đơn hàng có in chi tiết đơn đang chọn. Hóa đơn, phiếu nhập và báo cáo sử dụng tên SƯỜN MƯỜI.

## Code và dữ liệu

Tên project, assembly và namespace: `Quan_Li_Nha_Hang_Suon_Muoi`. Các form tiếp tục có file `.Designer.cs` và `.resx` để chỉnh bằng View Designer. `.editorconfig` quy định thụt lề 4 khoảng trắng và tách câu lệnh thành các dòng rõ ràng.

DB hiện tại giữ tên `LuaMocDB` để tiếp tục sử dụng dữ liệu đã có. Ảnh và dữ liệu nhập cũ vẫn dùng `%LOCALAPPDATA%/LuaMoc/Data`; có thể ghi đè bằng `SUONMUOI_DATA_DIR`. Chuỗi kết nối có thể ghi đè bằng `SUONMUOI_CONNECTION_STRING`.

Repo chỉ chứa mã nguồn và script tạo DB; không chứa tài khoản Windows, dữ liệu DB thực tế, bản sao lưu, hoặc tệp build.

Hướng dẫn SQL Server và bảng dữ liệu: [Database/README_SQL_SERVER.md](Quan_Li_Nha_Hang_Suon_Muoi/Database/README_SQL_SERVER.md). Hướng dẫn nghiệp vụ: [HUONG_DAN_NGHIEP_VU.md](Quan_Li_Nha_Hang_Suon_Muoi/HUONG_DAN_NGHIEP_VU.md).
