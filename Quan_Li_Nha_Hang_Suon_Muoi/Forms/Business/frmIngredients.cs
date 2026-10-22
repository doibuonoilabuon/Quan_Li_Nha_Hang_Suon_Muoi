using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmIngredients : frmEntityList
{
    public frmIngredients()
    {
        InitializeComponent();
        ActionButton("+ Nguyên liệu", () => Edit(null), true);
        ActionButton("Sửa", () => Edit(Ui.SelectedId(grid)), true);
        ActionButton(
            "Xóa",
            () => Ui.Confirm(
                this,
                "Xóa nguyên liệu đang chọn?",
                () =>
        {
            RestaurantStore.Current.DeleteIngredient(Ui.SelectedId(grid));
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
            RestaurantStore.Current.Snapshot().Ingredients.Where(x => Matches(x.Name, x.Unit))
                .Select(
                    x => new { x.Id, Nguyên_liệu = x.Name, Đơn_vị = x.Unit, Tồn_kho = x.Stock, Tối_thiểu = x.Minimum, Giá_vốn_bình_quân = x.AverageCost, Cảnh_báo = x.Stock <= x.Minimum ? "Cần nhập thêm" : "Đủ tồn" }
                )
        );
    }

    private void Edit(string? id)
    {
        var item = id == null ? new Ingredient() : RestaurantStore.Current.Snapshot().Ingredients.Single(x => x.Id == id);
        using var form = new frmEntry();
        var name = form.TextField("Tên nguyên liệu", item.Name);
        var unit = form.TextField("Đơn vị cơ sở (g, ml, cái...) — dùng chung trong kho / công thức", item.Unit);
        var min = form.NumberField("Mức tồn tối thiểu", item.Minimum, decimals: 3);
        if (form.Edit(
            this,
            "Nguyên liệu • Tồn kho thay đổi qua nhập hàng / thanh toán",
            () =>
        {
            item.Name = name.Text;
            item.Unit = unit.Text;
            item.Minimum = min.Value;
            RestaurantStore.Current.SaveIngredient(item);
        }
        ))
        {
            RefreshRows();
        }
    }
}
