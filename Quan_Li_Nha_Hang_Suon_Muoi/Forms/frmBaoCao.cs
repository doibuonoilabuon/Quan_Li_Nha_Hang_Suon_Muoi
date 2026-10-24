using System.ComponentModel;
using System.Data;
using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmBaoCao : Form
{
    private DataTable doanhThu = new(), monBan = new();
    private DateTime tuDaTai, denDaTai;
    private string kyDaTai = "";
    public frmBaoCao()
    {
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        dtTu.Value = new DateTime(DateTime.Today.Year, 1, 1);
        dtDen.Value = DateTime.Today;
        cboKy.Items.AddRange(["Theo ngày", "Theo tháng"]);
        cboKy.SelectedIndex = 0;
        Load += (_, _) => TaiBaoCao();
        btnXem.Click += (_, _) => TaiBaoCao();
        btnExcel.Click += btnExcel_Click;
        btnIn.Click += btnIn_Click;
    }
    private void TaiBaoCao()
    {
        if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        try
        {
            if (dtTu.Value.Date > dtDen.Value.Date)
                throw new InvalidOperationException("Từ ngày phải nhỏ hơn hoặc bằng đến ngày.");
            var dal = new BaoCaoDAL();
            // Chỉ thay báo cáo khi cả hai truy vấn đều thành công.
            var doanhThuMoi = dal.DoanhThu(dtTu.Value, dtDen.Value, cboKy.SelectedIndex == 1);
            var monBanMoi = dal.MonBanChay(dtTu.Value, dtDen.Value);
            doanhThu = doanhThuMoi;
            monBan = monBanMoi;
            gridDoanhThu.DataSource = doanhThu;
            gridMon.DataSource = monBan;
            string[] ten = ["Kỳ", "Số hóa đơn", "Tổng tiền hàng", "Giảm giá", "Doanh thu thuần"];
            for (int i = 0; i < gridDoanhThu.Columns.Count; i++)
                gridDoanhThu.Columns[i].HeaderText = ten[i];
            gridMon.Columns["TenMon"]!.HeaderText = "Món";
            gridMon.Columns["SoLuong"]!.HeaderText = "Số lượng";
            gridMon.Columns["TienMon"]!.HeaderText = "Tiền món trước giảm";
            foreach (var grid in new[] { gridDoanhThu, gridMon })
                foreach (DataGridViewColumn c in grid.Columns)
                    if (c.ValueType == typeof(decimal))
                        c.DefaultCellStyle.Format = "N0";
            decimal tong = doanhThu.AsEnumerable().Sum(r => r.Field<decimal>("DoanhThu"));
            int so = doanhThu.AsEnumerable().Sum(r => r.Field<int>("SoHoaDon"));
            tuDaTai = dtTu.Value.Date;
            denDaTai = dtDen.Value.Date;
            kyDaTai = cboKy.Text;
            btnExcel.Enabled = true;
            lblTong.Text = $"{tuDaTai:dd/MM/yyyy} - {denDaTai:dd/MM/yyyy}: {so} hóa đơn đã thanh toán | Doanh thu: {DichVuIn.Tien(tong)}\n" +
                "Không tính hóa đơn chưa thanh toán hoặc đã hủy. Món bán chạy tính tiền trước giảm.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void btnExcel_Click(object? sender, EventArgs e)
    {
        try
        {
            bool monBanChay = tabs.SelectedIndex == 1;
            using var dialog = new SaveFileDialog
            {
                Title = "Lưu báo cáo Excel",
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                DefaultExt = "xlsx",
                AddExtension = true,
                OverwritePrompt = true,
                FileName = $"{(monBanChay ? "Mon-ban-chay" : "Doanh-thu")}_{tuDaTai:yyyyMMdd}_{denDaTai:yyyyMMdd}.xlsx"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            XuatExcel.Luu(monBanChay ? monBan : doanhThu, dialog.FileName,
                monBanChay, tuDaTai, denDaTai, kyDaTai);
            MessageBox.Show("Đã xuất báo cáo Excel:\n" + dialog.FileName, "Xuất Excel");
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
            var lines = new List<string>{"SƯỜN MƯỜI - BÁO CÁO THỐNG KÊ",
                $"{tuDaTai:dd/MM/yyyy} - {denDaTai:dd/MM/yyyy} | {kyDaTai}","Chỉ tính hóa đơn đã thanh toán","DOANH THU"};
            foreach (DataRow row in doanhThu.Rows)
                lines.Add(row["Ky"] + " | " + row["SoHoaDon"] + " HĐ | Thực thu: " + DichVuIn.Tien(Convert.ToDecimal(row["DoanhThu"])));
            lines.Add("MÓN BÁN CHẠY - TIỀN TRƯỚC GIẢM");
            foreach (DataRow row in monBan.Rows)
                lines.Add(row["TenMon"] + " x " + row["SoLuong"] + " = " + DichVuIn.Tien(Convert.ToDecimal(row["TienMon"])));
            DichVuIn.XemTruoc(this, "Báo cáo thống kê", lines);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
