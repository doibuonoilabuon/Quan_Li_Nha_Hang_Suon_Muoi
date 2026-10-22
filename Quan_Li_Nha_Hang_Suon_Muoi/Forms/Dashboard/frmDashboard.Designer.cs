using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Dashboard;
partial class frmDashboard
{
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private TableLayoutPanel layout;
    private Panel heading;
    private Label lblWelcome;
    private Label lblSubtitle;
    private Button btnRefresh;
    private TableLayoutPanel cards;
    private Panel cardRevenue;
    private Panel accentRevenue;
    private Label titleRevenue;
    private Label valueRevenue;
    private Label noteRevenue;
    private Panel cardOrders;
    private Panel accentOrders;
    private Label titleOrders;
    private Label valueOrders;
    private Label noteOrders;
    private Panel cardNew;
    private Panel accentNew;
    private Label titleNew;
    private Label valueNew;
    private Label noteNew;
    private Panel cardCustomers;
    private Panel accentCustomers;
    private Label titleCustomers;
    private Label valueCustomers;
    private Label noteCustomers;
    private TableLayoutPanel statuses;
    private Panel statusPending;
    private Panel stripePending;
    private Label statusTitlePending;
    private Label statusValuePending;
    private Panel statusPreparing;
    private Panel stripePreparing;
    private Label statusTitlePreparing;
    private Label statusValuePreparing;
    private Panel statusDelivery;
    private Panel stripeDelivery;
    private Label statusTitleDelivery;
    private Label statusValueDelivery;
    private TableLayoutPanel body;
    private Panel chartPanel;
    private Panel ordersPanel;
    private Panel chartHeader;
    private Label lblRevenue;
    private Label lblRevenueUnit;
    private ComboBox cboPeriod;
    private Quan_Li_Nha_Hang_Suon_Muoi.Controls.ucRevenueChart revenueChart;
    private Panel ordersHeader;
    private Label lblRecent;
    private Label lblRecentNote;
    private Button btnViewAll;
    private DataGridView gridRecent;
    private DataGridViewTextBoxColumn colCode;
    private DataGridViewTextBoxColumn colCustomer;
    private DataGridViewTextBoxColumn colAmount;
    private DataGridViewTextBoxColumn colStatus;
    private Label lblDemo;
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle amountStyle = new DataGridViewCellStyle();
        layout = new TableLayoutPanel();
        heading = new Panel();
        lblWelcome = new Label();
        lblSubtitle = new Label();
        btnRefresh = new Button();
        cards = new TableLayoutPanel();
        cardRevenue = new Panel();
        accentRevenue = new Panel();
        titleRevenue = new Label();
        valueRevenue = new Label();
        noteRevenue = new Label();
        cardOrders = new Panel();
        accentOrders = new Panel();
        titleOrders = new Label();
        valueOrders = new Label();
        noteOrders = new Label();
        cardNew = new Panel();
        accentNew = new Panel();
        titleNew = new Label();
        valueNew = new Label();
        noteNew = new Label();
        cardCustomers = new Panel();
        accentCustomers = new Panel();
        titleCustomers = new Label();
        valueCustomers = new Label();
        noteCustomers = new Label();
        statuses = new TableLayoutPanel();
        statusPending = new Panel();
        stripePending = new Panel();
        statusTitlePending = new Label();
        statusValuePending = new Label();
        statusPreparing = new Panel();
        stripePreparing = new Panel();
        statusTitlePreparing = new Label();
        statusValuePreparing = new Label();
        statusDelivery = new Panel();
        stripeDelivery = new Panel();
        statusTitleDelivery = new Label();
        statusValueDelivery = new Label();
        body = new TableLayoutPanel();
        chartPanel = new Panel();
        revenueChart = new Quan_Li_Nha_Hang_Suon_Muoi.Controls.ucRevenueChart();
        chartHeader = new Panel();
        lblRevenue = new Label();
        lblRevenueUnit = new Label();
        cboPeriod = new ComboBox();
        ordersPanel = new Panel();
        gridRecent = new DataGridView();
        colCode = new DataGridViewTextBoxColumn();
        colCustomer = new DataGridViewTextBoxColumn();
        colAmount = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        ordersHeader = new Panel();
        lblRecent = new Label();
        lblRecentNote = new Label();
        btnViewAll = new Button();
        lblDemo = new Label();
        layout.SuspendLayout();
        heading.SuspendLayout();
        cards.SuspendLayout();
        cardRevenue.SuspendLayout();
        cardOrders.SuspendLayout();
        cardNew.SuspendLayout();
        cardCustomers.SuspendLayout();
        statuses.SuspendLayout();
        statusPending.SuspendLayout();
        statusPreparing.SuspendLayout();
        statusDelivery.SuspendLayout();
        body.SuspendLayout();
        chartPanel.SuspendLayout();
        chartHeader.SuspendLayout();
        ordersPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridRecent).BeginInit();
        ordersHeader.SuspendLayout();
        SuspendLayout();
        //
        // layout
        //
        layout.ColumnCount = 1;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.Controls.Add(heading, 0, 0);
        layout.Controls.Add(cards, 0, 1);
        layout.Controls.Add(statuses, 0, 2);
        layout.Controls.Add(body, 0, 3);
        layout.Controls.Add(lblDemo, 0, 4);
        layout.Dock = DockStyle.Fill;
        layout.Location = new Point(25, 24);
        layout.Margin = new Padding(0);
        layout.Name = "layout";
        layout.RowCount = 5;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 176F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 83F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        layout.Size = new Size(1853, 1031);
        layout.TabIndex = 0;
        //
        // heading
        //
        heading.Controls.Add(lblWelcome);
        heading.Controls.Add(lblSubtitle);
        heading.Controls.Add(btnRefresh);
        heading.Dock = DockStyle.Fill;
        heading.Location = new Point(0, 0);
        heading.Margin = new Padding(0);
        heading.Name = "heading";
        heading.Size = new Size(1853, 72);
        heading.TabIndex = 0;
        //
        // lblWelcome
        //
        lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblWelcome.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblWelcome.ForeColor = Color.FromArgb(36, 32, 29);
        lblWelcome.Location = new Point(7, 0);
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Size = new Size(1624, 47);
        lblWelcome.TabIndex = 0;
        lblWelcome.Text = "Chào mừng đến với SƯỜN MƯỜI";
        //
        // lblSubtitle
        //
        lblSubtitle.Font = new Font("Segoe UI", 10F);
        lblSubtitle.ForeColor = Color.FromArgb(130, 118, 109);
        lblSubtitle.Location = new Point(9, 45);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(1029, 27);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Theo dõi nhanh hoạt động kinh doanh và đơn hàng của nhà hàng.";
        //
        // btnRefresh
        //
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(231, 220, 207);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnRefresh.Location = new Point(1703, 5);
        btnRefresh.Margin = new Padding(3, 4, 3, 4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(128, 48);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "↻  Làm mới";
        btnRefresh.UseVisualStyleBackColor = false;
        //
        // cards
        //
        cards.ColumnCount = 4;
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        cards.Controls.Add(cardRevenue, 0, 0);
        cards.Controls.Add(cardOrders, 1, 0);
        cards.Controls.Add(cardNew, 2, 0);
        cards.Controls.Add(cardCustomers, 3, 0);
        cards.Dock = DockStyle.Fill;
        cards.Location = new Point(0, 72);
        cards.Margin = new Padding(0);
        cards.Name = "cards";
        cards.RowCount = 1;
        cards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        cards.Size = new Size(1853, 176);
        cards.TabIndex = 1;
        //
        // cardRevenue
        //
        cardRevenue.BackColor = Color.White;
        cardRevenue.BorderStyle = BorderStyle.FixedSingle;
        cardRevenue.Controls.Add(accentRevenue);
        cardRevenue.Controls.Add(titleRevenue);
        cardRevenue.Controls.Add(valueRevenue);
        cardRevenue.Controls.Add(noteRevenue);
        cardRevenue.Dock = DockStyle.Fill;
        cardRevenue.Location = new Point(0, 0);
        cardRevenue.Margin = new Padding(0, 0, 16, 11);
        cardRevenue.Name = "cardRevenue";
        cardRevenue.Size = new Size(447, 165);
        cardRevenue.TabIndex = 0;
        //
        // accentRevenue
        //
        accentRevenue.BackColor = Color.FromArgb(168, 63, 43);
        accentRevenue.Location = new Point(19, 17);
        accentRevenue.Margin = new Padding(3, 4, 3, 4);
        accentRevenue.Name = "accentRevenue";
        accentRevenue.Size = new Size(5, 132);
        accentRevenue.TabIndex = 0;
        //
        // titleRevenue
        //
        titleRevenue.Font = new Font("Segoe UI", 10F);
        titleRevenue.ForeColor = Color.FromArgb(130, 118, 109);
        titleRevenue.Location = new Point(25, 16);
        titleRevenue.Name = "titleRevenue";
        titleRevenue.Size = new Size(354, 35);
        titleRevenue.TabIndex = 1;
        titleRevenue.Text = "DOANH THU HÔM NAY";
        //
        // valueRevenue
        //
        valueRevenue.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        valueRevenue.ForeColor = Color.FromArgb(36, 32, 29);
        valueRevenue.Location = new Point(26, 53);
        valueRevenue.Name = "valueRevenue";
        valueRevenue.Size = new Size(383, 60);
        valueRevenue.TabIndex = 2;
        valueRevenue.Text = "8.450.000 đ";
        //
        // noteRevenue
        //
        noteRevenue.Font = new Font("Segoe UI", 9F);
        noteRevenue.ForeColor = Color.FromArgb(130, 118, 109);
        noteRevenue.Location = new Point(24, 112);
        noteRevenue.Name = "noteRevenue";
        noteRevenue.Size = new Size(377, 32);
        noteRevenue.TabIndex = 3;
        noteRevenue.Text = "+12% so với hôm qua";
        //
        // cardOrders
        //
        cardOrders.BackColor = Color.White;
        cardOrders.BorderStyle = BorderStyle.FixedSingle;
        cardOrders.Controls.Add(accentOrders);
        cardOrders.Controls.Add(titleOrders);
        cardOrders.Controls.Add(valueOrders);
        cardOrders.Controls.Add(noteOrders);
        cardOrders.Dock = DockStyle.Fill;
        cardOrders.Location = new Point(463, 0);
        cardOrders.Margin = new Padding(0, 0, 16, 11);
        cardOrders.Name = "cardOrders";
        cardOrders.Size = new Size(447, 165);
        cardOrders.TabIndex = 1;
        //
        // accentOrders
        //
        accentOrders.BackColor = Color.FromArgb(204, 130, 40);
        accentOrders.Location = new Point(19, 17);
        accentOrders.Margin = new Padding(3, 4, 3, 4);
        accentOrders.Name = "accentOrders";
        accentOrders.Size = new Size(5, 132);
        accentOrders.TabIndex = 0;
        //
        // titleOrders
        //
        titleOrders.Font = new Font("Segoe UI", 10F);
        titleOrders.ForeColor = Color.FromArgb(130, 118, 109);
        titleOrders.Location = new Point(25, 16);
        titleOrders.Name = "titleOrders";
        titleOrders.Size = new Size(354, 35);
        titleOrders.TabIndex = 1;
        titleOrders.Text = "TỔNG ĐƠN HÀNG";
        //
        // valueOrders
        //
        valueOrders.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        valueOrders.ForeColor = Color.FromArgb(36, 32, 29);
        valueOrders.Location = new Point(26, 53);
        valueOrders.Name = "valueOrders";
        valueOrders.Size = new Size(383, 60);
        valueOrders.TabIndex = 2;
        valueOrders.Text = "128";
        //
        // noteOrders
        //
        noteOrders.Font = new Font("Segoe UI", 9F);
        noteOrders.ForeColor = Color.FromArgb(130, 118, 109);
        noteOrders.Location = new Point(24, 112);
        noteOrders.Name = "noteOrders";
        noteOrders.Size = new Size(377, 32);
        noteOrders.TabIndex = 3;
        noteOrders.Text = "Trong tháng này";
        //
        // cardNew
        //
        cardNew.BackColor = Color.White;
        cardNew.BorderStyle = BorderStyle.FixedSingle;
        cardNew.Controls.Add(accentNew);
        cardNew.Controls.Add(titleNew);
        cardNew.Controls.Add(valueNew);
        cardNew.Controls.Add(noteNew);
        cardNew.Dock = DockStyle.Fill;
        cardNew.Location = new Point(926, 0);
        cardNew.Margin = new Padding(0, 0, 16, 11);
        cardNew.Name = "cardNew";
        cardNew.Size = new Size(447, 165);
        cardNew.TabIndex = 2;
        //
        // accentNew
        //
        accentNew.BackColor = Color.FromArgb(40, 118, 76);
        accentNew.Location = new Point(19, 17);
        accentNew.Margin = new Padding(3, 4, 3, 4);
        accentNew.Name = "accentNew";
        accentNew.Size = new Size(5, 132);
        accentNew.TabIndex = 0;
        //
        // titleNew
        //
        titleNew.Font = new Font("Segoe UI", 10F);
        titleNew.ForeColor = Color.FromArgb(130, 118, 109);
        titleNew.Location = new Point(25, 16);
        titleNew.Name = "titleNew";
        titleNew.Size = new Size(354, 35);
        titleNew.TabIndex = 1;
        titleNew.Text = "ĐƠN HÀNG MỚI";
        //
        // valueNew
        //
        valueNew.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        valueNew.ForeColor = Color.FromArgb(36, 32, 29);
        valueNew.Location = new Point(26, 53);
        valueNew.Name = "valueNew";
        valueNew.Size = new Size(383, 60);
        valueNew.TabIndex = 2;
        valueNew.Text = "12";
        //
        // noteNew
        //
        noteNew.Font = new Font("Segoe UI", 9F);
        noteNew.ForeColor = Color.FromArgb(130, 118, 109);
        noteNew.Location = new Point(24, 112);
        noteNew.Name = "noteNew";
        noteNew.Size = new Size(377, 32);
        noteNew.TabIndex = 3;
        noteNew.Text = "Cần được kiểm tra";
        //
        // cardCustomers
        //
        cardCustomers.BackColor = Color.White;
        cardCustomers.BorderStyle = BorderStyle.FixedSingle;
        cardCustomers.Controls.Add(accentCustomers);
        cardCustomers.Controls.Add(titleCustomers);
        cardCustomers.Controls.Add(valueCustomers);
        cardCustomers.Controls.Add(noteCustomers);
        cardCustomers.Dock = DockStyle.Fill;
        cardCustomers.Location = new Point(1389, 0);
        cardCustomers.Margin = new Padding(0, 0, 0, 11);
        cardCustomers.Name = "cardCustomers";
        cardCustomers.Size = new Size(464, 165);
        cardCustomers.TabIndex = 3;
        //
        // accentCustomers
        //
        accentCustomers.BackColor = Color.FromArgb(82, 126, 163);
        accentCustomers.Location = new Point(19, 17);
        accentCustomers.Margin = new Padding(3, 4, 3, 4);
        accentCustomers.Name = "accentCustomers";
        accentCustomers.Size = new Size(5, 132);
        accentCustomers.TabIndex = 0;
        //
        // titleCustomers
        //
        titleCustomers.Font = new Font("Segoe UI", 10F);
        titleCustomers.ForeColor = Color.FromArgb(130, 118, 109);
        titleCustomers.Location = new Point(25, 16);
        titleCustomers.Name = "titleCustomers";
        titleCustomers.Size = new Size(354, 35);
        titleCustomers.TabIndex = 1;
        titleCustomers.Text = "KHÁCH HÀNG";
        //
        // valueCustomers
        //
        valueCustomers.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        valueCustomers.ForeColor = Color.FromArgb(36, 32, 29);
        valueCustomers.Location = new Point(26, 53);
        valueCustomers.Name = "valueCustomers";
        valueCustomers.Size = new Size(383, 60);
        valueCustomers.TabIndex = 2;
        valueCustomers.Text = "864";
        //
        // noteCustomers
        //
        noteCustomers.Font = new Font("Segoe UI", 9F);
        noteCustomers.ForeColor = Color.FromArgb(130, 118, 109);
        noteCustomers.Location = new Point(24, 112);
        noteCustomers.Name = "noteCustomers";
        noteCustomers.Size = new Size(377, 32);
        noteCustomers.TabIndex = 3;
        noteCustomers.Text = "Tài khoản demo";
        //
        // statuses
        //
        statuses.ColumnCount = 3;
        statuses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        statuses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
        statuses.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33334F));
        statuses.Controls.Add(statusPending, 0, 0);
        statuses.Controls.Add(statusPreparing, 1, 0);
        statuses.Controls.Add(statusDelivery, 2, 0);
        statuses.Dock = DockStyle.Fill;
        statuses.Location = new Point(0, 248);
        statuses.Margin = new Padding(0);
        statuses.Name = "statuses";
        statuses.RowCount = 1;
        statuses.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        statuses.Size = new Size(1853, 83);
        statuses.TabIndex = 2;
        //
        // statusPending
        //
        statusPending.BackColor = Color.White;
        statusPending.BorderStyle = BorderStyle.FixedSingle;
        statusPending.Controls.Add(stripePending);
        statusPending.Controls.Add(statusTitlePending);
        statusPending.Controls.Add(statusValuePending);
        statusPending.Dock = DockStyle.Fill;
        statusPending.Location = new Point(0, 0);
        statusPending.Margin = new Padding(0, 0, 11, 16);
        statusPending.Name = "statusPending";
        statusPending.Size = new Size(606, 67);
        statusPending.TabIndex = 0;
        statusPending.Paint += statusPending_Paint;
        //
        // stripePending
        //
        stripePending.BackColor = Color.FromArgb(185, 56, 34);
        stripePending.Dock = DockStyle.Left;
        stripePending.Location = new Point(0, 0);
        stripePending.Margin = new Padding(3, 4, 3, 4);
        stripePending.Name = "stripePending";
        stripePending.Size = new Size(5, 65);
        stripePending.TabIndex = 0;
        //
        // statusTitlePending
        //
        statusTitlePending.Font = new Font("Segoe UI", 10F);
        statusTitlePending.ForeColor = Color.FromArgb(130, 118, 109);
        statusTitlePending.Location = new Point(21, 9);
        statusTitlePending.Name = "statusTitlePending";
        statusTitlePending.Size = new Size(343, 29);
        statusTitlePending.TabIndex = 1;
        statusTitlePending.Text = "Chờ xác nhận";
        //
        // statusValuePending
        //
        statusValuePending.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        statusValuePending.ForeColor = Color.FromArgb(185, 56, 34);
        statusValuePending.Location = new Point(21, 35);
        statusValuePending.Name = "statusValuePending";
        statusValuePending.Size = new Size(343, 33);
        statusValuePending.TabIndex = 2;
        statusValuePending.Text = "12 đơn";
        //
        // statusPreparing
        //
        statusPreparing.BackColor = Color.White;
        statusPreparing.BorderStyle = BorderStyle.FixedSingle;
        statusPreparing.Controls.Add(stripePreparing);
        statusPreparing.Controls.Add(statusTitlePreparing);
        statusPreparing.Controls.Add(statusValuePreparing);
        statusPreparing.Dock = DockStyle.Fill;
        statusPreparing.Location = new Point(617, 0);
        statusPreparing.Margin = new Padding(0, 0, 11, 16);
        statusPreparing.Name = "statusPreparing";
        statusPreparing.Size = new Size(606, 67);
        statusPreparing.TabIndex = 1;
        //
        // stripePreparing
        //
        stripePreparing.BackColor = Color.FromArgb(204, 130, 40);
        stripePreparing.Dock = DockStyle.Left;
        stripePreparing.Location = new Point(0, 0);
        stripePreparing.Margin = new Padding(3, 4, 3, 4);
        stripePreparing.Name = "stripePreparing";
        stripePreparing.Size = new Size(5, 65);
        stripePreparing.TabIndex = 0;
        //
        // statusTitlePreparing
        //
        statusTitlePreparing.Font = new Font("Segoe UI", 10F);
        statusTitlePreparing.ForeColor = Color.FromArgb(130, 118, 109);
        statusTitlePreparing.Location = new Point(21, 9);
        statusTitlePreparing.Name = "statusTitlePreparing";
        statusTitlePreparing.Size = new Size(343, 29);
        statusTitlePreparing.TabIndex = 1;
        statusTitlePreparing.Text = "Đang chuẩn bị";
        //
        // statusValuePreparing
        //
        statusValuePreparing.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        statusValuePreparing.ForeColor = Color.FromArgb(204, 130, 40);
        statusValuePreparing.Location = new Point(21, 35);
        statusValuePreparing.Name = "statusValuePreparing";
        statusValuePreparing.Size = new Size(343, 33);
        statusValuePreparing.TabIndex = 2;
        statusValuePreparing.Text = "08 đơn";
        //
        // statusDelivery
        //
        statusDelivery.BackColor = Color.White;
        statusDelivery.BorderStyle = BorderStyle.FixedSingle;
        statusDelivery.Controls.Add(stripeDelivery);
        statusDelivery.Controls.Add(statusTitleDelivery);
        statusDelivery.Controls.Add(statusValueDelivery);
        statusDelivery.Dock = DockStyle.Fill;
        statusDelivery.Location = new Point(1234, 0);
        statusDelivery.Margin = new Padding(0, 0, 11, 16);
        statusDelivery.Name = "statusDelivery";
        statusDelivery.Size = new Size(608, 67);
        statusDelivery.TabIndex = 2;
        //
        // stripeDelivery
        //
        stripeDelivery.BackColor = Color.FromArgb(82, 126, 163);
        stripeDelivery.Dock = DockStyle.Left;
        stripeDelivery.Location = new Point(0, 0);
        stripeDelivery.Margin = new Padding(3, 4, 3, 4);
        stripeDelivery.Name = "stripeDelivery";
        stripeDelivery.Size = new Size(5, 65);
        stripeDelivery.TabIndex = 0;
        //
        // statusTitleDelivery
        //
        statusTitleDelivery.Font = new Font("Segoe UI", 10F);
        statusTitleDelivery.ForeColor = Color.FromArgb(130, 118, 109);
        statusTitleDelivery.Location = new Point(21, 9);
        statusTitleDelivery.Name = "statusTitleDelivery";
        statusTitleDelivery.Size = new Size(343, 29);
        statusTitleDelivery.TabIndex = 1;
        statusTitleDelivery.Text = "Đang giao hàng";
        //
        // statusValueDelivery
        //
        statusValueDelivery.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        statusValueDelivery.ForeColor = Color.FromArgb(82, 126, 163);
        statusValueDelivery.Location = new Point(21, 35);
        statusValueDelivery.Name = "statusValueDelivery";
        statusValueDelivery.Size = new Size(343, 33);
        statusValueDelivery.TabIndex = 2;
        statusValueDelivery.Text = "05 đơn";
        //
        // body
        //
        body.ColumnCount = 2;
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
        body.Controls.Add(chartPanel, 0, 0);
        body.Controls.Add(ordersPanel, 1, 0);
        body.Dock = DockStyle.Fill;
        body.Location = new Point(0, 331);
        body.Margin = new Padding(0);
        body.Name = "body";
        body.RowCount = 1;
        body.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        body.Size = new Size(1853, 665);
        body.TabIndex = 3;
        //
        // chartPanel
        //
        chartPanel.BackColor = Color.White;
        chartPanel.BorderStyle = BorderStyle.FixedSingle;
        chartPanel.Controls.Add(revenueChart);
        chartPanel.Controls.Add(chartHeader);
        chartPanel.Dock = DockStyle.Fill;
        chartPanel.Location = new Point(0, 0);
        chartPanel.Margin = new Padding(0, 0, 14, 0);
        chartPanel.Name = "chartPanel";
        chartPanel.Padding = new Padding(16, 0, 16, 16);
        chartPanel.Size = new Size(1042, 665);
        chartPanel.TabIndex = 0;
        //
        // revenueChart
        //
        revenueChart.BackColor = Color.White;
        revenueChart.Dock = DockStyle.Fill;
        revenueChart.Font = new Font("Segoe UI", 9F);
        revenueChart.ForeColor = Color.FromArgb(36, 32, 29);
        revenueChart.Location = new Point(16, 96);
        revenueChart.Margin = new Padding(0);
        revenueChart.MinimumSize = new Size(297, 227);
        revenueChart.Name = "revenueChart";
        revenueChart.Size = new Size(1008, 551);
        revenueChart.TabIndex = 0;
        //
        // chartHeader
        //
        chartHeader.Controls.Add(lblRevenue);
        chartHeader.Controls.Add(lblRevenueUnit);
        chartHeader.Controls.Add(cboPeriod);
        chartHeader.Dock = DockStyle.Top;
        chartHeader.Location = new Point(16, 0);
        chartHeader.Margin = new Padding(3, 4, 3, 4);
        chartHeader.Name = "chartHeader";
        chartHeader.Size = new Size(1008, 96);
        chartHeader.TabIndex = 1;
        //
        // lblRevenue
        //
        lblRevenue.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblRevenue.ForeColor = Color.FromArgb(36, 32, 29);
        lblRevenue.Location = new Point(24, 21);
        lblRevenue.Name = "lblRevenue";
        lblRevenue.Size = new Size(286, 37);
        lblRevenue.TabIndex = 0;
        lblRevenue.Text = "Doanh thu";
        //
        // lblRevenueUnit
        //
        lblRevenueUnit.Font = new Font("Segoe UI", 9F);
        lblRevenueUnit.ForeColor = Color.FromArgb(130, 118, 109);
        lblRevenueUnit.Location = new Point(24, 59);
        lblRevenueUnit.Name = "lblRevenueUnit";
        lblRevenueUnit.Size = new Size(571, 29);
        lblRevenueUnit.TabIndex = 1;
        lblRevenueUnit.Text = "Đơn vị: triệu đồng • Dữ liệu minh họa";
        //
        // cboPeriod
        //
        cboPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cboPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPeriod.Font = new Font("Segoe UI", 9F);
        cboPeriod.Items.AddRange(new object[] { "7 ngày qua", "Tháng này" });
        cboPeriod.Location = new Point(865, 23);
        cboPeriod.Margin = new Padding(3, 4, 3, 4);
        cboPeriod.Name = "cboPeriod";
        cboPeriod.Size = new Size(127, 28);
        cboPeriod.TabIndex = 2;
        //
        // ordersPanel
        //
        ordersPanel.BackColor = Color.White;
        ordersPanel.BorderStyle = BorderStyle.FixedSingle;
        ordersPanel.Controls.Add(gridRecent);
        ordersPanel.Controls.Add(ordersHeader);
        ordersPanel.Dock = DockStyle.Fill;
        ordersPanel.Location = new Point(1056, 0);
        ordersPanel.Margin = new Padding(0);
        ordersPanel.Name = "ordersPanel";
        ordersPanel.Padding = new Padding(15, 0, 15, 16);
        ordersPanel.Size = new Size(797, 665);
        ordersPanel.TabIndex = 1;
        //
        // gridRecent
        //
        colCode.Name = "colCode";
        colCode.DataPropertyName = "OrderCode";
        colCode.HeaderText = "Mã đơn";
        colCode.FillWeight = 19F;
        colCode.ReadOnly = true;
        colCode.SortMode = DataGridViewColumnSortMode.NotSortable;
        colCustomer.Name = "colCustomer";
        colCustomer.DataPropertyName = "CustomerName";
        colCustomer.HeaderText = "Khách hàng";
        colCustomer.FillWeight = 30F;
        colCustomer.ReadOnly = true;
        colCustomer.SortMode = DataGridViewColumnSortMode.NotSortable;
        colAmount.Name = "colAmount";
        colAmount.DataPropertyName = "TotalAmount";
        colAmount.HeaderText = "Tổng tiền";
        colAmount.FillWeight = 23F;
        colAmount.ReadOnly = true;
        colAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
        amountStyle.Format = "#,##0 'đ'";
        amountStyle.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
        colAmount.DefaultCellStyle = amountStyle;
        colStatus.Name = "colStatus";
        colStatus.DataPropertyName = "Status";
        colStatus.HeaderText = "Trạng thái";
        colStatus.FillWeight = 28F;
        colStatus.ReadOnly = true;
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        gridRecent.AutoGenerateColumns = false;
        gridRecent.Columns.AddRange(colCode, colCustomer, colAmount, colStatus);
        gridRecent.AllowUserToAddRows = false;
        gridRecent.AllowUserToDeleteRows = false;
        gridRecent.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(255, 252, 248);
        gridRecent.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        gridRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridRecent.BackgroundColor = Color.White;
        gridRecent.BorderStyle = BorderStyle.None;
        gridRecent.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(245, 235, 221);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(36, 32, 29);
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        gridRecent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        gridRecent.ColumnHeadersHeight = 36;
        gridRecent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Window;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(36, 32, 29);
        dataGridViewCellStyle3.Padding = new Padding(4, 0, 0, 0);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(243, 221, 212);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(36, 32, 29);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        gridRecent.DefaultCellStyle = dataGridViewCellStyle3;
        gridRecent.Dock = DockStyle.Fill;
        gridRecent.EnableHeadersVisualStyles = false;
        gridRecent.Font = new Font("Segoe UI", 9F);
        gridRecent.GridColor = Color.FromArgb(231, 220, 207);
        gridRecent.Location = new Point(15, 96);
        gridRecent.Margin = new Padding(3, 4, 3, 4);
        gridRecent.MultiSelect = false;
        gridRecent.Name = "gridRecent";
        gridRecent.ReadOnly = true;
        gridRecent.RowHeadersVisible = false;
        gridRecent.RowHeadersWidth = 51;
        gridRecent.RowTemplate.Height = 34;
        gridRecent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridRecent.Size = new Size(765, 551);
        gridRecent.TabIndex = 0;
        //
        // ordersHeader
        //
        ordersHeader.Controls.Add(lblRecent);
        ordersHeader.Controls.Add(lblRecentNote);
        ordersHeader.Controls.Add(btnViewAll);
        ordersHeader.Dock = DockStyle.Top;
        ordersHeader.Location = new Point(15, 0);
        ordersHeader.Margin = new Padding(3, 4, 3, 4);
        ordersHeader.Name = "ordersHeader";
        ordersHeader.Size = new Size(765, 96);
        ordersHeader.TabIndex = 1;
        //
        // lblRecent
        //
        lblRecent.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblRecent.ForeColor = Color.FromArgb(36, 32, 29);
        lblRecent.Location = new Point(8, 21);
        lblRecent.Name = "lblRecent";
        lblRecent.Size = new Size(480, 37);
        lblRecent.TabIndex = 0;
        lblRecent.Text = "Đơn hàng gần đây";
        //
        // lblRecentNote
        //
        lblRecentNote.Font = new Font("Segoe UI", 9F);
        lblRecentNote.ForeColor = Color.FromArgb(130, 118, 109);
        lblRecentNote.Location = new Point(8, 59);
        lblRecentNote.Name = "lblRecentNote";
        lblRecentNote.Size = new Size(480, 29);
        lblRecentNote.TabIndex = 1;
        lblRecentNote.Text = "Theo dõi các đơn mới nhất";
        //
        // btnViewAll
        //
        btnViewAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnViewAll.BackColor = Color.White;
        btnViewAll.FlatAppearance.BorderColor = Color.FromArgb(231, 220, 207);
        btnViewAll.FlatStyle = FlatStyle.Flat;
        btnViewAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnViewAll.Location = new Point(643, 19);
        btnViewAll.Margin = new Padding(3, 4, 3, 4);
        btnViewAll.Name = "btnViewAll";
        btnViewAll.Size = new Size(107, 45);
        btnViewAll.TabIndex = 2;
        btnViewAll.Text = "Xem tất cả";
        btnViewAll.UseVisualStyleBackColor = false;
        //
        // lblDemo
        //
        lblDemo.Dock = DockStyle.Fill;
        lblDemo.Font = new Font("Segoe UI", 9F);
        lblDemo.ForeColor = Color.FromArgb(130, 118, 109);
        lblDemo.Location = new Point(5, 1007);
        lblDemo.Margin = new Padding(5, 11, 0, 0);
        lblDemo.Name = "lblDemo";
        lblDemo.Size = new Size(1848, 24);
        lblDemo.TabIndex = 4;
        lblDemo.Text = "SQL SERVER  •  Xem trước giao diện; số liệu thực được tải khi chạy.";
        //
        // frmDashboard
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(1903, 1055);
        Controls.Add(layout);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(36, 32, 29);
        Margin = new Padding(3, 4, 3, 4);
        Name = "frmDashboard";
        Padding = new Padding(25, 24, 25, 0);
        Text = "Tổng quan";
        layout.ResumeLayout(false);
        heading.ResumeLayout(false);
        cards.ResumeLayout(false);
        cardRevenue.ResumeLayout(false);
        cardOrders.ResumeLayout(false);
        cardNew.ResumeLayout(false);
        cardCustomers.ResumeLayout(false);
        statuses.ResumeLayout(false);
        statusPending.ResumeLayout(false);
        statusPreparing.ResumeLayout(false);
        statusDelivery.ResumeLayout(false);
        body.ResumeLayout(false);
        chartPanel.ResumeLayout(false);
        chartHeader.ResumeLayout(false);
        ordersPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridRecent).EndInit();
        ordersHeader.ResumeLayout(false);
        ResumeLayout(false);
    }
}
