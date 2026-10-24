using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmHoaDon : Form
{
    private readonly HoaDonDAL dal = new();
    public frmHoaDon()
    {
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        btnXoa.Text = "Hủy hóa đơn";
        Load += (_, _) => TaiDanhSach();
        txtTim.TextChanged += (_, _) => TaiDanhSach();
        btnLamMoi.Click += (_, _) => TaiDanhSach();
        btnThem.Click += (_, _) => MoSua(new HoaDon());
        btnSua.Click += btnSua_Click;
        btnXoa.Click += btnXoa_Click;
        btnIn.Click += btnIn_Click;
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
                btnSua_Click(null, EventArgs.Empty);
        };
    }
    private void TaiDanhSach()
    {
        if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        try
        {
            grid.DataSource = dal.DanhSach(txtTim.Text);
            grid.Columns["MaHoaDon"]!.Visible = false;
            string[] ten = ["Số hóa đơn", "Ngày lập", "Khách hàng", "Người lập", "Tổng tiền", "Giảm giá", "Thành tiền", "Trạng thái", "Phương thức"];
            for (int i = 1; i < grid.Columns.Count; i++)
                grid.Columns[i].HeaderText = ten[i - 1];
            grid.Columns["NgayLap"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            grid.Columns["TongTien"]!.DefaultCellStyle.Format = "N0";
            grid.Columns["TienGiam"]!.DefaultCellStyle.Format = "N0";
            grid.Columns["ThanhTien"]!.DefaultCellStyle.Format = "N0";
            lblSoDong.Text = grid.Rows.Count + " hóa đơn - chỉ hóa đơn chưa thanh toán được sửa hoặc hủy.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private HoaDon DaChon()
    {
        if (grid.CurrentRow == null)
            throw new InvalidOperationException("Chọn một hóa đơn.");
        return dal.LayChiTiet(Convert.ToInt32(grid.CurrentRow.Cells["MaHoaDon"].Value));
    }
    private void MoSua(HoaDon hd)
    {
        using var form = new frmLapHoaDon(hd);
        if (form.ShowDialog(this) == DialogResult.OK)
            TaiDanhSach();
    }
    private void btnSua_Click(object? sender, EventArgs e)
    {
        try
        {
            MoSua(DaChon());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void btnXoa_Click(object? sender, EventArgs e)
    {
        try
        {
            var hd = DaChon();
            if (MessageBox.Show("Hủy hóa đơn " + hd.SoHoaDon + "?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            dal.Huy(hd);
            TaiDanhSach();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void btnIn_Click(object? sender, EventArgs e)
    {
        try
        {
            DichVuIn.XemHoaDon(this, DaChon());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
