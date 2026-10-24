# SƯỜN MƯỜI - Bản tinh gọn

Chỉ có 5 phần: Đăng nhập, Món ăn và ảnh, Khách hàng, Hóa đơn, Báo cáo thống kê.

## Chạy
Mở solution ở thư mục ngoài, chọn project cùng tên, F5.
Kết nối đặt tại DAL/ProcessDatabase.cs: WHISPERSTONE\SQLEXPRESS01, LuaMocDB, Windows Authentication.
Đã giữ Microsoft.Data.SqlClient phù hợp với .NET 9 của dự án.
Nếu DB chưa tồn tại, tạo LuaMocDB trong SSMS trước khi chạy.
Ứng dụng chỉ tạo bảng còn thiếu. Tài khoản cũ được giữ.
Với DB mới hoàn toàn: admin / admin123.

## Cấu trúc
- Forms: mỗi màn hình có .cs xử lý sự kiện và .Designer.cs bố cục.
- Models: MonAn, KhachHang, HoaDon, ChiTietHoaDon, PhienDangNhap.
- DAL: câu lệnh SELECT / INSERT / UPDATE và giao dịch hóa đơn.
- Services: lưu ảnh, in hóa đơn, xuất CSV.
- Database/TaoBang.sql: các bảng chính và chuyển đơn cũ sang HoaDon một lần.

## Thao tác
1. Món ăn -> Thêm / Sửa -> Chọn ảnh -> Lưu món. JPG/PNG/BMP, tối đa 10 MB.
2. Khách hàng -> Thêm / Sửa. Ngừng hoạt động để giữ lịch sử.
3. Hóa đơn -> Thêm -> chọn khách, thêm món, số lượng, giảm giá.
4. Lưu chưa thanh toán hoặc Thanh toán. Tiền mặt phải đủ; chuyển khoản/thẻ ghi nhận đã thu đủ.
5. Hóa đơn đã thanh toán không sửa/hủy. Chọn In hóa đơn để xem trước.
6. Báo cáo thống kê lọc theo ngày/tháng, chỉ tính hóa đơn đã thanh toán. Có món bán chạy, CSV, in báo cáo.

Thanh toán không còn dùng kho, công thức, dịch vụ hay đặt bàn.
Món ngừng bán không xóa vật lý, để hóa đơn cũ còn dữ liệu.
Ảnh lưu trong %LOCALAPPDATA%/LuaMoc/Data/Images; DB chỉ lưu đường dẫn.
Các bảng nghiệp vụ cũ được giữ nguyên nhưng bản này không sử dụng.
Xem DOC_MA_NGUON.md để ôn code và giải thích trước cô.
