using System.Data;
using Microsoft.Data.SqlClient;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;

public class KhachHangDAL
{
    private readonly ProcessDatabase db = new();
    public DataTable DanhSach(string tuKhoa = "", bool chiHoatDong = false)
    {
        PhienDangNhap.KiemTraDangNhap();
        return db.DataReader(
            "SELECT Code AS MaKhach,Name AS HoTen,Phone AS DienThoai,Email,Status AS TrangThai FROM dbo.Customers " +
            "WHERE (Name LIKE @tim OR Phone LIKE @tim OR Code LIKE @tim)" +
            (chiHoatDong ? " AND Status=N'Hoạt động'" : "") + " ORDER BY Name",
            new SqlParameter("@tim", "%" + tuKhoa.Trim() + "%"));
    }
    public void Luu(KhachHang khach)
    {
        PhienDangNhap.KiemTraQuyenQuanTri();
        if (string.IsNullOrWhiteSpace(khach.HoTen))
            throw new InvalidOperationException("Nhập họ tên khách hàng.");
        bool themMoi = string.IsNullOrEmpty(khach.MaKhach);
        if (themMoi)
            khach.MaKhach = "KH" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        string sql = themMoi
            ? "INSERT dbo.Customers(Code,Name,Phone,Email,Status) VALUES(@ma,@ten,@sdt,@email,N'Hoạt động')"
            : "UPDATE dbo.Customers SET Name=@ten,Phone=@sdt,Email=@email WHERE Code=@ma";
        db.DataChange(sql, new SqlParameter("@ma", khach.MaKhach), new SqlParameter("@ten", khach.HoTen.Trim()),
            new SqlParameter("@sdt", khach.DienThoai.Trim()), new SqlParameter("@email", khach.Email.Trim()));
    }
    public void NgungHoatDong(string maKhach)
    {
        PhienDangNhap.KiemTraQuyenQuanTri();
        db.DataChange("UPDATE dbo.Customers SET Status=N'Ngừng hoạt động' WHERE Code=@ma", new SqlParameter("@ma", maKhach));
    }
}
