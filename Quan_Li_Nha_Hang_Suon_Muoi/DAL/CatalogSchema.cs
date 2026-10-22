namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;
internal sealed record CatalogColumn(string Name, string SqlType, string Header);
internal sealed record CatalogDefinition(string Table, CatalogColumn[] Columns);
internal static class CatalogSchema
{
    internal static readonly Dictionary<string, CatalogDefinition> Modules = new()
    {
        ["customers"] = new(
            "Customers",
            [new("Code", "nvarchar(128)", "Mã khách"), new("Name", "nvarchar(256)", "Họ tên"), new("Phone", "nvarchar(64)", "Số điện thoại"), new("Email", "nvarchar(256)", "Email"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["category"] = new(
            "MenuCategories",
            [new("Code", "nvarchar(128)", "Mã danh mục"), new("Name", "nvarchar(256)", "Tên danh mục"), new("Description", "nvarchar(max)", "Mô tả"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["foodoption"] = new(
            "MenuItemOptions",
            [new("Code", "nvarchar(128)", "Mã tùy chọn"), new("Name", "nvarchar(256)", "Tên tùy chọn"), new("AppliesTo", "nvarchar(256)", "Món áp dụng"), new("Surcharge", "decimal(28,8)", "Phụ thu"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["combo"] = new(
            "MealCombos",
            [new("Code", "nvarchar(128)", "Mã combo"), new("Name", "nvarchar(256)", "Tên combo"), new("Price", "decimal(28,8)", "Giá bán"), new("ItemCount", "int", "Số món"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["vouchers"] = new(
            "DiscountVouchers",
            [new("Code", "nvarchar(128)", "Mã voucher"), new("Name", "nvarchar(256)", "Tên chương trình"), new("DiscountType", "nvarchar(40)", "Loại giảm"), new("Value", "decimal(28,8)", "Giá trị"), new("ExpiresOn", "nvarchar(32)", "Hạn sử dụng"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["banners"] = new(
            "PromotionalBanners",
            [new("Code", "nvarchar(128)", "Mã banner"), new("Title", "nvarchar(256)", "Tiêu đề"), new("ImagePath", "nvarchar(2048)", "Ảnh"), new("SortOrder", "int", "Thứ tự"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["openinghours"] = new(
            "BusinessHours",
            [new("Day", "nvarchar(128)", "Thứ"), new("IsOpen", "nvarchar(16)", "Mở cửa"), new("OpenTime", "nvarchar(16)", "Giờ mở"), new("CloseTime", "nvarchar(16)", "Giờ đóng")]
        ),
        ["food"] = new(
            "LegacyMenuItems",
            [new("Code", "nvarchar(128)", "Mã món"), new("Name", "nvarchar(256)", "Tên món"), new("Category", "nvarchar(256)", "Danh mục"), new("Price", "decimal(28,8)", "Giá bán"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
        ["orders"] = new(
            "LegacySalesOrders",
            [new("Code", "nvarchar(128)", "Mã đơn"), new("Customer", "nvarchar(256)", "Khách hàng"), new("OrderedOn", "nvarchar(32)", "Ngày đặt"), new("Total", "decimal(28,8)", "Tổng tiền"), new("Status", "nvarchar(40)", "Trạng thái")]
        ),
    };
}
