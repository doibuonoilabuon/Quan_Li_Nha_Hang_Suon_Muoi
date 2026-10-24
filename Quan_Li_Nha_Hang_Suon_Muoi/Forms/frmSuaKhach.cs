using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmSuaKhach : Form
{
    private readonly KhachHang khach;
    public frmSuaKhach() : this(new KhachHang()) { }
    public frmSuaKhach(KhachHang duLieu)
    {
        khach = duLieu;
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        txtTen.Text = khach.HoTen;
        txtSdt.Text = khach.DienThoai;
        txtEmail.Text = khach.Email;
        btnLuu.Click += btnLuu_Click;
    }
    private void btnLuu_Click(object? sender, EventArgs e)
    {
        try
        {
            khach.HoTen = txtTen.Text;
            khach.DienThoai = txtSdt.Text;
            khach.Email = txtEmail.Text;
            new KhachHangDAL().Luu(khach);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
