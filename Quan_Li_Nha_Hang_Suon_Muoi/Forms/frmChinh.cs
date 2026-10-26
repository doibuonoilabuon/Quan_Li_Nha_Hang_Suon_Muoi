using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmChinh : Form
{
    private Form? manHinh;
    public frmChinh()
    {
        InitializeComponent();
     
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        lblNguoi.Text = "SƯỜN MƯỜI | " + PhienDangNhap.HienTai?.HoTen;
        btnMonAn.Click += (_, _) => MoManHinh(new frmMonAn());
        btnKhachHang.Click += (_, _) => MoManHinh(new frmKhachHang());
        btnHoaDon.Click += (_, _) => MoManHinh(new frmHoaDon());
        btnBaoCao.Click += (_, _) => MoManHinh(new frmBaoCao());
        btnDangXuat.Click += (_, _) => Close();
        MoManHinh(new frmHoaDon());
    }
    private void MoManHinh(Form form)
    {
        if (manHinh != null)
        {
            noiDung.Controls.Remove(manHinh);
            manHinh.Dispose();
        }
        manHinh = form;
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        noiDung.Controls.Add(form);
        form.Show();
    }
}
