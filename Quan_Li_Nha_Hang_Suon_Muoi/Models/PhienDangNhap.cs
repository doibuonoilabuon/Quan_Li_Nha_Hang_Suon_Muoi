namespace Quan_Li_Nha_Hang_Suon_Muoi.Models;

public class NguoiDung
{
    public string TenDangNhap
    {
        get;
        set;
    } = "";
    public string HoTen
    {
        get;
        set;
    } = "";
    public string VaiTro
    {
        get;
        set;
    } = "";
}
public static class PhienDangNhap
{
    public static NguoiDung? HienTai
    {
        get;
        set;
    }
    public static bool LaQuanTri => HienTai?.VaiTro == "Admin";
    public static void KiemTraDangNhap()
    {
        if (HienTai == null)
            throw new InvalidOperationException("Vui lòng đăng nhập.");
    }
    public static void KiemTraQuyenQuanTri()
    {
        KiemTraDangNhap();
        if (!LaQuanTri)
            throw new InvalidOperationException("Chỉ quản trị viên được thay đổi danh mục.");
    }
}
