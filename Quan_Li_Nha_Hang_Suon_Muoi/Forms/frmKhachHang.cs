using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmKhachHang : Form
{
    private readonly KhachHangDAL dal = new();
    public frmKhachHang()
    {
        InitializeComponent();
       
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = PhienDangNhap.LaQuanTri;
        Load += (_, _) => TaiDanhSach();
        txtTim.TextChanged += (_, _) => TaiDanhSach();
        btnLamMoi.Click += (_, _) => TaiDanhSach();
        btnThem.Click += (_, _) => MoSua(new KhachHang());
        btnSua.Click += btnSua_Click;
        btnXoa.Click += btnXoa_Click;
    }
    private void TaiDanhSach()
    {
        if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        try
        {
            grid.DataSource = dal.DanhSach(txtTim.Text);
            grid.Columns["MaKhach"]!.HeaderText = "Mã khách";
            grid.Columns["HoTen"]!.HeaderText = "Họ tên";
            grid.Columns["DienThoai"]!.HeaderText = "Điện thoại";
            grid.Columns["TrangThai"]!.HeaderText = "Trạng thái";
            lblSoDong.Text = grid.Rows.Count + " khách hàng.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void MoSua(KhachHang khach)
    {
        using var form = new frmSuaKhach(khach);
        if (form.ShowDialog(this) == DialogResult.OK)
            TaiDanhSach();
    }
    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow == null)
            return;
        var r = grid.CurrentRow;
        MoSua(new KhachHang
        {
            MaKhach = Convert.ToString(r.Cells["MaKhach"].Value)!,
            HoTen = Convert.ToString(r.Cells["HoTen"].Value)!,
            DienThoai = Convert.ToString(r.Cells["DienThoai"].Value)!,
            Email = Convert.ToString(r.Cells["Email"].Value)!
        });
    }
    private void btnXoa_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow == null || MessageBox.Show("Ngừng hoạt động khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        try
        {
            dal.NgungHoatDong(Convert.ToString(grid.CurrentRow.Cells["MaKhach"].Value)!);
            TaiDanhSach();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void btnThem_Click(object sender, EventArgs e)
    {

    }
}
