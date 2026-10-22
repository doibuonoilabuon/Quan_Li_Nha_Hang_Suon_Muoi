using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
/// <summary>Compatibility name for the login form; authentication now uses dbo.UserAccounts.</summary>
public static class DemoAuthService
{
    public static bool TryLogin(string username, string password, out User? user, out string message)
    {
        user = null;
        message = "Tên đăng nhập hoặc mật khẩu chưa chính xác.";
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            message = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
            return false;
        }

        try
        {
            user = AppDatabase.Authenticate(username, password);
            if (user == null)
            {
                return false;
            }

            message = string.Empty;
            return true;
        }
        catch (Exception e) when (e is System.Data.Common.DbException or InvalidOperationException or IOException)
        {
            message = "Không truy cập được tài khoản trong SQL Server. " + e.Message;
            return false;
        }
    }
}
