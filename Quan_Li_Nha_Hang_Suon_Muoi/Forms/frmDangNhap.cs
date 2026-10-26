using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Security.Cryptography;
using Quan_Li_Nha_Hang_Suon_Muoi.Database;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmDangNhap : Form
{
    private readonly DBProcess db = new();
    public frmDangNhap()
    {
        InitializeComponent();
       
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        btnDangNhap.Click += btnDangNhap_Click;
    }
    private void TogglePassword(object? sender, EventArgs e)
    {
        txtMatKhau.UseSystemPasswordChar = !chkShow.Checked;
    }

    private void btnDangNhap_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrEmpty(txtMatKhau.Text))
                throw new InvalidOperationException("Nhập tên đăng nhập và mật khẩu.");
            var nguoi = KiemTraTaiKhoan(txtTen.Text, txtMatKhau.Text);
            if (nguoi == null)
            {
                lblLoi.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return;
            }
            PhienDangNhap.HienTai = nguoi;
            Hide();
            using (var main = new frmChinh())
                main.ShowDialog();
            PhienDangNhap.HienTai = null;
            txtMatKhau.Clear();
            lblLoi.Text = "";
            Show();
        }
        catch (Exception ex)
        {
            lblLoi.Text = ex.Message;
        }
    }

    private void txtMatKhau_TextChanged(object sender, EventArgs e)
    {

    }

    private NguoiDung? KiemTraTaiKhoan(string tenDangNhap, string matKhau)
    {
        DataTable bang = db.DataReader(
            "SELECT Username,FullName,Role,PasswordHash,Salt,Iterations FROM dbo.Users WHERE Username=@ten AND IsActive=1",
            new SqlParameter("@ten", tenDangNhap.Trim()));
        if (bang.Rows.Count == 0) return null;
        DataRow r = bang.Rows[0];
        string role = Convert.ToString(r["Role"])!;
        if (role != "Admin" && role != "Staff") return null;
        int iterations = Convert.ToInt32(r["Iterations"]);
        if (iterations < 600000 || iterations > 10000000) throw new InvalidOperationException("Cấu hình tài khoản không hợp lệ.");
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(matKhau, (byte[])r["Salt"], iterations, HashAlgorithmName.SHA256, 32);
        if (!CryptographicOperations.FixedTimeEquals(hash, (byte[])r["PasswordHash"])) return null;
        return new NguoiDung { TenDangNhap = Convert.ToString(r["Username"])!, HoTen = Convert.ToString(r["FullName"])!, VaiTro = role };
    }
}
