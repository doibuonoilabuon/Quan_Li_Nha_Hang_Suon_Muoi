# SƯỜN MƯỜI — nghiệp vụ nhà hàng

## Các chức năng đã bổ sung

- Món / đồ uống: thêm, sửa, ngừng bán, ảnh được sao chép vào kho ảnh của ứng dụng.
- Công thức: định lượng nguyên liệu cho một phần, hướng dẫn chế biến / pha chế. Nhân viên xem, quản trị viên sửa.
- Nguyên liệu: danh mục, đơn vị cơ sở, tồn kho, mức tồn tối thiểu và giá vốn bình quân.
- Nhập hàng: phiếu có nhà cung cấp, ngày, nhiều nguyên liệu, số lượng, đơn giá; ghi sổ cộng tồn, có xem trước / in phiếu.
- Bàn: số chỗ, trạng thái trống / đã đặt / đang phục vụ.
- Đặt bàn: kiểm tra số chỗ, tránh lịch trùng trong 2 giờ, nhận bàn / hủy lịch.
- Hóa đơn: nhiều món, ghi chú, dịch vụ kèm theo, giảm giá, lưu hóa đơn mở, thanh toán tiền mặt / chuyển khoản / thẻ, tiền thừa, hủy hóa đơn chưa thanh toán.
- Thanh toán: kiểm tra công thức và tồn nguyên liệu, trừ kho một lần, giải phóng bàn. Hóa đơn đã thanh toán không được sửa/hủy.
- Xem trước / in hóa đơn hoặc tạm tính; có thể chọn máy in Microsoft Print to PDF.
- Doanh thu theo khoảng ngày, tháng / quý; món và đồ uống bán chạy; xuất CSV, xem trước / in báo cáo. Chỉ hóa đơn đã thanh toán được tính doanh thu.
- Tổng quan lấy số liệu từ hóa đơn thực. Các danh mục phụ cũng được đọc/ghi trong SQL Server.
- Hướng dẫn / Sao lưu: mở thư mục dữ liệu, tạo ZIP gồm dữ liệu và ảnh.

## Bắt đầu sử dụng

1. Kiểm tra món, đơn vị nguyên liệu và công thức. Dữ liệu ban đầu có một số món / công thức / bàn mẫu, tồn kho bằng 0.
2. Tạo phiếu nhập và ghi sổ để có nguyên liệu. Nhập cùng đơn vị cơ sở với công thức (ví dụ g hoặc ml); phần mềm chưa tự quy đổi kg sang g.
3. Nếu khách đặt bàn, tạo lịch. Khi khách tới, chọn Nhận bàn rồi mở hóa đơn ở màn hình Bàn.
4. Thêm món / dịch vụ, lưu hóa đơn mở hoặc thanh toán. Chọn hóa đơn đã lưu để in.
5. Chọn In / Thống kê và khoảng ngày muốn xem.

## View Designer

Mở file form .cs rồi nhấn Shift+F7. Các màn hình trong Forms/Business có file .Designer.cs riêng. Khách hàng, Món ăn, Đơn hàng có các control riêng trong file .Designer.cs để chỉnh trực tiếp. Các danh sách nghiệp vụ khác dùng bố cục chung tại frmEntityList.Designer.cs. Tổng quan có các cột lưới được khai báo trong Designer và định dạng theo DataPropertyName.

## Lưu trữ

Dữ liệu nghiệp vụ và tài khoản nằm trong SQL Server, database **LuaMocDB**, kết nối Windows Authentication qua `App.config`. Lần đầu nhập các tệp JSON cũ nếu có; các lần sau không đọc/ghi JSON làm dữ liệu hoạt động. Ảnh vẫn ở `%LOCALAPPDATA%/LuaMoc/Data/Images`.

Xem [Database/README_SQL_SERVER.md](Database/README_SQL_SERVER.md) để tạo DB, đổi server, sao lưu `.bak` bằng SSMS và chuyển dữ liệu. Bản sao ZIP trong ứng dụng chứa dữ liệu SQL hiện tại và ảnh; không bao gồm tài khoản/mật khẩu.

Chưa tích hợp hóa đơn điện tử hay cổng thanh toán: các hình thức thanh toán ghi nhận thao tác thu tiền của nhân viên.

## Kiểm tra

Bản nâng cấp đã qua 53 kiểm tra tự động về dữ liệu, nhập kho, công thức, thu tiền, tồn kho, đặt bàn, báo cáo, lưu/mở lại và mở 13 màn hình. Đã mở và lưu form hóa đơn qua Visual Studio 2022 View Designer rồi kiểm tra lại. Kiểm thử in ở mức xem trước, không gửi tới máy in vật lý.

## Xuất Excel và in từng đơn hàng

Các danh sách Khách hàng, Đơn hàng, Món ăn dùng mẫu Thêm – Sửa – Xóa – Làm mới. Bấm Xuất Excel để lưu tệp .xlsx gồm các dòng đang hiển thị sau tìm kiếm/lọc, theo thứ tự cột hiện tại; cột nội bộ ẩn không xuất. Số điện thoại/mã dạng chữ giữ số 0 đầu, tiền và ngày được giữ kiểu dữ liệu Excel. Danh sách trống vẫn xuất hàng tiêu đề. Không cần cài Microsoft Excel để xuất tệp.

Ở Đơn hàng, chọn một dòng rồi bấm In đơn hàng để xem trước đúng đơn đó, gồm mã đơn, khách/bàn, chi tiết món, dịch vụ, giảm giá, tổng tiền và thanh toán. Từ cửa sổ xem trước có thể in hoặc chọn Microsoft Print to PDF. Đơn chưa thanh toán in phiếu tạm tính. Nút Xóa tại Đơn hàng hủy đơn chưa thanh toán; hóa đơn đã thanh toán được giữ để đối soát.
