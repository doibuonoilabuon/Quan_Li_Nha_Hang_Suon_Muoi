using System;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Auth;
public partial class frmLogin : Form
{
    public frmLogin()
    {
        InitializeComponent();
    }

    private void LoginClick(object? sender, EventArgs e)
    {
        if (!DemoAuthService.TryLogin(txtUser.Text, txtPassword.Text, out var user, out var error))
        {
            lblError.Text = error;
            return;
        }

        Session.SignIn(user!);
        Hide();
        using (var main = new Form1())
        {
            main.ShowDialog();
        }

        Session.SignOut();
        txtPassword.Clear();
        lblError.Text = "";
        Show();
    }

    private void TogglePassword(object? sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !chkShow.Checked;
    }
}
