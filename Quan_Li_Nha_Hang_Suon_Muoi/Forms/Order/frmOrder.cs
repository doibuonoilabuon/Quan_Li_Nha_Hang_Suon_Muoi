using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Order;
public partial class frmOrder : Form
{
    public frmOrder()
    {
        InitializeComponent();
        Wire(btnAdd, () => Open(new Sale()));
        Wire(btnEdit, () => Open(Selected()));
        Wire(
            btnDelete,
            () => Ui.Confirm(
                this,
                "Hủy đơn hàng đang chọn? Chỉ đơn chưa thanh toán được hủy; đơn đã thanh toán được giữ để đối soát.",
                () =>
        {
            RestaurantStore.Current.CancelSale(Ui.SelectedId(grid));
            RefreshRows();
        }
            )
        );
        Wire(
            btnRefresh,
            () =>
        {
            RestaurantStore.Current.Reload();
            RefreshRows();
        }
        );
        Wire(btnExcel, () => ExcelExport.Show(this, grid, "Đơn hàng"));
        Wire(btnPrintOrder, PrintSelectedOrder);
        search.TextChanged += (_, _) => Ui.Run(this, RefreshRows);
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
            {
                Ui.Run(this, () => Open(Selected()));
            }
        };
    }

    private Sale Selected()
    {
        return RestaurantStore.Current.Snapshot().Sales.Single(x => x.Id == Ui.SelectedId(grid));
    }

    private void PrintSelectedOrder()
    {
        var sale = Selected();
        Reports.Preview(this, sale.Id, Reports.SaleDocument(sale));
    }

    private void Open(Sale sale)
    {
        using var f = new frmSaleEdit(sale);
        f.ShowDialog(this);
        RefreshRows();
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
        Bind(
            RestaurantStore.Current.Snapshot().Sales.Where(x => Matches(x.Id, x.Customer, x.TableName, x.Status))
                .OrderByDescending(x => x.CreatedAt)
                .Select(
                    x => new { x.Id, Mã_đơn = x.Id, Ngày = x.CreatedAt, Bàn = x.TableName, Khách = x.Customer, Tổng_tiền = x.Total, Trạng_thái = x.Status, Thanh_toán = x.PaidAt, Phương_thức = x.PaymentMethod }
                )
        );
        foreach (DataGridViewColumn column in grid.Columns)
        {
            column.HeaderText = column.HeaderText.Replace('_', ' ');
            if (column.ValueType == typeof(decimal))
            {
                column.DefaultCellStyle.Format = "N0";
            }

            if (column.ValueType == typeof(DateTime) || column.ValueType == typeof(DateTime?))
            {
                column.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }
    }
}
