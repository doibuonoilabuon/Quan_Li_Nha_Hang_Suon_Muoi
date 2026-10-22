using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Dashboard;
public partial class frmDashboard : Form
{
    public frmDashboard()
    {
        InitializeComponent();
        ConfigureOrderGrid();
        revenueChart.SetData(DashboardDemoData.GetRevenue("7 ngày qua"));
        gridRecent.DataSource = DashboardDemoData.RecentOrders.Take(5).ToList();
        cboPeriod.SelectedIndex = 0;
        cboPeriod.SelectedIndexChanged += (_, _) => UpdateChart();
        btnRefresh.Click += (_, _) => RefreshDashboard();
        btnViewAll.Click += (_, _) =>
        {
            using var orders = new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business.frmSales();
            orders.StartPosition = FormStartPosition.CenterParent;
            orders.Size = new Size(1100, 700);
            orders.ShowDialog(this);
        };
        gridRecent.CellFormatting += FormatOrderCell;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (LicenseManager.UsageMode != LicenseUsageMode.Designtime && !DesignMode)
        {
            RefreshDashboard();
        }
    }

    private void RefreshDashboard()
    {
        RestaurantStore.Current.Reload();
        DemoModuleData.Reload("customers");
        var s = RestaurantStore.Current.Snapshot();
        var today = s.Sales.Where(x => x.Status == "Đã thanh toán" && x.PaidAt?.Date == DateTime.Today).ToList();
        valueRevenue.Text = Reports.Money(today.Sum(x => x.Total));
        valueOrders.Text = s.Sales.Count(
            x => x.CreatedAt.Year == DateTime.Today.Year && x.CreatedAt.Month == DateTime.Today.Month
                && x.Status != "Đã hủy"
        )
            .ToString();
        valueNew.Text = s.Sales.Count(x => x.Status == "Đang phục vụ").ToString();
        valueCustomers.Text = DemoModuleData.Get("customers").Rows.Count.ToString();
        titleOrders.Text = "TỔNG ĐƠN HÀNG";
        titleNew.Text = "ĐƠN HÀNG MỚI";
        noteRevenue.Text = "Từ đơn hàng đã thanh toán hôm nay";
        noteNew.Text = "Cần được kiểm tra";
        noteCustomers.Text = "Khách hàng đã lưu";
        statusTitlePending.Text = "Chờ xác nhận";
        statusValuePending.Text = s.Sales.Count(x => x.Status is "Chờ xác nhận" or "Đang phục vụ").ToString("00") + " đơn";
        statusTitlePreparing.Text = "Đang chuẩn bị";
        statusValuePreparing.Text = s.Sales.Count(x => x.Status == "Đang chuẩn bị").ToString("00") + " đơn";
        statusTitleDelivery.Text = "Đang giao hàng";
        statusValueDelivery.Text = s.Sales.Count(x => x.Status == "Đang giao").ToString("00") + " đơn";
        lblDemo.Text = "SQL SERVER • Kết nối LuaMocDB • Dữ liệu cập nhật từ cơ sở dữ liệu.";
        lblRevenueUnit.Text = "Đơn vị: triệu đồng • Doanh thu đã thanh toán";
        gridRecent.DataSource = s.Sales.OrderByDescending(x => x.CreatedAt).Take(5)
            .Select(x => new DashboardOrder(
                    x.Id,
                    x.Customer,
                    x.Total,
                    x.Status,
                    x.CreatedAt
                ))
            .ToList();
        UpdateChart();
    }

    private void UpdateChart()
    {
        var s = RestaurantStore.Current.Snapshot();
        var points = new List<RevenuePoint>();
        if (cboPeriod.Text == "Tháng này")
        {
            var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            for (int day = 1; day <= DateTime.Today.Day; day += 5)
            {
                var a = start.AddDays(day - 1);
                var b = a.AddDays(5);
                points.Add(
                    new RevenuePoint(
                        $"{day:00}–{Math.Min(day + 4, DateTime.Today.Day):00}",
                        Reports.Paid(s, a, b.AddDays(-1)).Sum(x => x.Total) / 1000000m
                    )
                );
            }
        }
        else
        {
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Today.AddDays(-i);
                points.Add(
                    new RevenuePoint(date.ToString("dd/MM"), Reports.Paid(s, date, date).Sum(x => x.Total) / 1000000m)
                );
            }
        }

        revenueChart.SetData(points);
    }

    // Restore the schema if the WinForms Designer has removed the serialized columns.
    private void ConfigureOrderGrid()
    {
        gridRecent.AutoGenerateColumns = false;
        if (gridRecent.Columns.Count != 0)
        {
            return;
        }

        gridRecent.Columns.AddRange(
            new DataGridViewTextBoxColumn { Name = "colCode", DataPropertyName = "OrderCode", HeaderText = "Mã đơn", FillWeight = 19, ReadOnly = true },
            new DataGridViewTextBoxColumn { Name = "colCustomer", DataPropertyName = "CustomerName", HeaderText = "Khách hàng", FillWeight = 30, ReadOnly = true },
            new DataGridViewTextBoxColumn { Name = "colAmount", DataPropertyName = "TotalAmount", HeaderText = "Tổng tiền", FillWeight = 23, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "#,##0 'đ'", FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN") } },
            new DataGridViewTextBoxColumn { Name = "colStatus", DataPropertyName = "Status", HeaderText = "Trạng thái", FillWeight = 28, ReadOnly = true }
        );
        foreach (DataGridViewColumn column in gridRecent.Columns)
        {
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }

    private void FormatOrderCell(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (sender is not DataGridView grid || e.RowIndex < 0 || e.ColumnIndex < 0
            || e.ColumnIndex >= grid.Columns.Count
            || grid.Columns[e.ColumnIndex].DataPropertyName != "Status"
            || e.Value is not string status
            || e.CellStyle is null)
        {
            return;
        }

        e.CellStyle.ForeColor = status switch
        {
            "Đang phục vụ" or "Đang chuẩn bị" => Color.FromArgb(190, 112, 20),
            "Đang giao" => Color.FromArgb(65, 113, 159),
            "Đã thanh toán" or "Hoàn thành" => Color.FromArgb(0, 128, 98),
            _ => Color.FromArgb(36, 32, 29)
        };
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        e.CellStyle.Font = gridRecent.ColumnHeadersDefaultCellStyle.Font;
    }

    private void statusPending_Paint(object sender, PaintEventArgs e)
    {
    }
}
