# SƯỜN MƯỜI - WinForms Designer (.NET 9)

Mở `Quan_Li_Nha_Hang_Suon_Muoi.slnx` bằng Visual Studio 2022 17.12+ và cài .NET 9 SDK, workload .NET desktop development.

**Các màn hình có .Designer.cs**: đăng nhập, Main, Dashboard, Food, Category, FoodOption, Combo, Order, Voucher, Customer, Banner, OpeningHour, frmCrudModule, frmRecordEdit, Sidebar, Header, StatCard.

Nhấp chuột phải file `.cs` → View Designer (Shift+F7). Với các form module kế thừa `frmCrudModule`, bố cục bảng và thanh công cụ nằm ở **Forms/Modules/frmCrudModule.Designer.cs**. Muốn kéo thả các control dùng chung, mở **frmCrudModule.cs → View Designer**. Form con kế thừa hiển thị các control này nhưng có thể khóa chỉnh sửa control kế thừa theo cơ chế WinForms.

**Quan trọng**: Demo CRUD dùng DataTable trong bộ nhớ; không có SQL Server. `frmRecordEdit` tạo ô nhập theo số cột khi chạy; Designer cho chỉnh phần khung hộp thoại, không kéo thả từng ô động. Các form riêng như `frmFoodEdit`/`frmComboEdit` chưa có bản Designer nghiệp vụ chuyên biệt.

Tài khoản demo: admin/admin123 và staff/staff123. Không sử dụng tài khoản mẫu này khi triển khai thật.

Đây là chuyển đổi mã nguồn; chưa được biên dịch trên Windows trong môi trường tạo file. Hãy Build → Rebuild Solution và gửi Error List nếu có lỗi.
