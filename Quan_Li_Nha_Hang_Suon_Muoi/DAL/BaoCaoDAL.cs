using System.Data;
using Microsoft.Data.SqlClient;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;

public class BaoCaoDAL
{
    private readonly ProcessDatabase db = new();
    public DataTable DoanhThu(DateTime tuNgay, DateTime denNgay, bool theoThang)
    {
        PhienDangNhap.KiemTraDangNhap();
        string nhom = theoThang ? "CONVERT(char(7),NgayThanhToan,120)" : "CONVERT(char(10),NgayThanhToan,120)";
        return db.DataReader(
            "SELECT " + nhom + " AS Ky,COUNT(*) AS SoHoaDon,SUM(TongTien) AS TongTien,SUM(TienGiam) AS TienGiam,SUM(ThanhTien) AS DoanhThu " +
            "FROM dbo.HoaDon WHERE TrangThai=N'Đã thanh toán' AND NgayThanhToan>=@tu AND NgayThanhToan<@den " +
            "GROUP BY " + nhom + " ORDER BY " + nhom,
            new SqlParameter("@tu", tuNgay.Date), new SqlParameter("@den", denNgay.Date.AddDays(1)));
    }
    public DataTable MonBanChay(DateTime tuNgay, DateTime denNgay)
    {
        PhienDangNhap.KiemTraDangNhap();
        return db.DataReader(
            "SELECT c.TenMon,SUM(c.SoLuong) AS SoLuong,SUM(c.ThanhTien) AS TienMon " +
            "FROM dbo.ChiTietHoaDon c JOIN dbo.HoaDon h ON h.MaHoaDon=c.MaHoaDon " +
            "WHERE h.TrangThai=N'Đã thanh toán' AND h.NgayThanhToan>=@tu AND h.NgayThanhToan<@den " +
            "GROUP BY c.MaMon,c.TenMon ORDER BY SoLuong DESC,TienMon DESC",
            new SqlParameter("@tu", tuNgay.Date), new SqlParameter("@den", denNgay.Date.AddDays(1)));
    }
}
