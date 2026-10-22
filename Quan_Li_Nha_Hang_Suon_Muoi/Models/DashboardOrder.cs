namespace Quan_Li_Nha_Hang_Suon_Muoi.Models;
/// <summary>Thông tin đơn hàng dùng trên Dashboard. Hiện được nạp bằng dữ liệu demo.</summary>
public sealed record DashboardOrder(string OrderCode, string CustomerName, decimal TotalAmount, string Status, DateTime CreatedAt);
