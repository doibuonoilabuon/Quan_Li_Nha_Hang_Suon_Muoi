using System.Data;
using Microsoft.Data.SqlClient;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;

public class MonAnDAL
{
    private readonly ProcessDatabase db = new();
    public DataTable DanhSach(string tuKhoa = "")
    {
        PhienDangNhap.KiemTraDangNhap();
        return db.DataReader(
            "SELECT FoodId AS MaMon,Name AS TenMon,Category AS DanhMuc,Price AS DonGia,IsAvailable AS DangBan,ImageUrl AS Anh " +
            "FROM dbo.Foods WHERE Name LIKE @tim OR Category LIKE @tim ORDER BY Name",
            new SqlParameter("@tim", "%" + tuKhoa.Trim() + "%"));
    }
    public void Luu(MonAn mon)
    {
        PhienDangNhap.KiemTraQuyenQuanTri();
        if (string.IsNullOrWhiteSpace(mon.TenMon) || string.IsNullOrWhiteSpace(mon.DanhMuc) || mon.DonGia <= 0 || mon.DonGia > 1000000000m)
            throw new InvalidOperationException("Nhập tên món, danh mục và đơn giá từ 1 đến 1 tỷ đồng.");
        bool themMoi = string.IsNullOrEmpty(mon.MaMon);
        if (themMoi)
            mon.MaMon = Guid.NewGuid().ToString("N");
        string sql = themMoi
            ? "INSERT dbo.Foods(FoodId,Name,Category,Price,IsAvailable,ImageUrl,Instructions) VALUES(@ma,@ten,@nhom,@gia,@ban,@anh,N'')"
            : "UPDATE dbo.Foods SET Name=@ten,Category=@nhom,Price=@gia,IsAvailable=@ban,ImageUrl=@anh WHERE FoodId=@ma";
        db.DataChange(sql, new SqlParameter("@ma", mon.MaMon), new SqlParameter("@ten", mon.TenMon.Trim()),
            new SqlParameter("@nhom", mon.DanhMuc.Trim()), new SqlParameter("@gia", mon.DonGia),
            new SqlParameter("@ban", mon.DangBan), new SqlParameter("@anh", mon.Anh));
    }
    public void NgungBan(string maMon)
    {
        PhienDangNhap.KiemTraQuyenQuanTri();
      
        db.DataChange("UPDATE dbo.Foods SET IsAvailable=0 WHERE FoodId=@ma", new SqlParameter("@ma", maMon));
    }
}
