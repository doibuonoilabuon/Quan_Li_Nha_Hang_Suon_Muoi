using Quan_Li_Nha_Hang_Suon_Muoi.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new frmDangNhap());
    }
}
