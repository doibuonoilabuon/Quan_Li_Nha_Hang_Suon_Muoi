using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmPurchases : frmEntityList
{
    public frmPurchases()
    {
        InitializeComponent();
        ActionButton(
            "+ Nhập hàng",
            () =>
        {
            using var form = new frmPurchaseEdit();
            form.ShowDialog(this);
            RefreshRows();
        }
        );
        ActionButton(
            "Xem / In phiếu",
            () =>
        {
            var p = RestaurantStore.Current.Snapshot().Purchases.Single(x => x.Id == Ui.SelectedId(grid));
            Reports.Preview(this, p.Id, Reports.PurchaseDocument(p));
        }
        );
        AddRefresh();
    }

    protected override void RefreshRows()
    {
        Bind(
            RestaurantStore.Current.Snapshot().Purchases.Where(x => Matches(x.Id, x.Supplier))
                .OrderByDescending(x => x.Date)
                .Select(
                    x => new { x.Id, Số_phiếu = x.Id, Ngày_nhập = x.Date, Nhà_cung_cấp = x.Supplier, Tổng_tiền = x.Total, Nhân_viên = x.Operator }
                )
        );
    }
}
