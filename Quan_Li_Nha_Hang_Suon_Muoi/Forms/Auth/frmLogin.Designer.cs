using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Auth;
partial class frmLogin
{
    private Panel card = null!;
    private Label lblBrand = null!, lblTitle = null!, lblError = null!;
    private TextBox txtUser = null!, txtPassword = null!;
    private CheckBox chkShow = null!;
    private Button btnLogin = null!;
    private void InitializeComponent()
    {
        Text = "SƯỜN MƯỜI | Đăng nhập";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(960, 600);
        MinimumSize = new Size(750, 530);
        BackColor = Color.FromArgb(255, 247, 237);
        Font = new Font("Segoe UI", 10);
        card = new Panel
        {
            Size = new Size(430, 450),
            BackColor = Color.White
        };
        Controls.Add(card);
        void CenterCard()
        {
            card.Location = new Point((ClientSize.Width - card.Width) / 2, (ClientSize.Height - card.Height) / 2);
        }

        Resize += (_, _) => CenterCard();
        CenterCard();
        lblBrand = new Label
        {
            Text = "SƯỜN MƯỜI",
            Font = new Font("Segoe UI", 26, FontStyle.Bold),
            ForeColor = Color.FromArgb(168, 63, 43),
            Location = new Point(45, 35),
            Size = new Size(350, 58),
            TextAlign = ContentAlignment.MiddleCenter
        };
        lblTitle = new Label
        {
            Text = "Đăng nhập quản trị",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(45, 100),
            Size = new Size(340, 36),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var lblU = new Label
        {
            Text = "Tên đăng nhập",
            Location = new Point(45, 153),
            AutoSize = true
        };
        txtUser = new TextBox
        {
            Name = "txtUser",
            Location = new Point(45, 180),
            Size = new Size(340, 32),
            PlaceholderText = "Nhập tên đăng nhập"
        };
        var lblP = new Label
        {
            Text = "Mật khẩu",
            Location = new Point(45, 231),
            AutoSize = true
        };
        txtPassword = new TextBox
        {
            Name = "txtPassword",
            Location = new Point(45, 257),
            Size = new Size(340, 32),
            UseSystemPasswordChar = true
        };
        chkShow = new CheckBox
        {
            Name = "chkShow",
            Text = "Hiện mật khẩu",
            Location = new Point(45, 303),
            AutoSize = true
        };
        chkShow.CheckedChanged += TogglePassword;
        lblError = new Label
        {
            Name = "lblError",
            ForeColor = Color.Firebrick,
            Location = new Point(45, 337),
            Size = new Size(340, 28)
        };
        btnLogin = new Button
        {
            Name = "btnLogin",
            Text = "ĐĂNG NHẬP",
            Location = new Point(45, 375),
            Size = new Size(340, 42),
            BackColor = Color.FromArgb(168, 63, 43),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.Click += LoginClick;
        AcceptButton = btnLogin;
        card.Controls.AddRange(
            new Control[] { lblBrand, lblTitle, lblU, txtUser, lblP, txtPassword, chkShow, lblError, btnLogin }
        );
    }
}
