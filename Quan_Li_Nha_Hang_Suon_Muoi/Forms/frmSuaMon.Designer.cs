namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;
partial class frmSuaMon
{
    private Label lblTen = null!;
    private TextBox txtTen = null!;
    private Label lblDanhMuc = null!;
    private TextBox txtDanhMuc = null!;
    private Label lblGia = null!;
    private NumericUpDown numGia = null!;
    private CheckBox chkDangBan = null!;
    private PictureBox picAnh = null!;
    private Button btnChonAnh = null!;
    private Button btnXoaAnh = null!;
    private Button btnLuu = null!;
    private Button btnHuy = null!;
    private void InitializeComponent()
    {
        lblTen = new Label();
        txtTen = new TextBox();
        lblDanhMuc = new Label();
        txtDanhMuc = new TextBox();
        lblGia = new Label();
        numGia = new NumericUpDown();
        chkDangBan = new CheckBox();
        picAnh = new PictureBox();
        btnChonAnh = new Button();
        btnXoaAnh = new Button();
        btnLuu = new Button();
        btnHuy = new Button();
        ((System.ComponentModel.ISupportInitialize)numGia).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picAnh).BeginInit();
        SuspendLayout();
        // 
        // lblTen
        // 
        lblTen.Location = new Point(25, 30);
        lblTen.Name = "lblTen";
        lblTen.Size = new Size(300, 25);
        lblTen.TabIndex = 0;
        lblTen.Text = "Tên món";
        // 
        // txtTen
        // 
        txtTen.Location = new Point(25, 60);
        txtTen.MaxLength = 256;
        txtTen.Name = "txtTen";
        txtTen.Size = new Size(320, 30);
        txtTen.TabIndex = 1;
        // 
        // lblDanhMuc
        // 
        lblDanhMuc.Location = new Point(25, 110);
        lblDanhMuc.Name = "lblDanhMuc";
        lblDanhMuc.Size = new Size(300, 25);
        lblDanhMuc.TabIndex = 2;
        lblDanhMuc.Text = "Danh mục";
        // 
        // txtDanhMuc
        // 
        txtDanhMuc.Location = new Point(25, 140);
        txtDanhMuc.MaxLength = 128;
        txtDanhMuc.Name = "txtDanhMuc";
        txtDanhMuc.Size = new Size(320, 30);
        txtDanhMuc.TabIndex = 3;
        // 
        // lblGia
        // 
        lblGia.Location = new Point(25, 190);
        lblGia.Name = "lblGia";
        lblGia.Size = new Size(300, 25);
        lblGia.TabIndex = 4;
        lblGia.Text = "Đơn giá (đồng)";
        // 
        // numGia
        // 
        numGia.Location = new Point(25, 220);
        numGia.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        numGia.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numGia.Name = "numGia";
        numGia.Size = new Size(320, 30);
        numGia.TabIndex = 5;
        numGia.ThousandsSeparator = true;
        numGia.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chkDangBan
        // 
        chkDangBan.Location = new Point(25, 285);
        chkDangBan.Name = "chkDangBan";
        chkDangBan.Size = new Size(300, 30);
        chkDangBan.TabIndex = 6;
        chkDangBan.Text = "Đang bán";
        // 
        // picAnh
        // 
        picAnh.BackColor = Color.White;
        picAnh.BorderStyle = BorderStyle.FixedSingle;
        picAnh.Location = new Point(380, 30);
        picAnh.Name = "picAnh";
        picAnh.Size = new Size(330, 300);
        picAnh.SizeMode = PictureBoxSizeMode.Zoom;
        picAnh.TabIndex = 7;
        picAnh.TabStop = false;
        picAnh.Click += picAnh_Click;
        // 
        // btnChonAnh
        // 
        btnChonAnh.Location = new Point(380, 350);
        btnChonAnh.Name = "btnChonAnh";
        btnChonAnh.Size = new Size(155, 38);
        btnChonAnh.TabIndex = 8;
        btnChonAnh.Text = "Chọn ảnh...";
        // 
        // btnXoaAnh
        // 
        btnXoaAnh.Location = new Point(550, 350);
        btnXoaAnh.Name = "btnXoaAnh";
        btnXoaAnh.Size = new Size(155, 38);
        btnXoaAnh.TabIndex = 9;
        btnXoaAnh.Text = "Bỏ ảnh";
        // 
        // btnLuu
        // 
        btnLuu.BackColor = Color.FromArgb(168, 63, 43);
        btnLuu.ForeColor = Color.White;
        btnLuu.Location = new Point(380, 440);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new Size(155, 42);
        btnLuu.TabIndex = 10;
        btnLuu.Text = "Lưu món";
        btnLuu.UseVisualStyleBackColor = false;
        // 
        // btnHuy
        // 
        btnHuy.DialogResult = DialogResult.Cancel;
        btnHuy.Location = new Point(550, 440);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(155, 42);
        btnHuy.TabIndex = 11;
        btnHuy.Text = "Đóng";
        // 
        // frmSuaMon
        // 
        AcceptButton = btnLuu;
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(255, 247, 237);
        CancelButton = btnHuy;
        ClientSize = new Size(760, 540);
        Controls.Add(lblTen);
        Controls.Add(txtTen);
        Controls.Add(lblDanhMuc);
        Controls.Add(txtDanhMuc);
        Controls.Add(lblGia);
        Controls.Add(numGia);
        Controls.Add(chkDangBan);
        Controls.Add(picAnh);
        Controls.Add(btnChonAnh);
        Controls.Add(btnXoaAnh);
        Controls.Add(btnLuu);
        Controls.Add(btnHuy);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(760, 540);
        Name = "frmSuaMon";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Món ăn - Thêm / Sửa ảnh";
        ((System.ComponentModel.ISupportInitialize)numGia).EndInit();
        ((System.ComponentModel.ISupportInitialize)picAnh).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
