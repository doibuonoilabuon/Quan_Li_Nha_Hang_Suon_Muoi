using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmMonAn : Form
{
    private readonly MonAnDAL dal = new();
    public frmMonAn()
    {
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        btnXoa.Text = "Ngừng bán";
        btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = PhienDangNhap.LaQuanTri;
        Load += (_, _) => TaiDanhSach();
        txtTim.TextChanged += (_, _) => TaiDanhSach();
        btnLamMoi.Click += (_, _) => TaiDanhSach();
        btnThem.Click += (_, _) => MoSua(new MonAn());
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
            grid.Columns["MaMon"]!.Visible = false;
            grid.Columns["Anh"]!.Visible = false;
            grid.Columns["TenMon"]!.HeaderText = "Tên món";
            grid.Columns["DanhMuc"]!.HeaderText = "Danh mục";
            grid.Columns["DonGia"]!.HeaderText = "Đơn giá";
            grid.Columns["DonGia"]!.DefaultCellStyle.Format = "N0";
            grid.Columns["DangBan"]!.HeaderText = "Đang bán";
            if (!grid.Columns.Contains("AnhXem"))
            {
                grid.Columns.Add(new DataGridViewImageColumn { Name = "AnhXem", HeaderText = "Ảnh", ImageLayout = DataGridViewImageCellLayout.Zoom, FillWeight = 50, DefaultCellStyle = new DataGridViewCellStyle { NullValue = null } });
                grid.CellFormatting += grid_CellFormatting;
                grid.RowTemplate.Height = 70;
            }
            foreach (DataGridViewRow row in grid.Rows)
                row.Height = 70;
            lblSoDong.Text = grid.Rows.Count + " món - bấm Sửa / Xem để chọn hoặc đổi ảnh.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || grid.Columns[e.ColumnIndex].Name != "AnhXem")
            return;
        try
        {
            string anh = Convert.ToString(grid.Rows[e.RowIndex].Cells["Anh"].Value) ?? "";
            e.Value = LayAnh(anh);
            e.FormattingApplied = true;
        }
        catch
        {
            e.Value = null;
            e.FormattingApplied = true;
        }
    }
    private readonly Dictionary<string, Image?> anhDaTai = new();
    private Image? LayAnh(string anh)
    {
        if (!anhDaTai.ContainsKey(anh))
            anhDaTai[anh] = XuLyAnh.MoAnh(XuLyAnh.DuongDan(anh));
        return anhDaTai[anh];
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        foreach (var anh in anhDaTai.Values)
            anh?.Dispose();
        base.Dispose(disposing);
    }
    private void MoSua(MonAn mon)
    {
        using var form = new frmSuaMon(mon);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            foreach (var anh in anhDaTai.Values)
                anh?.Dispose();
            anhDaTai.Clear();
            TaiDanhSach();
        }
    }
    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow == null)
            return;
        var r = grid.CurrentRow;
        MoSua(new MonAn
        {
            MaMon = Convert.ToString(r.Cells["MaMon"].Value)!,
            TenMon = Convert.ToString(r.Cells["TenMon"].Value)!,
            DanhMuc = Convert.ToString(r.Cells["DanhMuc"].Value)!,
            DonGia = Convert.ToDecimal(r.Cells["DonGia"].Value),
            DangBan = Convert.ToBoolean(r.Cells["DangBan"].Value),
            Anh = Convert.ToString(r.Cells["Anh"].Value)!
        });
    }
    private void btnXoa_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow == null || MessageBox.Show("Ngừng bán món này? Hóa đơn cũ vẫn được giữ.", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        try
        {
            dal.NgungBan(Convert.ToString(grid.CurrentRow.Cells["MaMon"].Value)!);
            TaiDanhSach();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
