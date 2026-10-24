using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmDangNhap : Form
{
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
            var dal = new TaiKhoanDAL();
            var nguoi = dal.DangNhap(txtTen.Text, txtMatKhau.Text);
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
}
