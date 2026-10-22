using System.Data;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;
/// <summary>Catalog UI adapter for SQL Server. CreateTable is used only for one-time initial seed data.</summary>
public static class DemoModuleData
{
    private static readonly Dictionary<string, DataTable> Tables = new();
    public static DataTable Get(string key)
    {
        if (!Tables.ContainsKey(key))
        {
            Tables[key] = Load(key);
        }

        return Tables[key];
    }

    private static DataTable Load(string key)
    {
        return Quan_Li_Nha_Hang_Suon_Muoi.DAL.AppDatabase.LoadCatalog(key);
    }

    public static void Save(string key)
    {
        Quan_Li_Nha_Hang_Suon_Muoi.DAL.AppDatabase.SaveCatalog(key, Get(key));
    }

    public static void Reload(string key)
    {
        Tables[key] = Load(key);
    }

    public static DataTable CreateTable(string key)
    {
        string[] columns = key switch
        {
            "food" => ["Mã món", "Tên món", "Danh mục", "Giá bán", "Trạng thái"],
            "category" => ["Mã danh mục", "Tên danh mục", "Mô tả", "Trạng thái"],
            "foodoption" => ["Mã tùy chọn", "Tên tùy chọn", "Món áp dụng", "Phụ thu", "Trạng thái"],
            "combo" => ["Mã combo", "Tên combo", "Giá bán", "Số món", "Trạng thái"],
            "orders" => ["Mã đơn", "Khách hàng", "Ngày đặt", "Tổng tiền", "Trạng thái"],
            "vouchers" => ["Mã voucher", "Tên chương trình", "Loại giảm", "Giá trị", "Hạn sử dụng", "Trạng thái"],
            "customers" => ["Mã khách", "Họ tên", "Số điện thoại", "Email", "Trạng thái"],
            "banners" => ["Mã banner", "Tiêu đề", "Ảnh", "Thứ tự", "Trạng thái"],
            "openinghours" => ["Thứ", "Mở cửa", "Giờ mở", "Giờ đóng"],
            _ => ["Mã", "Tên", "Mô tả", "Trạng thái"]
        };
        var table = new DataTable(key);
        foreach (var col in columns)
        {
            table.Columns.Add(col);
        }

        void Add(params string[] values)
        {
            table.Rows.Add(values);
        }

        switch (key)
        {
            case "food":
                Add(
            "MON001",
            "Sườn nướng mật ong",
            "Sườn nướng",
            "129000",
            "Đang bán"
        );
                Add(
            "MON002",
            "Sườn cay Hàn Quốc",
            "Sườn nướng",
            "139000",
            "Đang bán"
        );
                Add(
            "MON003",
            "Khoai tây chiên",
            "Món ăn kèm",
            "45000",
            "Đang bán"
        );
                Add(
            "MON004",
            "Cơm rang trứng",
            "Cơm",
            "39000",
            "Tạm hết"
        );
                break;
            case "category":
                Add("DM01", "Sườn nướng", "Các món sườn", "Hoạt động");
                Add("DM02", "Món ăn kèm", "Món phụ", "Hoạt động");
                Add("DM03", "Đồ uống", "Nước uống", "Hoạt động");
                break;
            case "foodoption":
                Add(
            "TC01",
            "Thêm phô mai",
            "Sườn nướng mật ong",
            "15000",
            "Hoạt động"
        );
                Add(
            "TC02",
            "Thêm sốt cay",
            "Sườn cay Hàn Quốc",
            "5000",
            "Hoạt động"
        );
                break;
            case "combo":
                Add(
            "CB01",
            "Combo Đôi Ấm Áp",
            "299000",
            "3",
            "Đang bán"
        );
                Add(
            "CB02",
            "Combo Gia Đình",
            "499000",
            "5",
            "Đang bán"
        );
                break;
            case "orders":
                Add(
            "DH1001",
            "Nguyễn Minh Anh",
            DateTime.Today.ToString("dd/MM/yyyy"),
            "299000",
            "Chờ xác nhận"
        );
                Add(
            "DH1002",
            "Trần Hoàng Nam",
            DateTime.Today.ToString("dd/MM/yyyy"),
            "499000",
            "Đang chuẩn bị"
        );
                Add(
            "DH1003",
            "Lê Thu Hà",
            DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy"),
            "159000",
            "Hoàn thành"
        );
                break;
            case "vouchers":
                Add(
            "LUA10",
            "Ưu đãi 10%",
            "%",
            "10",
            "31/12/2026",
            "Hoạt động"
        );
                Add(
            "LUA50K",
            "Giảm 50.000đ",
            "Tiền mặt",
            "50000",
            "30/11/2026",
            "Hoạt động"
        );
                break;
            case "customers":
                Add(
            "KH001",
            "Nguyễn Minh Anh",
            "0987654321",
            "minhanh@example.com",
            "Hoạt động"
        );
                Add(
            "KH002",
            "Trần Hoàng Nam",
            "0912345678",
            "hoangnam@example.com",
            "Hoạt động"
        );
                break;
            case "banners":
                Add(
            "BN01",
            "Ưu đãi cuối tuần",
            "banner_weekend.jpg",
            "1",
            "Đang hiển thị"
        );
                Add(
            "BN02",
            "Combo mới",
            "combo.jpg",
            "2",
            "Đang hiển thị"
        );
                break;
            case "openinghours":
                foreach (var day in new[]
                {
                    "Thứ Hai",
                    "Thứ Ba",
                    "Thứ Tư",
                    "Thứ Năm",
                    "Thứ Sáu",
                    "Thứ Bảy",
                    "Chủ nhật"
                }

                )
                {
                    Add(day, "Có", "10:00", "22:00");
                }

                break;
        }

        return table;
    }
}
