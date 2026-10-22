using System;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
public partial class ucHeader : UserControl
{
    public event EventHandler? LogoutRequested;
    public ucHeader()
    {
        InitializeComponent();
        lblUser.Text = Session.CurrentUser?.DisplayName ?? "Khách";
    }

    public void SetPage(string title, string subtitle)
    {
        lblTitle.Text = title;
        lblSubtitle.Text = subtitle;
    }

    private void LogoutClick(object? sender, EventArgs e)
    {
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }
}
