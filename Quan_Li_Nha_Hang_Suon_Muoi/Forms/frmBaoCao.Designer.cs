namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;
partial class frmBaoCao
{
    private FlowLayoutPanel boLoc = null!;
    private Label lblTu = null!;
    private DateTimePicker dtTu = null!;
    private Label lblDen = null!;
    private DateTimePicker dtDen = null!;
    private ComboBox cboKy = null!;
    private Button btnXem = null!;
    private Button btnExcel = null!;
    private Button btnIn = null!;
    private TabControl tabs = null!;
    private TabPage tabDoanhThu = null!;
    private TabPage tabMon = null!;
    private DataGridView gridDoanhThu = null!;
    private DataGridView gridMon = null!;
    private Label lblTong = null!;
    private void InitializeComponent()
    {
        boLoc = new FlowLayoutPanel();
        lblTu = new Label();
        dtTu = new DateTimePicker();
        lblDen = new Label();
        dtDen = new DateTimePicker();
        cboKy = new ComboBox();
        btnXem = new Button();
        btnExcel = new Button();
        btnIn = new Button();
        tabs = new TabControl();
        tabDoanhThu = new TabPage();
        gridDoanhThu = new DataGridView();
        tabMon = new TabPage();
        gridMon = new DataGridView();
        lblTong = new Label();
        boLoc.SuspendLayout();
        tabs.SuspendLayout();
        tabDoanhThu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridDoanhThu).BeginInit();
        tabMon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridMon).BeginInit();
        SuspendLayout();
        // 
        // boLoc
        // 
        boLoc.Controls.Add(lblTu);
        boLoc.Controls.Add(dtTu);
        boLoc.Controls.Add(lblDen);
        boLoc.Controls.Add(dtDen);
        boLoc.Controls.Add(cboKy);
        boLoc.Controls.Add(btnXem);
        boLoc.Controls.Add(btnExcel);
        boLoc.Controls.Add(btnIn);
        boLoc.Dock = DockStyle.Top;
        boLoc.Location = new Point(0, 0);
        boLoc.Name = "boLoc";
        boLoc.Padding = new Padding(12);
        boLoc.Size = new Size(1100, 105);
        boLoc.TabIndex = 2;
        // 
        // lblTu
        // 
        lblTu.AutoSize = true;
        lblTu.Location = new Point(15, 12);
        lblTu.Name = "lblTu";
        lblTu.Padding = new Padding(0, 8, 0, 0);
        lblTu.Size = new Size(29, 31);
        lblTu.TabIndex = 0;
        lblTu.Text = "Từ";
        // 
        // dtTu
        // 
        dtTu.Format = DateTimePickerFormat.Short;
        dtTu.Location = new Point(50, 15);
        dtTu.Name = "dtTu";
        dtTu.Size = new Size(135, 30);
        dtTu.TabIndex = 1;
        // 
        // lblDen
        // 
        lblDen.AutoSize = true;
        lblDen.Location = new Point(191, 12);
        lblDen.Name = "lblDen";
        lblDen.Padding = new Padding(8, 8, 0, 0);
        lblDen.Size = new Size(49, 31);
        lblDen.TabIndex = 2;
        lblDen.Text = "Đến";
        // 
        // dtDen
        // 
        dtDen.Format = DateTimePickerFormat.Short;
        dtDen.Location = new Point(246, 15);
        dtDen.Name = "dtDen";
        dtDen.Size = new Size(135, 30);
        dtDen.TabIndex = 3;
        // 
        // cboKy
        // 
        cboKy.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKy.Location = new Point(387, 15);
        cboKy.Name = "cboKy";
        cboKy.Size = new Size(145, 31);
        cboKy.TabIndex = 4;
        // 
        // btnXem
        // 
        btnXem.Location = new Point(538, 15);
        btnXem.Name = "btnXem";
        btnXem.Size = new Size(130, 34);
        btnXem.TabIndex = 5;
        btnXem.Text = "Xem báo cáo";
        // 
        // btnExcel
        // 
        btnExcel.Enabled = false;
        btnExcel.Location = new Point(674, 15);
        btnExcel.Name = "btnExcel";
        btnExcel.Size = new Size(120, 34);
        btnExcel.TabIndex = 6;
        btnExcel.Text = "Xuất Excel";
        btnExcel.Click += btnExcel_Click_1;
        // 
        // btnIn
        // 
        btnIn.Location = new Point(800, 15);
        btnIn.Name = "btnIn";
        btnIn.Size = new Size(120, 34);
        btnIn.TabIndex = 7;
        btnIn.Text = "In báo cáo";
        // 
        // tabs
        // 
        tabs.Controls.Add(tabDoanhThu);
        tabs.Controls.Add(tabMon);
        tabs.Dock = DockStyle.Fill;
        tabs.Location = new Point(0, 105);
        tabs.Name = "tabs";
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1100, 525);
        tabs.TabIndex = 0;
        // 
        // tabDoanhThu
        // 
        tabDoanhThu.Controls.Add(gridDoanhThu);
        tabDoanhThu.Location = new Point(4, 32);
        tabDoanhThu.Name = "tabDoanhThu";
        tabDoanhThu.Size = new Size(1092, 489);
        tabDoanhThu.TabIndex = 0;
        tabDoanhThu.Text = "Doanh thu";
        // 
        // gridDoanhThu
        // 
        gridDoanhThu.AllowUserToAddRows = false;
        gridDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridDoanhThu.BackgroundColor = Color.White;
        gridDoanhThu.ColumnHeadersHeight = 29;
        gridDoanhThu.Dock = DockStyle.Fill;
        gridDoanhThu.Location = new Point(0, 0);
        gridDoanhThu.Name = "gridDoanhThu";
        gridDoanhThu.ReadOnly = true;
        gridDoanhThu.RowHeadersVisible = false;
        gridDoanhThu.RowHeadersWidth = 51;
        gridDoanhThu.Size = new Size(1092, 489);
        gridDoanhThu.TabIndex = 0;
        // 
        // tabMon
        // 
        tabMon.Controls.Add(gridMon);
        tabMon.Location = new Point(4, 29);
        tabMon.Name = "tabMon";
        tabMon.Size = new Size(192, 67);
        tabMon.TabIndex = 1;
        tabMon.Text = "Món bán chạy";
        // 
        // gridMon
        // 
        gridMon.AllowUserToAddRows = false;
        gridMon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridMon.BackgroundColor = Color.White;
        gridMon.ColumnHeadersHeight = 29;
        gridMon.Dock = DockStyle.Fill;
        gridMon.Location = new Point(0, 0);
        gridMon.Name = "gridMon";
        gridMon.ReadOnly = true;
        gridMon.RowHeadersVisible = false;
        gridMon.RowHeadersWidth = 51;
        gridMon.Size = new Size(192, 67);
        gridMon.TabIndex = 0;
        // 
        // lblTong
        // 
        lblTong.Dock = DockStyle.Bottom;
        lblTong.ForeColor = Color.FromArgb(168, 63, 43);
        lblTong.Location = new Point(0, 630);
        lblTong.Name = "lblTong";
        lblTong.Padding = new Padding(14, 10, 0, 0);
        lblTong.Size = new Size(1100, 70);
        lblTong.TabIndex = 1;
        // 
        // frmBaoCao
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(1100, 700);
        Controls.Add(tabs);
        Controls.Add(lblTong);
        Controls.Add(boLoc);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(950, 620);
        Name = "frmBaoCao";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Báo cáo thống kê";
        boLoc.ResumeLayout(false);
        boLoc.PerformLayout();
        tabs.ResumeLayout(false);
        tabDoanhThu.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridDoanhThu).EndInit();
        tabMon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridMon).EndInit();
        ResumeLayout(false);
    }
}
