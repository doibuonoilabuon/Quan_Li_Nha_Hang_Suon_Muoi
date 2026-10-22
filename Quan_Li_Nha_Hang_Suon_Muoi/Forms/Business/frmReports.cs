using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmReports : Form
{
    private List<RevenueSummary> revenue = [];
    private List<BestSeller> best = [];
    private DateTime loadedFrom, loadedTo;
    private string loadedPeriod = "";
    public frmReports()
    {
        InitializeComponent();
        Shown += (_, _) => Ui.Fit(this);
        Text = "Báo cáo / Thống kê";
        Ui.Grid(gridRevenue);
        Ui.Grid(gridBest);
        from.Value = new DateTime(DateTime.Today.Year, 1, 1);
        to.Value = DateTime.Today;
        period.SelectedIndex = 0;
        btnLoad.Click += (_, _) => Ui.Run(this, LoadReport);
        btnPrint.Click += (_, _) => Ui.Run(this, Print);
        btnCsv.Click += (_, _) => Ui.Run(this, Export);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!Ui.IsDesigner(this))
        {
            Ui.Run(this, LoadReport);
        }
    }

    private void LoadReport()
    {
        if (from.Value.Date > to.Value.Date)
        {
            throw new InvalidOperationException("Ngày bắt đầu phải trước hoặc bằng ngày kết thúc.");
        }

        RestaurantStore.Current.Reload();
        var s = RestaurantStore.Current.Snapshot();
        loadedFrom = from.Value.Date;
        loadedTo = to.Value.Date;
        loadedPeriod = period.Text;
        revenue = Reports.Revenue(s, loadedFrom, loadedTo, period.SelectedIndex == 1);
        best = Reports.Best(s, loadedFrom, loadedTo);
        gridRevenue.DataSource = revenue.Select(
            x => new { Kỳ = x.Period, Số_hóa_đơn = x.Receipts, Tiền_hàng_dịch_vụ = x.Gross, Giảm_giá = x.Discount, Doanh_thu_thuần = x.Net }
        )
            .ToList();
        gridBest.DataSource = best.Select(
            x => new { Món_đồ_uống = x.Food, Danh_mục = x.Category, Số_lượng = x.Quantity, Tiền_món_trước_giảm = x.Revenue }
        )
            .ToList();
        foreach (var grid in new[]
        {
            gridRevenue,
            gridBest
        }

        )
        {
            foreach (DataGridViewColumn c in grid.Columns)
            {
                if (c.ValueType == typeof(decimal))
                {
                    c.DefaultCellStyle.Format = "N0";
                }
            }
        }

        summary.Text = $"{loadedFrom:dd/MM/yyyy} – {loadedTo:dd/MM/yyyy} • {revenue.Sum(x => x.Receipts)} hóa đơn đã thanh toán • Thực thu: {Reports.Money(revenue.Sum(x => x.Net))}\nKhông tính đơn đang mở / đã hủy. Bảng bán chạy thống kê số lượng và tiền món trước giảm; dịch vụ được tính trong doanh thu hóa đơn.";
    }

    private void Print()
    {
        var lines = new List<string>
        {
            "SƯỜN MƯỜI - BÁO CÁO KINH DOANH",
            $"{loadedFrom:dd/MM/yyyy} – {loadedTo:dd/MM/yyyy} • {loadedPeriod}",
            "Chỉ tính hóa đơn đã thanh toán",
            "DOANH THU"
        };
        lines.AddRange(
            revenue.Select(
                x => $"{x.Period}: {x.Receipts} HĐ | Tiền hàng/DV {Reports.Money(x.Gross)} | Giảm {Reports.Money(x.Discount)} | Thực thu {Reports.Money(x.Net)}"
            )
        );
        lines.Add("TỔNG THỰC THU: " + Reports.Money(revenue.Sum(x => x.Net)));
        lines.Add("MÓN / ĐỒ UỐNG BÁN CHẠY (TIỀN TRƯỚC GIẢM)");
        lines.AddRange(best.Select(x => $"{x.Food} ({x.Category}): {x.Quantity} phần | {Reports.Money(x.Revenue)}"));
        Reports.Preview(this, "Báo cáo doanh thu", lines);
    }

    private void Export()
    {
        var rows = new List<string[]>
        {
            new[]
            {
                "Từ",
                loadedFrom.ToString("yyyy-MM-dd"),
                "Đến",
                loadedTo.ToString("yyyy-MM-dd")
            }
        };
        if (tabs.SelectedIndex == 0)
        {
            rows.Add(["Kỳ", "Số hóa đơn", "Tiền hàng và dịch vụ", "Giảm giá", "Doanh thu thuần"]);
            rows.AddRange(
                revenue.Select(
                    x => new[] { x.Period, x.Receipts.ToString(), x.Gross.ToString(System.Globalization.CultureInfo.InvariantCulture), x.Discount.ToString(System.Globalization.CultureInfo.InvariantCulture), x.Net.ToString(System.Globalization.CultureInfo.InvariantCulture) }
                )
            );
        }
        else
        {
            rows.Add(["Món", "Danh mục", "Số lượng", "Tiền trước giảm"]);
            rows.AddRange(
                best.Select(
                    x => new[] { x.Food, x.Category, x.Quantity.ToString(), x.Revenue.ToString(System.Globalization.CultureInfo.InvariantCulture) }
                )
            );
        }

        Reports.ExportCsv(this, tabs.SelectedIndex == 0 ? "doanh-thu" : "mon-ban-chay", rows);
    }
}
