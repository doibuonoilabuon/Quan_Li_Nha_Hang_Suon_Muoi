using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
/// <summary>Lưu phiên người dùng trong bộ nhớ. Chưa thay thế cơ chế xác thực từ máy chủ.</summary>
public static class Session
{
    public static User? CurrentUser
    {
        get; private set;
    }

    public static bool IsLoggedIn
    {
        get
        {
            return CurrentUser is not null;
        }
    }

    public static void SignIn(User user)
    {
        CurrentUser = user;
    }

    public static void SignOut()
    {
        CurrentUser = null;
    }
}
