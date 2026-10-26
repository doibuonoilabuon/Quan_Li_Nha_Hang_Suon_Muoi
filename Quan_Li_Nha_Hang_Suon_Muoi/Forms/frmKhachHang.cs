using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.Database;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmKhachHang : Form
{
    private readonly DBProcess db = new();
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
            grid.DataSource = DocDanhSachKhach(txtTim.Text);
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
            NgungHoatDongKhach(Convert.ToString(grid.CurrentRow.Cells["MaKhach"].Value)!);
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

    private DataTable DocDanhSachKhach(string tuKhoa = "", bool chiHoatDong = false)
    {
        PhienDangNhap.KiemTraDangNhap();
        return db.DataReader(
            "SELECT Code AS MaKhach,Name AS HoTen,Phone AS DienThoai,Email,Status AS TrangThai FROM dbo.Customers " +
            "WHERE (Name LIKE @tim OR Phone LIKE @tim OR Code LIKE @tim)" +
            (chiHoatDong ? " AND Status=N'Hoạt động'" : "") + " ORDER BY Name",
            new SqlParameter("@tim", "%" + tuKhoa.Trim() + "%"));
    }
    private void NgungHoatDongKhach(string maKhach)
    {
        PhienDangNhap.KiemTraQuyenQuanTri();
        db.DataChange("UPDATE dbo.Customers SET Status=N'Ngừng hoạt động' WHERE Code=@ma", new SqlParameter("@ma", maKhach));
    }
}
