using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Forms;
namespace Quan_Li_Nha_Hang_Suon_Muoi;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            KhoiTaoDatabase.ThucHien();
            Application.Run(new frmDangNhap());
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không mở được ứng dụng. Kiểm tra kết nối trong DAL/ProcessDatabase.cs.\n" + ex.Message,
                "Sườn Mười", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
