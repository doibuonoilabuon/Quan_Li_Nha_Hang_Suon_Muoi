using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
/// <summary>
/// Nguồn dữ liệu minh họa riêng cho Dashboard. Sau này thay bằng service/DAL truy vấn SQL Server.
/// </summary>
public static class DashboardDemoData
{
    public static IReadOnlyList<DashboardOrder> RecentOrders
    {
        get;
    } = new List<DashboardOrder>
    {
        new(
            "LM-1028",
            "Nguyễn Minh Anh",
            485_000m,
            "Chờ xác nhận",
            DateTime.Today.AddMinutes(-12)
        ),
        new(
            "LM-1027",
            "Trần Hoàng Nam",
            320_000m,
            "Đang chuẩn bị",
            DateTime.Today.AddMinutes(-28)
        ),
        new(
            "LM-1026",
            "Lê Thu Hà",
            615_000m,
            "Đang giao",
            DateTime.Today.AddHours(-1)
        ),
        new(
            "LM-1025",
            "Phạm Đức Long",
            279_000m,
            "Hoàn thành",
            DateTime.Today.AddHours(-2)
        ),
        new(
            "LM-1024",
            "Vũ Ngọc Mai",
            440_000m,
            "Hoàn thành",
            DateTime.Today.AddHours(-3)
        ),
        new(
            "LM-1023",
            "Đỗ Quang Huy",
            385_000m,
            "Đang chuẩn bị",
            DateTime.Today.AddHours(-4)
        )
    };

    public static IReadOnlyList<RevenuePoint> GetRevenue(string period)
    {
        if (period == "Tháng này")
        {
            return new List<RevenuePoint>
            {
                new("01–05", 18.6m),
                new("06–10", 22.4m),
                new("11–15", 19.8m),
                new("16–20", 27.5m),
                new("21–25", 24.1m),
                new("26–30", 31.2m)
            };
        }

        return new List<RevenuePoint>
        {
            new("T2", 4.2m),
            new("T3", 5.7m),
            new("T4", 4.9m),
            new("T5", 6.4m),
            new("T6", 7.1m),
            new("T7", 8.45m),
            new("CN", 6.8m)
        };
    }
}
