# HƯỚNG DẪN ĐỌC MÃ NGUỒN

## 1. Luồng xử lý
Người dùng bấm nút -> Form kiểm tra dữ liệu -> gọi DBProcess -> SQL Server.
DBProcess trả DataTable -> Form hiển thị lên DataGridView hoặc đọc thành Model.
Không còn các lớp MonAnDAL, KhachHangDAL, HoaDonDAL, TaiKhoanDAL hay BaoCaoDAL.
Chỉ Database/DBProcess.cs tạo SqlConnection, SqlCommand và SqlDataAdapter.

## 2. DBProcess
- OpenConnect / CloseConnect là phương thức private: form không gọi trực tiếp.
- DataReader nhận SQL và SqlParameter, dùng SqlDataAdapter.Fill trả DataTable.
- DataChange nhận SQL và SqlParameter, dùng ExecuteNonQuery trả số dòng bị thay đổi.
- Nếu không có giao dịch, mỗi lệnh tự đóng kết nối trong finally.
- BatDauGiaoDich giữ một kết nối dùng chung cho các lệnh của cùng hóa đơn.
- ChapNhanGiaoDich là Commit; HuyGiaoDich là Rollback. Cả hai đều đóng kết nối.
SqlParameter tách dữ liệu nhập khỏi SQL; tên có dấu nháy vẫn hoạt động và không biến thành lệnh SQL.

Ví dụ trong form:
```csharp
private readonly DBProcess db = new();
DataTable bang = db.DataReader(
    "SELECT * FROM dbo.Foods WHERE Name LIKE @tim",
    new SqlParameter("@tim", "%" + txtTim.Text + "%"));
grid.DataSource = bang;
```

## 3. Đọc từng chức năng
1. Program.cs: cấu hình ứng dụng và mở frmDangNhap; sử dụng LuaMocDB có sẵn, không tự tạo bảng hoặc tài khoản.
2. frmDangNhap.KiemTraTaiKhoan: SELECT tài khoản, kiểm tra mật khẩu PBKDF2 và vai trò.
3. frmMonAn.DocDanhSachMon / NgungBanMon: tìm kiếm, ngừng bán qua DBProcess.
4. frmSuaMon.LuuMon: INSERT nếu chưa có mã; UPDATE nếu đã có mã. Ảnh qua XuLyAnh.
5. frmKhachHang.DocDanhSachKhach / NgungHoatDongKhach và frmSuaKhach.LuuKhach.
6. frmHoaDon.DocDanhSachHoaDon / DocHoaDon / HuyHoaDon: danh sách, chi tiết, hủy.
7. frmLapHoaDon.LuuHoaDon: lưu hóa đơn và chi tiết trong một giao dịch.
8. frmBaoCao.DocDoanhThu / DocMonBanChay: SQL có WHERE, GROUP BY, SUM, COUNT.
9. Services/XuatExcel.cs: các thao tác Excel.Application, Workbook, Worksheet, Range.
File .Designer.cs chỉ chứa bố cục và control; không viết SQL hay tải DB trong Designer.

## 4. Logic hóa đơn
Thành tiền dòng = số lượng x đơn giá. Tổng tiền = tổng các dòng. Phải thu = tổng tiền - giảm giá.
Tiền thừa áp dụng tiền mặt. Khi thanh toán, khách phải đưa đủ tiền.
Chuyển khoản/thẻ ghi nhận đã thu đúng phải thu; chưa tích hợp cổng thanh toán.
Món mới lấy giá từ DB. Món đã lưu giữ tên/giá tại lúc bán để không thay đổi lịch sử.
Chỉ hóa đơn chưa thanh toán được sửa/hủy. So sánh PhienBan (rowversion) để phát hiện hai người cùng sửa.
Begin -> kiểm tra -> lưu đầu hóa đơn -> lưu chi tiết -> Commit. Có lỗi thì Rollback cả đầu và chi tiết.
INSERT ... OUTPUT INSERTED.MaHoaDon trả mã mới; dùng DataReader để đọc dòng kết quả này.
SQL đọc chi tiết dùng giao dịch RepeatableRead để đầu và chi tiết cùng một thời điểm.

## 5. Xuất Excel như video của cô
Nút Xuất Excel ở frmBaoCao xuất tab đang chọn. Nút ở frmHoaDon xuất hóa đơn đang chọn.
XuatExcel.Luu / LuuHoaDon tạo một Excel.Application riêng, thêm Workbook và lấy Worksheet đầu tiên.
Range.Merge trộn ô. Font.Name / Size / Bold / Color định dạng chữ. Value2 điền dữ liệu, Formula đặt công thức.
Các vòng lặp chuyển DataTable hoặc ChiTietHoaDon sang bảng object[,] rồi gán vào vùng Excel.
SUM tính tổng tiền; số tiền và ngày giữ kiểu số để Excel tính và sắp xếp được.
PageSetup đặt giấy A4, hướng in, vùng in và hàng tiêu đề lặp lại.
SaveAs lưu .xlsx; Close và Quit đóng phiên xuất. Marshal.ReleaseComObject giải phóng đối tượng COM.
Không đóng phiên Excel người dùng đang mở. Văn bản bắt đầu bằng dấu = vẫn được giữ là văn bản.
Video tham khảo: https://www.youtube.com/watch?v=FFhC6DI_He4

## 6. Câu hỏi cô có thể hỏi
- Vì sao tách HoaDon và ChiTietHoaDon? Một hóa đơn có nhiều món, không lặp thông tin khách/ngày.
- Vì sao dùng DBProcess? Tập trung kết nối và thực thi SQL ở một lớp; các form dùng lại.
- Vì sao không ghép dữ liệu người dùng vào SQL? Để tránh lỗi dấu nháy và SQL injection.
- Vì sao cần transaction? Tránh lưu đầu hóa đơn mà thiếu chi tiết khi có lỗi.
- Vì sao dùng decimal cho tiền? Tránh sai số của float/double.
- Vì sao không xóa món đã bán? Giữ lịch sử; dùng cờ ngừng bán.
- Vì sao ảnh không hiện trên máy khác? Cần chép ảnh và cấu hình đường dẫn phù hợp.
- Vì sao doanh thu bằng 0? Kiểm tra trạng thái đã thanh toán, ngày thanh toán và khoảng lọc.
- Vì sao cần cài Excel? Interop điều khiển Microsoft Excel thực tế trên Windows.
