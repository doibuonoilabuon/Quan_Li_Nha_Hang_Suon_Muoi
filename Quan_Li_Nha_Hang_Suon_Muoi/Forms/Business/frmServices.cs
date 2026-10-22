using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmServices : frmEntityList
{
    public frmServices()
    {
        InitializeComponent();
        ActionButton("+ Dịch vụ", () => Edit(null), true);
        ActionButton("Sửa", () => Edit(Ui.SelectedId(grid)), true);
        ActionButton(
            "Xóa",
            () => Ui.Confirm(
                this,
                "Xóa dịch vụ đang chọn?",
                () =>
        {
            RestaurantStore.Current.DeleteService(Ui.SelectedId(grid));
            RefreshRows();
        }
            ),
            true
        );
        AddRefresh();
    }

    protected override void RefreshRows()
    {
        Bind(
            RestaurantStore.Current.Snapshot().Services.Where(x => Matches(x.Name))
                .Select(
                    x => new { x.Id, Dịch_vụ = x.Name, Giá = x.Price, Trạng_thái = x.Active ? "Đang cung cấp" : "Ngừng cung cấp" }
                )
        );
    }

    private void Edit(string? id)
    {
        var item = id == null ? new ExtraService() : RestaurantStore.Current.Snapshot().Services.Single(x => x.Id == id);
        using var form = new frmEntry();
        var name = form.TextField("Tên dịch vụ", item.Name);
        var price = form.NumberField("Giá dịch vụ (đồng)", item.Price);
        var active = form.BoolField("Đang cung cấp", item.Active);
        if (form.Edit(
            this,
            "Dịch vụ kèm theo",
            () =>
        {
            item.Name = name.Text;
            item.Price = price.Value;
            item.Active = active.Checked;
            RestaurantStore.Current.SaveService(item);
        }
        ))
        {
            RefreshRows();
        }
    }
}
