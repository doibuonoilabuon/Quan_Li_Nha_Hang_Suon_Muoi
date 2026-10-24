namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;
partial class frmKhachHang
{
    private FlowLayoutPanel congCu = null!;
    private TextBox txtTim = null!;
    private Button btnThem = null!;
    private Button btnSua = null!;
    private Button btnXoa = null!;
    private Button btnLamMoi = null!;
    private DataGridView grid = null!;
    private Label lblSoDong = null!;
    private void InitializeComponent()
    {
        congCu = new FlowLayoutPanel();
        txtTim = new TextBox();
        btnThem = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        btnLamMoi = new Button();
        grid = new DataGridView();
        lblSoDong = new Label();
        congCu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        // 
        // congCu
        // 
        congCu.Controls.Add(txtTim);
        congCu.Controls.Add(btnThem);
        congCu.Controls.Add(btnSua);
        congCu.Controls.Add(btnXoa);
        congCu.Controls.Add(btnLamMoi);
        congCu.Dock = DockStyle.Top;
        congCu.Location = new Point(0, 0);
        congCu.Name = "congCu";
        congCu.Padding = new Padding(12);
        congCu.Size = new Size(1100, 62);
        congCu.TabIndex = 2;
        // 
        // txtTim
        // 
        txtTim.Location = new Point(15, 15);
        txtTim.Name = "txtTim";
        txtTim.PlaceholderText = "Tìm kiếm...";
        txtTim.Size = new Size(245, 30);
        txtTim.TabIndex = 0;
        // 
        // btnThem
        // 
        btnThem.Location = new Point(266, 15);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(90, 34);
        btnThem.TabIndex = 1;
        btnThem.Text = "Thêm";
        btnThem.Click += btnThem_Click;
        // 
        // btnSua
        // 
        btnSua.Location = new Point(362, 15);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(110, 34);
        btnSua.TabIndex = 2;
        btnSua.Text = "Sửa / Xem";
        // 
        // btnXoa
        // 
        btnXoa.Location = new Point(478, 15);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new Size(155, 34);
        btnXoa.TabIndex = 3;
        btnXoa.Text = "Ngừng hoạt động";
        // 
        // btnLamMoi
        // 
        btnLamMoi.Location = new Point(639, 15);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.Size = new Size(100, 34);
        btnLamMoi.TabIndex = 4;
        btnLamMoi.Text = "Làm mới";
        // 
        // grid
        // 
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.ColumnHeadersHeight = 29;
        grid.Dock = DockStyle.Fill;
        grid.Location = new Point(0, 62);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 32;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1100, 602);
        grid.TabIndex = 0;
        // 
        // lblSoDong
        // 
        lblSoDong.Dock = DockStyle.Bottom;
        lblSoDong.Location = new Point(0, 664);
        lblSoDong.Name = "lblSoDong";
        lblSoDong.Padding = new Padding(12, 8, 0, 0);
        lblSoDong.Size = new Size(1100, 36);
        lblSoDong.TabIndex = 1;
        // 
        // frmKhachHang
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(1100, 700);
        Controls.Add(grid);
        Controls.Add(lblSoDong);
        Controls.Add(congCu);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(950, 620);
        Name = "frmKhachHang";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Khách hàng";
        congCu.ResumeLayout(false);
        congCu.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
    }
}
