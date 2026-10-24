# Hướng dẫn đọc mã nguồn để bảo vệ

## 1. Đọc theo thứ tự
1. Program.cs: khởi tạo bảng còn thiếu rồi mở frmDangNhap.
2. DAL/ProcessDatabase.cs: OpenConnection, DataReader, DataChange.
3. Models/MonAn.cs và KhachHang.cs: các thuộc tính của một bản ghi.
4. DAL/MonAnDAL.cs: DanhSach tìm kiếm bằng tham số; Luu chọn INSERT hoặc UPDATE.
5. Forms/frmMonAn.cs: bấm nút -> gọi DAL -> tải lại DataGridView.
6. Forms/frmSuaMon.cs và Services/XuLyAnh.cs: chọn ảnh, xem trước, sao chép file, lưu đường dẫn.
7. Models/HoaDon.cs: một hóa đơn có nhiều ChiTietHoaDon; công thức tính tiền.
8. DAL/HoaDonDAL.cs: đọc chi tiết, lưu, thanh toán, hủy.
9. Forms/frmLapHoaDon.cs: danh sách món trong BindingList và cập nhật tổng tiền.
10. DAL/BaoCaoDAL.cs: WHERE, GROUP BY, SUM, COUNT; Forms/frmBaoCao.cs hiển thị kết quả.

Mỗi form có file .Designer.cs chứa control và kích thước. File .cs chứa sự kiện, không trộn SQL vào Designer.

## 2. Luồng dữ liệu đơn giản
Người dùng bấm nút -> Form đọc ô nhập -> tạo Model -> gọi DAL -> SQL Server.
DAL trả DataTable hoặc Model -> Form gán DataSource / hiển thị kết quả.

ProcessDatabase.DataReader dùng SqlDataAdapter.Fill để lấy dữ liệu vào DataTable.
ProcessDatabase.DataChange dùng ExecuteNonQuery cho lệnh thay đổi dữ liệu.
using tự đóng kết nối cả khi có lỗi.
SqlParameter truyền dữ liệu nhập riêng với câu SQL, tránh ghép chuỗi người dùng vào truy vấn.

## 3. Cơ sở dữ liệu chính
- Users: tài khoản đăng nhập. Giữ nguyên hash PBKDF2, salt, số vòng lặp của tài khoản cũ.
- Foods: món ăn; ImageUrl lưu đường dẫn, không lưu toàn bộ ảnh vào SQL.
- Customers: khách hàng.
- HoaDon: ngày lập, khách, người lập, tổng tiền, giảm giá, phương thức, trạng thái.
- ChiTietHoaDon: món, tên món tại lúc bán, số lượng, đơn giá tại lúc bán.
HoaDon.MaHoaDon là khóa chính; ChiTietHoaDon.MaHoaDon là khóa ngoại.
Một hóa đơn có nhiều dòng món. Một món có thể xuất hiện trong nhiều hóa đơn.
MaMon cho phép NULL ở dòng dịch vụ lịch sử khi chuyển từ ứng dụng cũ; hóa đơn mới chỉ bán món.

## 4. Logic hóa đơn phải giải thích được
- Thành tiền từng dòng = số lượng x đơn giá.
- Tổng tiền = tổng thành tiền các dòng.
- Phải thu = tổng tiền - tiền giảm.
- Tiền thừa = tiền khách đưa - phải thu, chỉ áp dụng tiền mặt.
- Giảm giá không âm và không lớn hơn tổng tiền.
- Khi thanh toán tiền mặt, tiền khách đưa phải đủ.
- Chuyển khoản/thẻ là ghi nhận của nhân viên, chưa tích hợp cổng thanh toán.
- Chỉ hóa đơn Chưa thanh toán được sửa hoặc hủy.
- Hóa đơn Đã thanh toán không thể thanh toán lần thứ hai.
- Giá và tên món được chụp vào chi tiết. Đổi giá thực đơn không làm thay đổi hóa đơn đã lưu.
- DAL lấy giá từ DB, không tin đơn giá tự truyền từ giao diện.
- Một transaction lưu cả đầu hóa đơn và toàn bộ chi tiết: lỗi một phần thì Rollback.
- Cột PhienBan kiểu rowversion phát hiện hai cửa sổ cùng sửa. Bản cũ phải mở lại trước khi lưu.

## 5. Logic ảnh món ăn
OpenFileDialog chọn ảnh -> PictureBox xem trước -> kiểm tra file/định dạng/dung lượng ->
sao chép sang thư mục Images với tên mới -> Foods.ImageUrl lưu đường dẫn tương đối.
Lưu một bản sao giúp ảnh không mất khi di chuyển hoặc xóa file gốc.
Image được clone rồi đóng stream để không giữ khóa file.
Không cần upload lên mạng hoặc lưu ảnh vào database dạng BLOB.

## 6. Báo cáo thống kê
WHERE TrangThai = 'Đã thanh toán'.
Lọc theo NgayThanhToan, không lấy NgayLap để tính ngày thu tiền.
Khoảng ngày: >= từ ngày 00:00 và < ngày sau ngày kết thúc.
GROUP BY ngày/tháng; COUNT số hóa đơn; SUM(ThanhTien) ra doanh thu thuần.
Món bán chạy JOIN HoaDon với ChiTietHoaDon, SUM số lượng, sắp xếp giảm dần.
Tiền món bán chạy là tiền trước giảm; không gọi đó là lợi nhuận.
Bản tinh gọn không còn quản lý giá vốn/tồn kho nên không báo cáo lợi nhuận.

## 7. Câu hỏi cô có thể hỏi
1. Vì sao tách HoaDon và ChiTietHoaDon? Một hóa đơn có nhiều món, tránh lặp thông tin khách/ngày.
2. Vì sao không xóa món đã bán? Cần giữ lịch sử, dùng cờ ngừng bán.
3. Vì sao dùng transaction? Đầu hóa đơn và chi tiết phải cùng thành công hoặc cùng thất bại.
4. Nếu hai người cùng sửa? So sánh rowversion trong transaction.
5. Vì sao dùng decimal cho tiền? Tránh sai số nhị phân của float/double.
6. Vì sao ảnh không hiện khi copy project? Ảnh nằm trong thư mục dữ liệu; phải copy cả thư mục Images.
7. Vì sao doanh thu bằng 0? Kiểm tra trạng thái đã thanh toán, ngày thanh toán và bộ lọc.
8. Vì sao nhân viên không sửa món? DAL kiểm tra quyền Admin, giao diện cũng khóa nút.
9. Vì sao đã lưu món nhưng lưới chưa đổi? Sau lưu cần tải lại DataSource.
10. Giao diện có tự kết nối ngân hàng không? Không; chỉ ghi nhận phương thức thu tiền.

## 8. Kịch bản trình bày nhanh
- Thêm một món, chọn ảnh, lưu và tìm lại.
- Thêm khách hàng.
- Lập hóa đơn hai món; chỉ ra số lượng, giảm giá và tiền thừa.
- Thử thanh toán khi tiền chưa đủ để minh họa kiểm tra dữ liệu.
- Thanh toán đủ, mở lại hóa đơn để thấy không thể sửa.
- Mở báo cáo, chọn khoảng ngày và xem doanh thu/món bán chạy.
- Mở HoaDonDAL.Luu và chỉ phần BeginTransaction, Commit, Rollback.

## 9. Logic xuất Excel
Forms/frmBaoCao.cs giữ DataTable và kỳ báo cáo đã tải thành công.
Nút Xuất Excel lấy tab đang chọn, mở SaveFileDialog rồi gọi Services/XuatExcel.Luu.
XuatExcel tạo file .xlsx bằng System.IO.Compression và XML, không cần cài Microsoft Excel.
Các phần chính: workbook (tên sheet), styles (màu/font/định dạng), sheetData (các ô).
Hàm O ghi từng ô: văn bản dùng inlineStr, số ghi giá trị số, ngày đổi sang số ngày Excel.
Tổng cộng dùng SUM cho các cột số, có sẵn kết quả để xem ngay.
Bộ lọc chỉ chứa tiêu đề và dữ liệu, không gồm dòng tổng cộng; SUM luôn là tổng cả kỳ.
File được tạo ở đường dẫn tạm rồi mới đổi sang file đích để tránh ghi dở khi có lỗi.
