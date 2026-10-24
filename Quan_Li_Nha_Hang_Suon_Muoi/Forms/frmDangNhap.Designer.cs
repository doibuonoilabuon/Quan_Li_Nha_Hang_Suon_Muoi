using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;
partial class frmDangNhap
{
    private Panel card = null!;
    private Label lblU = null!;
    private Label lblP = null!;
    private Label lblBrand = null!, lblTitle = null!, lblLoi = null!;
    private TextBox txtTen = null!, txtMatKhau = null!;
    private CheckBox chkShow = null!;
    private Button btnDangNhap = null!;
    private void InitializeComponent()
    {
        card = new Panel();
        lblBrand = new Label();
        lblTitle = new Label();
        lblU = new Label();
        txtTen = new TextBox();
        lblP = new Label();
        txtMatKhau = new TextBox();
        chkShow = new CheckBox();
        lblLoi = new Label();
        btnDangNhap = new Button();
        card.SuspendLayout();
        SuspendLayout();
        // 
        // card
        // 
        card.Anchor = AnchorStyles.None;
        card.BackColor = Color.White;
        card.Controls.Add(lblBrand);
        card.Controls.Add(lblTitle);
        card.Controls.Add(lblU);
        card.Controls.Add(txtTen);
        card.Controls.Add(lblP);
        card.Controls.Add(txtMatKhau);
        card.Controls.Add(chkShow);
        card.Controls.Add(lblLoi);
        card.Controls.Add(btnDangNhap);
        card.Location = new Point(265, 75);
        card.Name = "card";
        card.Size = new Size(430, 450);
        card.TabIndex = 0;
        // 
        // lblBrand
        // 
        lblBrand.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        lblBrand.ForeColor = Color.FromArgb(168, 63, 43);
        lblBrand.Location = new Point(45, 35);
        lblBrand.Name = "lblBrand";
        lblBrand.Size = new Size(350, 58);
        lblBrand.TabIndex = 0;
        lblBrand.Text = "SƯỜN MƯỜI";
        lblBrand.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTitle
        // 
        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.Location = new Point(45, 100);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(340, 36);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "Đăng nhập quản trị";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblU
        // 
        lblU.AutoSize = true;
        lblU.Location = new Point(45, 153);
        lblU.Name = "lblU";
        lblU.Size = new Size(124, 23);
        lblU.TabIndex = 2;
        lblU.Text = "Tên đăng nhập";
        // 
        // txtTen
        // 
        txtTen.Location = new Point(45, 180);
        txtTen.Name = "txtTen";
        txtTen.PlaceholderText = "Nhập tên đăng nhập";
        txtTen.Size = new Size(340, 30);
        txtTen.TabIndex = 0;
        // 
        // lblP
        // 
        lblP.AutoSize = true;
        lblP.Location = new Point(45, 231);
        lblP.Name = "lblP";
        lblP.Size = new Size(82, 23);
        lblP.TabIndex = 3;
        lblP.Text = "Mật khẩu";
        // 
        // txtMatKhau
        // 
        txtMatKhau.Location = new Point(45, 257);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.Size = new Size(340, 30);
        txtMatKhau.TabIndex = 1;
        txtMatKhau.UseSystemPasswordChar = true;
        txtMatKhau.TextChanged += txtMatKhau_TextChanged;
        // 
        // chkShow
        // 
        chkShow.AutoSize = true;
        chkShow.Location = new Point(45, 303);
        chkShow.Name = "chkShow";
        chkShow.Size = new Size(144, 27);
        chkShow.TabIndex = 2;
        chkShow.Text = "Hiện mật khẩu";
        chkShow.CheckedChanged += TogglePassword;
        // 
        // lblLoi
        // 
        lblLoi.ForeColor = Color.Firebrick;
        lblLoi.Location = new Point(45, 337);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(340, 28);
        lblLoi.TabIndex = 4;
        // 
        // btnDangNhap
        // 
        btnDangNhap.BackColor = Color.FromArgb(168, 63, 43);
        btnDangNhap.FlatAppearance.BorderSize = 0;
        btnDangNhap.FlatStyle = FlatStyle.Flat;
        btnDangNhap.ForeColor = Color.White;
        btnDangNhap.Location = new Point(45, 375);
        btnDangNhap.Name = "btnDangNhap";
        btnDangNhap.Size = new Size(340, 42);
        btnDangNhap.TabIndex = 3;
        btnDangNhap.Text = "ĐĂNG NHẬP";
        btnDangNhap.UseVisualStyleBackColor = false;
        // 
        // frmDangNhap
        // 
        AcceptButton = btnDangNhap;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(960, 600);
        Controls.Add(card);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(750, 530);
        Name = "frmDangNhap";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SƯỜN MƯỜI | Đăng nhập";
        card.ResumeLayout(false);
        card.PerformLayout();
        ResumeLayout(false);
    }
}
