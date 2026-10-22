using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;
using FoodItem = Quan_Li_Nha_Hang_Suon_Muoi.Models.MenuItem;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Food;
public partial class frmFood : Form
{
    public frmFood()
    {
        InitializeComponent();
        Wire(btnAdd, () => Edit(null), true);
        Wire(btnEdit, () => Edit(Ui.SelectedId(grid)), true);
        Wire(
            btnDelete,
            () => Ui.Confirm(
                this,
                "Xóa món đang chọn?",
                () =>
        {
            RestaurantStore.Current.DeleteFood(Ui.SelectedId(grid));
            RefreshRows();
        }
            ),
            true
        );
        Wire(
            btnRefresh,
            () =>
        {
            RestaurantStore.Current.Reload();
            RefreshRows();
        }
        );
        Wire(btnExcel, () => ExcelExport.Show(this, grid, "Món ăn"));
        Wire(btnRecipe, Recipe);
        search.TextChanged += (_, _) => Ui.Run(this, RefreshRows);
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
            {
                Ui.Run(this, Recipe);
            }
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!Ui.IsDesigner(this))
        {
            Ui.Run(this, RefreshRows);
        }
    }

    private bool Matches(params string[] fields)
    {
        return string.IsNullOrWhiteSpace(search.Text)
            || fields.Any(x => x.Contains(search.Text.Trim(), StringComparison.CurrentCultureIgnoreCase));
    }

    private void Bind<T>(IEnumerable<T> rows)
    {
        grid.DataSource = rows.ToList();
        if (grid.Columns["Id"] is { } idColumn)
        {
            idColumn.Visible = false;
        }

        footer.Text = $"{grid.Rows.Count} dòng • Dữ liệu SQL Server";
    }

    private void Wire(Button button, Action action, bool adminOnly = false)
    {
        button.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            if (adminOnly && !Ui.Admin)
            {
                throw new InvalidOperationException("Chỉ quản trị viên được thay đổi danh mục.");
            }

            action();
        }
        );
    }

    private void RefreshRows()
    {
        var s = RestaurantStore.Current.Snapshot();
        Bind(
            s.Foods.Where(x => Matches(x.Name, x.Category))
                .Select(
                    x => new { x.Id, Món = x.Name, Danh_mục = x.Category, Giá_bán = x.Price, Trạng_thái = x.Active ? "Đang bán" : "Ngừng bán", Ảnh = string.IsNullOrEmpty(x.ImagePath) ? "Chưa có" : "Đã có", Công_thức = s.Recipes.Count(r => r.FoodId == x.Id) + " nguyên liệu" }
                )
        );
        foreach (DataGridViewColumn column in grid.Columns)
        {
            column.HeaderText = column.HeaderText.Replace('_', ' ');
        }

        if (grid.Columns["Giá_bán"] is { } priceColumn)
        {
            priceColumn.DefaultCellStyle.Format = "N0";
        }
    }

    private void Edit(string? id)
    {
        var item = id == null ? new FoodItem() : RestaurantStore.Current.Snapshot().Foods.Single(x => x.Id == id);
        using var form = new frmEntry();
        var name = form.TextField("Tên món / đồ uống", item.Name);
        var category = form.TextField("Danh mục", item.Category);
        var price = form.NumberField("Giá bán (đồng)", item.Price);
        var active = form.BoolField("Đang bán", item.Active);
        var image = form.ImageField(item.ImagePath);
        if (form.Edit(
            this,
            id == null ? "Thêm món" : "Sửa món / ảnh",
            () =>
        {
            item.Name = name.Text;
            item.Category = category.Text;
            item.Price = price.Value;
            item.Active = active.Checked;
            if (string.IsNullOrWhiteSpace(item.Name) || string.IsNullOrWhiteSpace(item.Category) || item.Price <= 0)
            {
                throw new InvalidOperationException("Nhập tên, danh mục và giá bán lớn hơn 0.");
            }

            item.ImagePath = RestaurantStore.Current.ImportImage(image.Text);
            RestaurantStore.Current.SaveFood(item);
        }
        ))
        {
            RefreshRows();
        }
    }

    private void Recipe()
    {
        var id = Ui.SelectedId(grid);
        using var form = new frmRecipe(id);
        form.ShowDialog(this);
        RefreshRows();
    }
}
