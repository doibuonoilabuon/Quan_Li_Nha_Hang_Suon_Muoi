using System.Reflection;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
namespace Quan_Li_Nha_Hang_Suon_Muoi.DAL;

public static class KhoiTaoDatabase
{
    public static void ThucHien()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
            "Quan_Li_Nha_Hang_Suon_Muoi.Database.TaoBang.sql")
            ?? throw new InvalidOperationException(
                "Thiếu tài nguyên Database/TaoBang.sql. Kiểm tra EmbeddedResource trong file .csproj và build lại dự án.");
        using var reader = new StreamReader(stream);
        var db = new ProcessDatabase();
        db.DataChange(reader.ReadToEnd());
      
        using var connection = db.OpenConnection();
        using var count = new SqlCommand("SELECT COUNT(*) FROM dbo.Users", connection);
        if (Convert.ToInt32(count.ExecuteScalar()) != 0)
            return;
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2("admin123", salt, 600000, HashAlgorithmName.SHA256, 32);
        using var insert = new SqlCommand(
            "IF NOT EXISTS(SELECT 1 FROM dbo.Users) INSERT dbo.Users(Username,FullName,Role,PasswordHash,Salt,Iterations,IsActive) " +
            "VALUES(N'admin',N'Quản trị viên',N'Admin',@hash,@salt,600000,1)", connection);
        insert.Parameters.Add("@hash", System.Data.SqlDbType.VarBinary, 32).Value = hash;
        insert.Parameters.Add("@salt", System.Data.SqlDbType.VarBinary, 16).Value = salt;
        insert.ExecuteNonQuery();
    }
}
