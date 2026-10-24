using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;

public class TaiKhoanDAL
{
    public NguoiDung? DangNhap(string tenDangNhap, string matKhau)
    {
        var db = new ProcessDatabase();
        using var connection = db.OpenConnection();
        using var command = new SqlCommand(
            "SELECT Username,FullName,Role,PasswordHash,Salt,Iterations FROM dbo.Users WHERE Username=@ten AND IsActive=1", connection);
        command.Parameters.AddWithValue("@ten", tenDangNhap.Trim());
        using var reader = command.ExecuteReader();
        if (!reader.Read())
            return null;
        string role = reader.GetString(2);
        if (role != "Admin" && role != "Staff")
            return null;
        int iterations = reader.GetInt32(5);
        if (iterations < 600000 || iterations > 10000000)
            throw new InvalidOperationException("Cấu hình tài khoản không hợp lệ.");
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(matKhau, (byte[])reader[4], iterations, HashAlgorithmName.SHA256, 32);
        if (!CryptographicOperations.FixedTimeEquals(hash, (byte[])reader[3]))
            return null;
        return new NguoiDung { TenDangNhap = reader.GetString(0), HoTen = reader.GetString(1), VaiTro = role };
    }
}
