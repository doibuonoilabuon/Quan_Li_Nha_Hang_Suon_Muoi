# Kết nối SQL Server — SƯỜN MƯỜI

- Server trên máy này: `.` hoặc `LAPTOP-V6HOH5QF` (default instance SQL Server 2022).
- Database: `LuaMocDB`.
- Authentication: **Windows Authentication**.
- Chuỗi kết nối đọc từ `App.config`; không đặt mật khẩu SQL trong mã nguồn.
- Thư viện: `Microsoft.Data.SqlClient` 7.1.1, dành cho project .NET 9 Windows Forms.

## Cài trên máy khác

1. Mở SSMS, kết nối SQL Server bằng Windows Authentication.
2. Mở `CreateDatabase.sql`, Execute. Script chỉ tạo DB/bảng còn thiếu, không DROP database.
3. Chỉnh `Server=` trong `App.config` cho đúng instance (ví dụ `.\SQLEXPRESS`). Build lại project.
4. Chạy ứng dụng. Lần đầu DB chưa khởi tạo, ứng dụng nhập `restaurant.json` và `catalog-*.json` trong `%LOCALAPPDATA%\LuaMoc\Data`; nếu không có, tạo dữ liệu mẫu ban đầu. Việc nhập chạy trong một transaction và chỉ diễn ra một lần. Các tệp cũ vẫn được giữ nguyên.
5. Các lần sau đọc/ghi SQL Server, không dùng JSON làm nơi lưu hoạt động. Ảnh món vẫn ở thư mục `Images` tại máy hiện tại; muốn dùng nhiều máy cần thống nhất thư mục ảnh chia sẻ.

Tài khoản ban đầu khi tạo DB mới: `admin / admin123`, `staff / staff123`. Mật khẩu lưu dạng PBKDF2-SHA256 có salt ngẫu nhiên và 600.000 vòng; không lưu mật khẩu rõ. Chỉ tạo tài khoản ban đầu trong lần nhập đầu, không tự tạo lại khi tài khoản bị xóa/khóa.

## Các bảng

- Nghiệp vụ: MenuItems, Ingredients, RecipeIngredients, ExtraServices, DiningTables, TableReservations, StockReceipts, StockReceiptItems, SalesOrders, SalesOrderItems, SalesOrderServices.
- Danh mục: Customers, MenuCategories, MenuItemOptions, MealCombos, DiscountVouchers, PromotionalBanners, BusinessHours.
- LegacyMenuItems và LegacySalesOrders bảo toàn các bảng danh mục mẫu của phiên bản trước; các màn hình Món ăn/Đơn hàng hiện dùng MenuItems/SalesOrders.
- UserAccounts lưu tài khoản và vai trò; ApplicationState và CatalogRevisions quản lý phiên bản/kiểm tra xung đột.
- Tên khách, tên món, giá trên hóa đơn là bản chụp tại thời điểm bán để giữ lịch sử khi đổi tên/giá danh mục.

View để xem trong SSMS: `vOrderTotals`, `vRevenueMonthly`, `vRevenueQuarterly`, `vBestSellingFoods`, `vTableStatus`. Báo cáo doanh thu chỉ tính đơn đã thanh toán.

Các thao tác nhập kho, thanh toán/trừ kho, cập nhật đơn dùng transaction SQL; nếu có lỗi sẽ rollback toàn bộ. Khi cửa sổ khác cập nhật trước, ứng dụng yêu cầu Làm mới thay vì ghi đè. Không đổi dữ liệu nghiệp vụ trực tiếp trong SSMS khi đang thao tác trên ứng dụng.

## Sao lưu

Trong SSMS: chuột phải LuaMocDB → Tasks → Back Up → Full. Phục hồi `.bak` bằng Tasks → Restore trên SQL Server.

Nút sao lưu ZIP trong ứng dụng xuất bản chụp dữ liệu SQL hiện tại và ảnh. ZIP là bản dữ liệu di chuyển, không bao gồm tài khoản/mật khẩu và không thay thế bản sao lưu SQL đầy đủ. Không phục hồi DB đang dùng bằng cách chỉ chép JSON vào thư mục cũ; dữ liệu SQL hiện tại luôn được ưu tiên.

Để di chuyển sang DB trống: giải nén bản ZIP vào thư mục dữ liệu tại máy mới trước lần khởi tạo DB đầu tiên. DB mới sẽ nhập toàn bộ bản chụp và tạo lại tài khoản ban đầu.

Biến môi trường phục vụ kiểm thử: `LUAMOC_CONNECTION_STRING` ghi đè chuỗi kết nối; `LUAMOC_DATA_DIR` đổi thư mục ảnh/nguồn nhập. Không khai báo hai biến này khi chạy bình thường nếu không cần.

## Chuẩn hóa tên bảng

Tên bảng dùng tiếng Anh, PascalCase và số nhiều cho các tập dữ liệu. `ApplicationState` là dòng trạng thái hệ thống duy nhất.

Nếu nâng cấp DB cũ, dừng ứng dụng và chạy `StandardizeTableNames.sql` trong **LuaMocDB** trước khi mở bản ứng dụng mới. `CreateDatabase.sql` và `AppDatabase.Provision()` cũng tự chạy bước đổi tên này; chạy lại không tạo bảng trùng hay xóa dữ liệu. Script dừng nếu cùng tồn tại cả tên cũ lẫn tên mới. Các view, khóa và chỉ mục được cập nhật cùng giao dịch.

| Tên cũ | Tên mới |
|---|---|
| AppState | ApplicationState |
| CatalogVersions | CatalogRevisions |
| Users | UserAccounts |
| Foods | MenuItems |
| Services | ExtraServices |
| Recipes | RecipeIngredients |
| Reservations | TableReservations |
| Purchases | StockReceipts |
| PurchaseLines | StockReceiptItems |
| Orders | SalesOrders |
| OrderItems | SalesOrderItems |
| OrderServices | SalesOrderServices |
| Categories | MenuCategories |
| FoodOptions | MenuItemOptions |
| Combos | MealCombos |
| Vouchers | DiscountVouchers |
| Banners | PromotionalBanners |
| OpeningHours | BusinessHours |
| LegacyFoods | LegacyMenuItems |
| LegacyOrders | LegacySalesOrders |
| Ingredients | Ingredients |
| DiningTables | DiningTables |
| Customers | Customers |

Tên cột và định dạng dữ liệu import/export được giữ nguyên để tương thích dữ liệu đã có. Các bảng Legacy lưu danh mục mẫu của bản cũ; MenuItems và SalesOrders là dữ liệu nghiệp vụ hiện tại.
