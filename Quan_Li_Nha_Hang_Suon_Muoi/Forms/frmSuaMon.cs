using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmSuaMon : Form
{
    private readonly MonAn mon;
    private string anhMoi = "";
    private string anhDangLuu = "";
    public frmSuaMon() : this(new MonAn()) { }
    public frmSuaMon(MonAn duLieu)
    {
        mon = duLieu;
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        txtTen.Text = mon.TenMon;
        txtDanhMuc.Text = mon.DanhMuc;
        numGia.Value = Math.Max(1, mon.DonGia);
        chkDangBan.Checked = mon.DangBan;
        anhDangLuu = mon.Anh;
        try
        {
            XuLyAnh.HienThi(picAnh, XuLyAnh.DuongDan(mon.Anh));
        }
        catch
        {
            picAnh.Image = null;
        }
        btnChonAnh.Click += btnChonAnh_Click;
        btnLuu.Click += btnLuu_Click;
        btnXoaAnh.Click += (_, _) =>
        {
            anhMoi = "";
            anhDangLuu = "";
            XuLyAnh.HienThi(picAnh, "");
        };
    }
    private void btnChonAnh_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Chọn ảnh món ăn",
            Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
            InitialDirectory = XuLyAnh.LayThuMucChonAnh(),
            RestoreDirectory = true,
            CheckFileExists = true,
            Multiselect = false
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            XuLyAnh.HienThi(picAnh, dialog.FileName);
            anhMoi = dialog.FileName;
        }
        catch
        {
            MessageBox.Show("Tệp được chọn không phải ảnh hợp lệ.");
        }
    }
    private void btnLuu_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text) || string.IsNullOrWhiteSpace(txtDanhMuc.Text))
                throw new InvalidOperationException("Nhập tên món và danh mục.");
            mon.TenMon = txtTen.Text;
            mon.DanhMuc = txtDanhMuc.Text;
            mon.DonGia = numGia.Value;
            mon.DangBan = chkDangBan.Checked;
            mon.Anh = anhMoi.Length > 0 ? XuLyAnh.LuuAnh(anhMoi) : anhDangLuu;
            new MonAnDAL().Luu(mon);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            picAnh?.Image?.Dispose();
        base.Dispose(disposing);
    }
}
