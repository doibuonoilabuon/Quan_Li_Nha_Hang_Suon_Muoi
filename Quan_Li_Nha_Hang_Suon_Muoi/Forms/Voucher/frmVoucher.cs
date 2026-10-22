using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Voucher;
public partial class frmVoucher : frmCrudModule
{
    public frmVoucher()
    {
        InitializeComponent();
        SetupModule("vouchers", "Voucher");
    }
}
