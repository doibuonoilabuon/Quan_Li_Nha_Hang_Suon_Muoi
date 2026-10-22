using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmRecipe : Form
{
    private readonly string foodId;
    private BindingList<RecipeLine> rows = [];
    public frmRecipe() : this("")
    {
    }

    public frmRecipe(string id)
    {
        foodId = id;
        InitializeComponent();
        Shown += (_, _) => Ui.Fit(this);
        btnClose.Click += (_, _) => Close();
        btnRemove.Click += (_, _) => LineGrids.Remove(gridLines);
        btnSave.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            if (!Ui.Admin)
            {
                throw new InvalidOperationException("Chỉ quản trị viên được sửa công thức.");
            }

            LineGrids.End(gridLines);
            RestaurantStore.Current.SaveRecipe(foodId, instructions.Text, rows.ToList());
            DialogResult = DialogResult.OK;
            Close();
        }
        );
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (Ui.IsDesigner(this) || string.IsNullOrEmpty(foodId))
        {
            return;
        }

        Ui.Run(
            this,
            () =>
        {
            var s = RestaurantStore.Current.Snapshot();
            var food = s.Foods.Single(x => x.Id == foodId);
            Text = "Công thức / Pha chế • " + food.Name;
            instructions.Text = food.Instructions;
            heading.Text = food.Name + " • Định lượng cho 1 phần; cùng đơn vị cơ sở trong kho. Thêm ở dòng trống cuối bảng.";
            LineGrids.Setup(gridLines, true);
            LineGrids.Ingredients(gridLines, s.Ingredients);
            LineGrids.Text(gridLines, "Quantity", "Định lượng / 1 phần", format: "N3");
            rows = new BindingList<RecipeLine>(s.Recipes.Where(x => x.FoodId == foodId).ToList());
            gridLines.DataSource = rows;
            btnSave.Enabled = Ui.Admin;
            btnRemove.Enabled = Ui.Admin;
            gridLines.ReadOnly = !Ui.Admin;
            gridLines.AllowUserToAddRows = Ui.Admin;
            gridLines.AllowUserToDeleteRows = Ui.Admin;
            instructions.ReadOnly = !Ui.Admin;
        }
        );
    }
}
