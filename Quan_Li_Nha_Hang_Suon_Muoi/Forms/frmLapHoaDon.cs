using System.ComponentModel;
using System.Data;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmLapHoaDon : Form
{
    private readonly HoaDon hoaDon;
    private readonly BindingList<ChiTietHoaDon> chiTiet;
    private DataTable danhSachMon = new();
    public frmLapHoaDon() : this(new HoaDon()) { }
    public frmLapHoaDon(HoaDon duLieu)
    {
        hoaDon = duLieu;
        chiTiet = new BindingList<ChiTietHoaDon>(hoaDon.ChiTiet);
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        Load += frmLapHoaDon_Load;
        btnThemMon.Click += btnThemMon_Click;
        btnBoMon.Click += (_, _) =>
        {
            if (grid.CurrentRow?.DataBoundItem is ChiTietHoaDon dong)
                chiTiet.Remove(dong);
            TinhTien();
        };
        btnLuu.Click += (_, _) => Luu(false);
        btnThanhToan.Click += btnThanhToan_Click;
        numGiam.ValueChanged += (_, _) => TinhTien();
        numDua.ValueChanged += (_, _) => TinhTien();
        cboPhuongThuc.SelectedIndexChanged += (_, _) => TinhTien();
        grid.CellValidating += grid_CellValidating;
        grid.CellEndEdit += (_, _) => TinhTien();
        cboKhach.SelectedIndexChanged += cboKhach_SelectedIndexChanged;
    }
    private void frmLapHoaDon_Load(object? sender, EventArgs e)
    {
        if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        try
        {
            Text = hoaDon.MaHoaDon == 0 ? "Hóa đơn mới" : hoaDon.SoHoaDon + " - " + hoaDon.TrangThai;
            danhSachMon = new MonAnDAL().DanhSach();
            danhSachMon.DefaultView.RowFilter = "DangBan = true";
            cboMon.DataSource = danhSachMon.DefaultView;
            cboMon.DisplayMember = "TenMon";
            cboMon.ValueMember = "MaMon";
            var khach = new KhachHangDAL().DanhSach("", true);
            if (hoaDon.MaKhachHang.Length > 0 && !khach.AsEnumerable().Any(r => r.Field<string>("MaKhach") == hoaDon.MaKhachHang))
                khach.Rows.Add(hoaDon.MaKhachHang, hoaDon.TenKhachHang, "", "", "Ngừng hoạt động");
            var le = khach.NewRow();
            le["MaKhach"] = "";
            le["HoTen"] = "Khách lẻ / Nhập tên";
            khach.Rows.InsertAt(le, 0);
            cboKhach.DataSource = khach;
            cboKhach.DisplayMember = "HoTen";
            cboKhach.ValueMember = "MaKhach";
            cboKhach.SelectedValue = hoaDon.MaKhachHang;
            txtTenKhach.Text = hoaDon.TenKhachHang;
            txtGhiChu.Text = hoaDon.GhiChu;
            cboPhuongThuc.Items.AddRange(["Tiền mặt", "Chuyển khoản", "Thẻ"]);
            cboPhuongThuc.SelectedItem = hoaDon.PhuongThuc;
            numGiam.Value = hoaDon.TienGiam;
            numDua.Value = hoaDon.TienKhachDua;
            grid.DataSource = chiTiet;
            grid.Columns["MaMon"]!.Visible = false;
            grid.Columns["TenMon"]!.HeaderText = "Tên món";
            grid.Columns["TenMon"]!.ReadOnly = true;
            grid.Columns["SoLuong"]!.HeaderText = "Số lượng";
            grid.Columns["DonGia"]!.HeaderText = "Đơn giá";
            grid.Columns["DonGia"]!.ReadOnly = true;
            grid.Columns["DonGia"]!.DefaultCellStyle.Format = "N0";
            grid.Columns["ThanhTien"]!.HeaderText = "Thành tiền";
            grid.Columns["ThanhTien"]!.ReadOnly = true;
            grid.Columns["ThanhTien"]!.DefaultCellStyle.Format = "N0";
            bool sua = hoaDon.TrangThai == "Chưa thanh toán";
            DatCheDoChinhSua(sua);
            TinhTien();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            btnLuu.Enabled = btnThanhToan.Enabled = false;
        }
    }
    private void DatCheDoChinhSua(bool choPhepSua)
    {
        khach.Enabled = chonMon.Enabled = tinhTien.Enabled = txtGhiChu.Enabled = btnLuu.Enabled = btnThanhToan.Enabled = choPhepSua;
        grid.ReadOnly = !choPhepSua;
    }
    private void cboKhach_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboKhach.SelectedItem is DataRowView row && Convert.ToString(row["MaKhach"]) != "")
            txtTenKhach.Text = Convert.ToString(row["HoTen"]);
    }
    private void btnThemMon_Click(object? sender, EventArgs e)
    {
        if (cboMon.SelectedItem is not DataRowView row)
            return;
        string ma = Convert.ToString(row["MaMon"])!;
        var dong = chiTiet.FirstOrDefault(d => d.MaMon == ma);
        int them = (int)numSoLuong.Value;
        if (dong != null)
        {
            if (dong.SoLuong + them > 10000)
            {
                MessageBox.Show("Số lượng tối đa 10.000.");
                return;
            }
            dong.SoLuong += them;
            chiTiet.ResetBindings();
        }
        else
            chiTiet.Add(new ChiTietHoaDon { MaMon = ma, TenMon = Convert.ToString(row["TenMon"])!, DonGia = Convert.ToDecimal(row["DonGia"]), SoLuong = them });
        TinhTien();
    }
    private void grid_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (grid.Columns[e.ColumnIndex].DataPropertyName == "SoLuong" &&
            (!int.TryParse(Convert.ToString(e.FormattedValue), out int n) || n < 1 || n > 10000))
        {
            e.Cancel = true;
            MessageBox.Show("Số lượng phải từ 1 đến 10.000.");
        }
    }
    private void TinhTien()
    {
        decimal tong = chiTiet.Sum(d => d.ThanhTien);
        decimal thu = tong - numGiam.Value;
        decimal thua = cboPhuongThuc.Text == "Tiền mặt" ? Math.Max(0, numDua.Value - thu) : 0;
        numDua.Enabled = hoaDon.TrangThai == "Chưa thanh toán" && cboPhuongThuc.Text == "Tiền mặt";
        lblTong.Text = "Tổng: " + DichVuIn.Tien(tong) + "  |  Phải thu: " + DichVuIn.Tien(thu) + "  |  Tiền thừa: " + DichVuIn.Tien(thua);
        grid.Refresh();
    }
    private void btnThanhToan_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("Xác nhận đã thu đủ tiền? Hóa đơn thanh toán sẽ không được sửa/hủy.",
            "Thanh toán", MessageBoxButtons.YesNo) == DialogResult.Yes)
            Luu(true);
    }
    private void Luu(bool thanhToan)
    {
        try
        {
            if (!grid.EndEdit())
                return;
            hoaDon.MaKhachHang = Convert.ToString(cboKhach.SelectedValue) ?? "";
            hoaDon.TenKhachHang = txtTenKhach.Text;
            hoaDon.GhiChu = txtGhiChu.Text;
            hoaDon.TienGiam = numGiam.Value;
            hoaDon.TienKhachDua = numDua.Value;
            hoaDon.PhuongThuc = cboPhuongThuc.Text;
            hoaDon.ChiTiet = chiTiet.ToList();
            new HoaDonDAL().Luu(hoaDon, thanhToan);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
