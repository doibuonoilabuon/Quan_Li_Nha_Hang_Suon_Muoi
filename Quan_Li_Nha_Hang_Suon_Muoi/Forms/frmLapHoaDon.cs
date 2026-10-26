using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Quan_Li_Nha_Hang_Suon_Muoi.Database;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmLapHoaDon : Form
{
    private readonly DBProcess db = new();
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
            PhienDangNhap.KiemTraDangNhap();
            Text = hoaDon.MaHoaDon == 0 ? "Hóa đơn mới" : hoaDon.SoHoaDon + " - " + hoaDon.TrangThai;
            danhSachMon = db.DataReader("SELECT FoodId AS MaMon,Name AS TenMon,Category AS DanhMuc,Price AS DonGia,IsAvailable AS DangBan,ImageUrl AS Anh FROM dbo.Foods ORDER BY Name");
            danhSachMon.DefaultView.RowFilter = "DangBan = true";
            cboMon.DataSource = danhSachMon.DefaultView;
            cboMon.DisplayMember = "TenMon";
            cboMon.ValueMember = "MaMon";
            var khach = db.DataReader("SELECT Code AS MaKhach,Name AS HoTen,Phone AS DienThoai,Email,Status AS TrangThai FROM dbo.Customers WHERE Status=N'Hoạt động' ORDER BY Name");
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
            LuuHoaDon(hoaDon, thanhToan);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private int LuuHoaDon(HoaDon hd, bool thanhToan)
    {
        PhienDangNhap.KiemTraDangNhap();
        if (hd.ChiTiet.Count == 0) throw new InvalidOperationException("Hóa đơn cần ít nhất một món.");
        if (hd.ChiTiet.Any(d => d.SoLuong < 1 || d.SoLuong > 10000))
            throw new InvalidOperationException("Số lượng phải từ 1 đến 10.000.");
        if (hd.ChiTiet.GroupBy(d => d.MaMon).Any(g => g.Count() > 1))
            throw new InvalidOperationException("Mỗi món chỉ có một dòng; hãy tăng số lượng.");
        if (hd.PhuongThuc != "Tiền mặt" && hd.PhuongThuc != "Chuyển khoản" && hd.PhuongThuc != "Thẻ")
            throw new InvalidOperationException("Phương thức thanh toán không hợp lệ.");
        db.BatDauGiaoDich();
        try
        {
            var giaCu = new Dictionary<string, ChiTietHoaDon>();
            if (hd.MaHoaDon > 0)
            {
                DataTable bang = db.DataReader("SELECT TrangThai,PhienBan FROM dbo.HoaDon WITH(UPDLOCK) WHERE MaHoaDon=@ma", new SqlParameter("@ma", hd.MaHoaDon));
                if (bang.Rows.Count == 0 || Convert.ToString(bang.Rows[0]["TrangThai"]) != "Chưa thanh toán")
                    throw new InvalidOperationException("Chỉ được sửa hóa đơn chưa thanh toán.");
                if (!hd.PhienBan.SequenceEqual((byte[])bang.Rows[0]["PhienBan"]))
                    throw new InvalidOperationException("Hóa đơn đã được thay đổi. Đóng và mở lại để tải bản mới.");
                DataTable cu = db.DataReader("SELECT MaMon,TenMon,DonGia FROM dbo.ChiTietHoaDon WHERE MaHoaDon=@ma", new SqlParameter("@ma", hd.MaHoaDon));
                foreach (DataRow r in cu.Rows)
                {
                    string ma = Convert.ToString(r["MaMon"]) ?? "";
                    if (ma.Length > 0) giaCu[ma] = new ChiTietHoaDon { TenMon = Convert.ToString(r["TenMon"])!, DonGia = Convert.ToDecimal(r["DonGia"]) };
                }
            }
            // Món cũ giữ giá lúc bán; món mới luôn lấy giá từ DB.
            foreach (var dong in hd.ChiTiet)
            {
                if (giaCu.TryGetValue(dong.MaMon, out var cu))
                {
                    dong.TenMon = cu.TenMon; dong.DonGia = cu.DonGia;
                    continue;
                }
                DataTable mon = db.DataReader("SELECT Name,Price FROM dbo.Foods WHERE FoodId=@ma AND IsAvailable=1", new SqlParameter("@ma", dong.MaMon));
                if (mon.Rows.Count == 0) throw new InvalidOperationException("Một món đã ngừng bán hoặc không còn tồn tại.");
                dong.TenMon = Convert.ToString(mon.Rows[0]["Name"])!;
                dong.DonGia = Convert.ToDecimal(mon.Rows[0]["Price"]);
            }
            if (hd.TienGiam < 0 || hd.TienGiam > hd.TongTien)
                throw new InvalidOperationException("Giảm giá phải nằm từ 0 đến tổng tiền.");
            if (hd.TienKhachDua < 0) throw new InvalidOperationException("Tiền khách đưa không được âm.");
            if (thanhToan && hd.PhuongThuc == "Tiền mặt" && hd.TienKhachDua < hd.ThanhTien)
                throw new InvalidOperationException("Tiền khách đưa chưa đủ.");
            if (thanhToan && hd.PhuongThuc != "Tiền mặt") hd.TienKhachDua = hd.ThanhTien;
            string tenKhach = string.IsNullOrWhiteSpace(hd.TenKhachHang) ? "Khách lẻ" : hd.TenKhachHang.Trim();
            string sql = hd.MaHoaDon == 0
                ? "INSERT dbo.HoaDon(NgayLap,MaKhachHang,TenKhachHang,NguoiLap,TongTien,TienGiam,TienKhachDua,PhuongThuc,TrangThai,NgayThanhToan,GhiChu) " +
                  "OUTPUT INSERTED.MaHoaDon VALUES(SYSDATETIME(),@khach,@ten,@nguoi,@tong,@giam,@dua,@pt,@tt,@ngay,@note)"
                : "UPDATE dbo.HoaDon SET MaKhachHang=@khach,TenKhachHang=@ten,TongTien=@tong,TienGiam=@giam,TienKhachDua=@dua," +
                  "PhuongThuc=@pt,TrangThai=@tt,NgayThanhToan=@ngay,GhiChu=@note WHERE MaHoaDon=@ma; SELECT @ma";
            DataTable daLuu = db.DataReader(sql,
                new SqlParameter("@ma", hd.MaHoaDon),
                new SqlParameter("@khach", hd.MaKhachHang.Length == 0 ? DBNull.Value : hd.MaKhachHang),
                new SqlParameter("@ten", tenKhach), new SqlParameter("@nguoi", PhienDangNhap.HienTai!.TenDangNhap),
                ThamSoTien("@tong", hd.TongTien), ThamSoTien("@giam", hd.TienGiam), ThamSoTien("@dua", hd.TienKhachDua),
                new SqlParameter("@pt", hd.PhuongThuc), new SqlParameter("@tt", thanhToan ? "Đã thanh toán" : "Chưa thanh toán"),
                new SqlParameter("@ngay", SqlDbType.DateTime2) { Value = thanhToan ? DateTime.Now : DBNull.Value },
                new SqlParameter("@note", hd.GhiChu.Trim()));
            int maHoaDon = Convert.ToInt32(daLuu.Rows[0][0]);
            db.DataChange("DELETE dbo.ChiTietHoaDon WHERE MaHoaDon=@ma", new SqlParameter("@ma", maHoaDon));
            foreach (var dong in hd.ChiTiet)
                db.DataChange("INSERT dbo.ChiTietHoaDon(MaHoaDon,MaMon,TenMon,SoLuong,DonGia) VALUES(@hd,@mon,@ten,@sl,@gia)",
                    new SqlParameter("@hd", maHoaDon), new SqlParameter("@mon", dong.MaMon), new SqlParameter("@ten", dong.TenMon),
                    new SqlParameter("@sl", dong.SoLuong), ThamSoTien("@gia", dong.DonGia));
            db.ChapNhanGiaoDich();
            return maHoaDon;
        }
        catch { db.HuyGiaoDich(); throw; }
    }

    private static SqlParameter ThamSoTien(string ten, decimal giaTri) =>
        new SqlParameter(ten, SqlDbType.Decimal) { Precision = 28, Scale = 8, Value = giaTri };
}
