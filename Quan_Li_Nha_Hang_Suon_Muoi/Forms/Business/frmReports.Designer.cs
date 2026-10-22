#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmReports
{
    private System.ComponentModel.IContainer components;
    private FlowLayoutPanel filters;
    private Label lblFrom;
    private DateTimePicker from;
    private Label lblTo;
    private DateTimePicker to;
    private ComboBox period;
    private Button btnLoad;
    private Button btnPrint;
    private Button btnCsv;
    private TabControl tabs;
    private TabPage revenueTab;
    private TabPage bestTab;
    private DataGridView gridRevenue;
    private DataGridView gridBest;
    private Label summary;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        filters = new FlowLayoutPanel();
        lblFrom = new Label();
        from = new DateTimePicker();
        lblTo = new Label();
        to = new DateTimePicker();
        period = new ComboBox();
        btnLoad = new Button();
        btnPrint = new Button();
        btnCsv = new Button();
        tabs = new TabControl();
        revenueTab = new TabPage();
        bestTab = new TabPage();
        gridRevenue = new DataGridView();
        gridBest = new DataGridView();
        summary = new Label();
        SuspendLayout();
        filters.Name = "filters";
        filters.Dock = DockStyle.Top;
        filters.Height = 100;
        filters.AutoScroll = true;
        filters.WrapContents = true;
        lblFrom.Text = "Từ ngày";
        lblFrom.AutoSize = true;
        lblFrom.Padding = new Padding(0, 6, 0, 0);
        from.Name = "from";
        from.Format = DateTimePickerFormat.Short;
        from.Width = 145;
        lblTo.Text = "Đến ngày";
        lblTo.AutoSize = true;
        lblTo.Padding = new Padding(8, 6, 0, 0);
        to.Name = "to";
        to.Format = DateTimePickerFormat.Short;
        to.Width = 145;
        period.Name = "period";
        period.DropDownStyle = ComboBoxStyle.DropDownList;
        period.Width = 130;
        period.Items.AddRange(new object[] { "Theo tháng", "Theo quý" });
        btnLoad.Name = "btnLoad";
        btnLoad.Text = "Thống kê";
        btnLoad.Size = new Size(110, 36);
        btnPrint.Name = "btnPrint";
        btnPrint.Text = "Xem / In";
        btnPrint.Size = new Size(100, 36);
        btnCsv.Name = "btnCsv";
        btnCsv.Text = "Xuất CSV";
        btnCsv.Size = new Size(110, 36);
        filters.Controls.Add(lblFrom);
        filters.Controls.Add(from);
        filters.Controls.Add(lblTo);
        filters.Controls.Add(to);
        filters.Controls.Add(period);
        filters.Controls.Add(btnLoad);
        filters.Controls.Add(btnPrint);
        filters.Controls.Add(btnCsv);
        tabs.Name = "tabs";
        tabs.Dock = DockStyle.Fill;
        revenueTab.Name = "revenueTab";
        revenueTab.Text = "Doanh thu thực thu";
        bestTab.Name = "bestTab";
        bestTab.Text = "Món / Đồ uống bán chạy";
        gridRevenue.Name = "gridRevenue";
        gridRevenue.Dock = DockStyle.Fill;
        gridBest.Name = "gridBest";
        gridBest.Dock = DockStyle.Fill;
        revenueTab.Controls.Add(gridRevenue);
        bestTab.Controls.Add(gridBest);
        tabs.Controls.Add(revenueTab);
        tabs.Controls.Add(bestTab);
        summary.Name = "summary";
        summary.Dock = DockStyle.Bottom;
        summary.Height = 70;
        summary.TextAlign = ContentAlignment.MiddleLeft;
        Controls.Add(tabs);
        Controls.Add(summary);
        Controls.Add(filters);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        MinimumSize = new Size(900, 650);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(255, 247, 237);
        Padding = new Padding(20);
        StartPosition = FormStartPosition.CenterParent;
        Name = "frmReports";
        Text = "frmReports";
        ResumeLayout(false);
    }
}
