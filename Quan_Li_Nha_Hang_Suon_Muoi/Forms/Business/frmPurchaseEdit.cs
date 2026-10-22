using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmPurchaseEdit : Form
{
    private BindingList<PurchaseLine> rows = [];
    public frmPurchaseEdit()
    {
        InitializeComponent();
        Shown += (_, _) => Ui.Fit(this);
        Text = "Phiếu nhập nguyên liệu • Đơn giá theo đơn vị cơ sở";
        date.Value = DateTime.Now;
        btnRemove.Click += (_, _) =>
        {
            LineGrids.Remove(gridLines);
            Total();
        };
        btnClose.Click += (_, _) => Close();
        btnPost.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            LineGrids.End(gridLines);
            var receipt = new Purchase
            {
                Supplier = supplier.Text,
                Date = date.Value,
                Lines = rows.ToList()
            };
            if (MessageBox.Show(
                    this,
                    "Ghi sổ phiếu nhập và cộng tồn kho? Phiếu đã ghi sổ không sửa/xóa.",
                    "Xác nhận",
                    MessageBoxButtons.YesNo
                ) != DialogResult.Yes)
            {
                return;
            }

            RestaurantStore.Current.PostPurchase(receipt);
            DialogResult = DialogResult.OK;
            Close();
        }
        );
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (Ui.IsDesigner(this))
        {
            return;
        }

        Ui.Run(
            this,
            () =>
        {
            LineGrids.Setup(gridLines, true);
            LineGrids.Ingredients(gridLines, RestaurantStore.Current.Snapshot().Ingredients);
            LineGrids.Text(gridLines, "Quantity", "Số lượng", format: "N3");
            LineGrids.Text(gridLines, "UnitCost", "Đơn giá / Đơn vị cơ sở", format: "N3");
            LineGrids.Text(
                    gridLines,
                    "Amount",
                    "Thành tiền",
                    true,
                    "N0"
                );
            gridLines.DataSource = rows;
            gridLines.CellEndEdit += (_, _) => Total();
            gridLines.RowsRemoved += (_, _) => Total();
            Total();
        }
        );
    }

    private void Total()
    {
        gridLines.Refresh();
        total.Text = "Tổng: " + Reports.Money(rows.Sum(x => x.Amount));
    }
}
