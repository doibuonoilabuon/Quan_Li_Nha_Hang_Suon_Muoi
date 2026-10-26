using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.Database;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmHoaDon : Form
{
    private readonly DBProcess db = new();
    public frmHoaDon()
    {
        InitializeComponent();
     
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
        btnExcel.Click += btnExcel_Click;
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
            grid.DataSource = DocDanhSachHoaDon(txtTim.Text);
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
        return DocHoaDon(Convert.ToInt32(grid.CurrentRow.Cells["MaHoaDon"].Value));
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
            HuyHoaDon(hd);
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

    private DataTable DocDanhSachHoaDon(string tuKhoa = "")
    {
        PhienDangNhap.KiemTraDangNhap();
        return db.DataReader(
            "SELECT MaHoaDon,COALESCE(MaDonCu,N'HD'+CONVERT(nvarchar(20),MaHoaDon)) AS SoHoaDon,NgayLap,TenKhachHang,NguoiLap," +
            "TongTien,TienGiam,ThanhTien,TrangThai,PhuongThuc FROM dbo.HoaDon " +
            "WHERE TenKhachHang LIKE @tim OR CONVERT(nvarchar(20),MaHoaDon) LIKE @tim OR MaDonCu LIKE @tim ORDER BY MaHoaDon DESC",
            new SqlParameter("@tim", "%" + tuKhoa.Trim() + "%"));
    }
    private HoaDon DocHoaDon(int maHoaDon)
    {
        PhienDangNhap.KiemTraDangNhap();
        db.BatDauGiaoDich(IsolationLevel.RepeatableRead);
        try
        {
            DataTable bang = db.DataReader("SELECT * FROM dbo.HoaDon WHERE MaHoaDon=@ma", new SqlParameter("@ma", maHoaDon));
            if (bang.Rows.Count == 0) throw new InvalidOperationException("Hóa đơn không còn tồn tại.");
            DataRow r = bang.Rows[0];
            var hd = new HoaDon
            {
                MaHoaDon = maHoaDon, MaDonCu = Convert.ToString(r["MaDonCu"]) ?? "",
                NgayLap = Convert.ToDateTime(r["NgayLap"]), MaKhachHang = Convert.ToString(r["MaKhachHang"]) ?? "",
                TenKhachHang = Convert.ToString(r["TenKhachHang"])!, NguoiLap = Convert.ToString(r["NguoiLap"])!,
                TrangThai = Convert.ToString(r["TrangThai"])!, PhuongThuc = Convert.ToString(r["PhuongThuc"])!,
                TienGiam = Convert.ToDecimal(r["TienGiam"]), TienKhachDua = Convert.ToDecimal(r["TienKhachDua"]),
                NgayThanhToan = r["NgayThanhToan"] is DBNull ? null : Convert.ToDateTime(r["NgayThanhToan"]),
                GhiChu = Convert.ToString(r["GhiChu"])!, PhienBan = (byte[])r["PhienBan"]
            };
            DataTable chiTiet = db.DataReader("SELECT MaMon,TenMon,SoLuong,DonGia FROM dbo.ChiTietHoaDon WHERE MaHoaDon=@ma ORDER BY MaChiTiet", new SqlParameter("@ma", maHoaDon));
            foreach (DataRow dong in chiTiet.Rows)
                hd.ChiTiet.Add(new ChiTietHoaDon
                {
                    MaMon = Convert.ToString(dong["MaMon"]) ?? "", TenMon = Convert.ToString(dong["TenMon"])!,
                    SoLuong = Convert.ToInt32(dong["SoLuong"]), DonGia = Convert.ToDecimal(dong["DonGia"])
                });
            db.ChapNhanGiaoDich();
            return hd;
        }
        catch { db.HuyGiaoDich(); throw; }
    }
    private void HuyHoaDon(HoaDon hd)
    {
        PhienDangNhap.KiemTraDangNhap();
        int soDong = db.DataChange(
            "UPDATE dbo.HoaDon SET TrangThai=N'Đã hủy' WHERE MaHoaDon=@ma AND TrangThai=N'Chưa thanh toán' AND PhienBan=@pb",
            new SqlParameter("@ma", hd.MaHoaDon), new SqlParameter("@pb", hd.PhienBan));
        if (soDong != 1)
            throw new InvalidOperationException("Không thể hủy hóa đơn đã thanh toán hoặc đã được thay đổi.");
    }
    private void btnExcel_Click(object? sender, EventArgs e)
    {
        try
        {
            HoaDon hd = DaChon();
            using var dialog = new SaveFileDialog
            {
                Title = "Xuất hóa đơn ra Excel", Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx", AddExtension = true, OverwritePrompt = true,
                FileName = $"Hoa-don-{hd.MaHoaDon}_{hd.NgayLap:yyyyMMdd}.xlsx"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            XuatExcel.LuuHoaDon(hd, dialog.FileName);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Xuất Excel"); }
    }
}
