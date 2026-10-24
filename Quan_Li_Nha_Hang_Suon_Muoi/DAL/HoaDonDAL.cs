using System.Data;
using Microsoft.Data.SqlClient;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;

public class HoaDonDAL
{
    private readonly ProcessDatabase db = new();
    public DataTable DanhSach(string tuKhoa = "")
    {
        PhienDangNhap.KiemTraDangNhap();
        return db.DataReader(
            "SELECT MaHoaDon,COALESCE(MaDonCu,N'HD'+CONVERT(nvarchar(20),MaHoaDon)) AS SoHoaDon,NgayLap,TenKhachHang,NguoiLap," +
            "TongTien,TienGiam,ThanhTien,TrangThai,PhuongThuc FROM dbo.HoaDon " +
            "WHERE TenKhachHang LIKE @tim OR CONVERT(nvarchar(20),MaHoaDon) LIKE @tim OR MaDonCu LIKE @tim ORDER BY MaHoaDon DESC",
            new SqlParameter("@tim", "%" + tuKhoa.Trim() + "%"));
    }

    public HoaDon LayChiTiet(int maHoaDon)
    {
        PhienDangNhap.KiemTraDangNhap();
        using var connection = db.OpenConnection();
        using var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);
        using var command = new SqlCommand("SELECT * FROM dbo.HoaDon WHERE MaHoaDon=@ma", connection, transaction);
        command.Parameters.AddWithValue("@ma", maHoaDon);
        var hd = new HoaDon();
        using (var reader = command.ExecuteReader())
        {
            if (!reader.Read())
                throw new InvalidOperationException("Hóa đơn không còn tồn tại.");
            hd.MaHoaDon = maHoaDon;
            hd.MaDonCu = Convert.ToString(reader["MaDonCu"]) ?? "";
            hd.NgayLap = Convert.ToDateTime(reader["NgayLap"]);
            hd.MaKhachHang = Convert.ToString(reader["MaKhachHang"]) ?? "";
            hd.TenKhachHang = Convert.ToString(reader["TenKhachHang"])!;
            hd.NguoiLap = Convert.ToString(reader["NguoiLap"])!;
            hd.TrangThai = Convert.ToString(reader["TrangThai"])!;
            hd.PhuongThuc = Convert.ToString(reader["PhuongThuc"])!;
            hd.TienGiam = Convert.ToDecimal(reader["TienGiam"]);
            hd.TienKhachDua = Convert.ToDecimal(reader["TienKhachDua"]);
            hd.NgayThanhToan = reader["NgayThanhToan"] is DBNull ? null : Convert.ToDateTime(reader["NgayThanhToan"]);
            hd.GhiChu = Convert.ToString(reader["GhiChu"])!;
            hd.PhienBan = (byte[])reader["PhienBan"];
        }
        using var lines = new SqlCommand("SELECT MaMon,TenMon,SoLuong,DonGia FROM dbo.ChiTietHoaDon WHERE MaHoaDon=@ma ORDER BY MaChiTiet", connection, transaction);
        lines.Parameters.AddWithValue("@ma", maHoaDon);
        using (var reader = lines.ExecuteReader())
        {
            while (reader.Read())
                hd.ChiTiet.Add(new ChiTietHoaDon
                {
                    MaMon = Convert.ToString(reader[0]) ?? "",
                    TenMon = reader.GetString(1),
                    SoLuong = reader.GetInt32(2),
                    DonGia = reader.GetDecimal(3)
                });
        }
        transaction.Commit();
        return hd;
    }

    public int Luu(HoaDon hd, bool thanhToan)
    {
        PhienDangNhap.KiemTraDangNhap();
        if (hd.ChiTiet.Count == 0)
            throw new InvalidOperationException("Hóa đơn cần ít nhất một món.");
        if (hd.ChiTiet.Any(d => d.SoLuong < 1 || d.SoLuong > 10000))
            throw new InvalidOperationException("Số lượng phải từ 1 đến 10.000.");
        if (hd.ChiTiet.GroupBy(d => d.MaMon).Any(g => g.Count() > 1))
            throw new InvalidOperationException("Mỗi món chỉ có một dòng; hãy tăng số lượng.");
        if (hd.PhuongThuc != "Tiền mặt" && hd.PhuongThuc != "Chuyển khoản" && hd.PhuongThuc != "Thẻ")
            throw new InvalidOperationException("Phương thức thanh toán không hợp lệ.");
        using var connection = db.OpenConnection();
        using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            var giaCu = new Dictionary<string, ChiTietHoaDon>();
            if (hd.MaHoaDon > 0)
            {
                using var check = new SqlCommand("SELECT TrangThai,PhienBan FROM dbo.HoaDon WITH(UPDLOCK) WHERE MaHoaDon=@ma", connection, transaction);
                check.Parameters.AddWithValue("@ma", hd.MaHoaDon);
                using (var reader = check.ExecuteReader())
                {
                    if (!reader.Read() || reader.GetString(0) != "Chưa thanh toán")
                        throw new InvalidOperationException("Chỉ được sửa hóa đơn chưa thanh toán.");
                    if (!hd.PhienBan.SequenceEqual((byte[])reader[1]))
                        throw new InvalidOperationException("Hóa đơn đã được thay đổi. Đóng và mở lại để tải bản mới.");
                }
                using var old = new SqlCommand("SELECT MaMon,TenMon,DonGia FROM dbo.ChiTietHoaDon WHERE MaHoaDon=@ma", connection, transaction);
                old.Parameters.AddWithValue("@ma", hd.MaHoaDon);
                using var oldReader = old.ExecuteReader();
                while (oldReader.Read())
                {
                    string ma = Convert.ToString(oldReader[0]) ?? "";
                    if (ma.Length > 0)
                        giaCu[ma] = new ChiTietHoaDon { TenMon = oldReader.GetString(1), DonGia = oldReader.GetDecimal(2) };
                }
            }
            // Luôn lấy giá từ DB; các món đã lưu giữ giá chụp tại thời điểm lập hóa đơn.
            foreach (var dong in hd.ChiTiet)
            {
                if (giaCu.TryGetValue(dong.MaMon, out var cu))
                {
                    dong.TenMon = cu.TenMon;
                    dong.DonGia = cu.DonGia;
                    continue;
                }
                using var mon = new SqlCommand("SELECT Name,Price FROM dbo.Foods WHERE FoodId=@ma AND IsAvailable=1", connection, transaction);
                mon.Parameters.AddWithValue("@ma", dong.MaMon);
                using var reader = mon.ExecuteReader();
                if (!reader.Read())
                    throw new InvalidOperationException("Một món đã ngừng bán hoặc không còn tồn tại.");
                dong.TenMon = reader.GetString(0);
                dong.DonGia = reader.GetDecimal(1);
            }
            if (hd.TienGiam < 0 || hd.TienGiam > hd.TongTien)
                throw new InvalidOperationException("Giảm giá phải nằm từ 0 đến tổng tiền.");
            if (hd.TienKhachDua < 0)
                throw new InvalidOperationException("Tiền khách đưa không được âm.");
            if (thanhToan && hd.PhuongThuc == "Tiền mặt" && hd.TienKhachDua < hd.ThanhTien)
                throw new InvalidOperationException("Tiền khách đưa chưa đủ.");
            if (thanhToan && hd.PhuongThuc != "Tiền mặt")
                hd.TienKhachDua = hd.ThanhTien;
            string tenKhach = string.IsNullOrWhiteSpace(hd.TenKhachHang) ? "Khách lẻ" : hd.TenKhachHang.Trim();
            string sql = hd.MaHoaDon == 0
                ? "INSERT dbo.HoaDon(NgayLap,MaKhachHang,TenKhachHang,NguoiLap,TongTien,TienGiam,TienKhachDua,PhuongThuc,TrangThai,NgayThanhToan,GhiChu) " +
                  "OUTPUT INSERTED.MaHoaDon VALUES(SYSDATETIME(),@khach,@ten,@nguoi,@tong,@giam,@dua,@pt,@tt,@ngay,@note)"
                : "UPDATE dbo.HoaDon SET MaKhachHang=@khach,TenKhachHang=@ten,TongTien=@tong,TienGiam=@giam,TienKhachDua=@dua," +
                  "PhuongThuc=@pt,TrangThai=@tt,NgayThanhToan=@ngay,GhiChu=@note WHERE MaHoaDon=@ma; SELECT @ma";
            using var save = new SqlCommand(sql, connection, transaction);
            save.Parameters.AddWithValue("@ma", hd.MaHoaDon);
            save.Parameters.AddWithValue("@khach", hd.MaKhachHang.Length == 0 ? DBNull.Value : hd.MaKhachHang);
            save.Parameters.AddWithValue("@ten", tenKhach);
            save.Parameters.AddWithValue("@nguoi", PhienDangNhap.HienTai!.TenDangNhap);
            Tien(save, "@tong", hd.TongTien);
            Tien(save, "@giam", hd.TienGiam);
            Tien(save, "@dua", hd.TienKhachDua);
            save.Parameters.AddWithValue("@pt", hd.PhuongThuc);
            save.Parameters.AddWithValue("@tt", thanhToan ? "Đã thanh toán" : "Chưa thanh toán");
            save.Parameters.Add("@ngay", SqlDbType.DateTime2).Value = thanhToan ? DateTime.Now : DBNull.Value;
            save.Parameters.AddWithValue("@note", hd.GhiChu.Trim());
            int maHoaDon = Convert.ToInt32(save.ExecuteScalar());
            using var clear = new SqlCommand("DELETE dbo.ChiTietHoaDon WHERE MaHoaDon=@ma", connection, transaction);
            clear.Parameters.AddWithValue("@ma", maHoaDon);
            clear.ExecuteNonQuery();
            foreach (var dong in hd.ChiTiet)
            {
                using var insert = new SqlCommand(
                    "INSERT dbo.ChiTietHoaDon(MaHoaDon,MaMon,TenMon,SoLuong,DonGia) VALUES(@hd,@mon,@ten,@sl,@gia)", connection, transaction);
                insert.Parameters.AddWithValue("@hd", maHoaDon);
                insert.Parameters.AddWithValue("@mon", dong.MaMon);
                insert.Parameters.AddWithValue("@ten", dong.TenMon);
                insert.Parameters.AddWithValue("@sl", dong.SoLuong);
                Tien(insert, "@gia", dong.DonGia);
                insert.ExecuteNonQuery();
            }
            transaction.Commit();
            return maHoaDon;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Huy(HoaDon hd)
    {
        PhienDangNhap.KiemTraDangNhap();
        int soDong = db.DataChange(
            "UPDATE dbo.HoaDon SET TrangThai=N'Đã hủy' WHERE MaHoaDon=@ma AND TrangThai=N'Chưa thanh toán' AND PhienBan=@pb",
            new SqlParameter("@ma", hd.MaHoaDon), new SqlParameter("@pb", hd.PhienBan));
        if (soDong != 1)
            throw new InvalidOperationException("Không thể hủy hóa đơn đã thanh toán hoặc đã được thay đổi.");
    }

    private static void Tien(SqlCommand command, string ten, decimal giaTri)
    {
        var parameter = command.Parameters.Add(ten, SqlDbType.Decimal);
        parameter.Precision = 28;
        parameter.Scale = 8;
        parameter.Value = giaTri;
    }
}
