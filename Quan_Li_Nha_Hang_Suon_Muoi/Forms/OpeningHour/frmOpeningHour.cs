using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.OpeningHour;
public partial class frmOpeningHour : frmCrudModule
{
    public frmOpeningHour()
    {
        InitializeComponent();
        SetupModule("openinghours", "Giờ mở cửa");
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
    }
}
