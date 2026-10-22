using System;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Auth;

namespace Quan_Li_Nha_Hang_Suon_Muoi;
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            _ = Services.RestaurantStore.Current;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Không mở được cơ sở dữ liệu SQL Server. Hãy kiểm tra SQL Server và App.config.\n" + ex.Message,
                "SƯỜN MƯỜI",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return;
        }

        Application.Run(new frmLogin());
    }
}
