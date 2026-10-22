using System.ComponentModel;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmSaleEdit : Form
{
    private Sale sale;
    private BindingList<SaleLine> lines = [];
    private BindingList<ServiceLine> extras = [];
    private RestaurantState snapshot = new();
    public frmSaleEdit() : this(new Sale())
    {
    }

    public frmSaleEdit(Sale value)
    {
        sale = RestaurantStore.Clone(value);
        InitializeComponent();
        Shown += (_, _) => Ui.Fit(this);
        btnClose.Click += (_, _) => Close();
        btnAddFood.Click += (_, _) => Ui.Run(this, AddFood);
        btnAddService.Click += (_, _) => Ui.Run(this, AddService);
        btnRemove.Click += (_, _) =>
        {
            LineGrids.Remove(tabs.SelectedIndex == 0 ? gridLines : gridExtras);
            Total();
        };
        btnSave.Click += (_, _) => Ui.Run(this, () => Save(false));
        btnCheckout.Click += (_, _) => Ui.Confirm(this, "Xác nhận đã thu tiền? Thanh toán sẽ trừ nguyên liệu và giải phóng bàn.", () => Save(true));
        btnPrint.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            if (string.IsNullOrEmpty(sale.Id))
            {
                throw new InvalidOperationException("Lưu hóa đơn trước khi in tạm tính.");
            }

            var saved = RestaurantStore.Current.Snapshot().Sales.Single(x => x.Id == sale.Id);
            Reports.Preview(this, saved.Id, Reports.SaleDocument(saved));
        }
        );
        discount.ValueChanged += (_, _) => Total();
        received.ValueChanged += (_, _) => Total();
        method.SelectedIndexChanged += (_, _) => Total();
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
            snapshot = RestaurantStore.Current.Snapshot();
            Text = string.IsNullOrEmpty(sale.Id) ? "Hóa đơn mới" : sale.Id + " • " + sale.Status;
            Ui.Combo(
                    table,
                    new[] { new Choice("", "Mang về / Không dùng bàn") }.Concat(
                        snapshot.Tables.Select(x => new Choice(x.Id, x.Name + " • " + RestaurantStore.TableStatus(snapshot, x.Id)))
                    )
                );
            Ui.Select(table, sale.TableId);
            customer.Text = sale.Customer;
            Ui.Combo(
                    food,
                    snapshot.Foods.Where(x => x.Active)
                        .Select(x => new Choice(x.Id, x.Name + " • " + Reports.Money(x.Price)))
                );
            Ui.Combo(
                    service,
                    snapshot.Services.Where(x => x.Active)
                        .Select(x => new Choice(x.Id, x.Name + " • " + Reports.Money(x.Price)))
                );
            LineGrids.Setup(gridLines, false);
            LineGrids.Text(gridLines, "Name", "Món / Đồ uống", true);
            LineGrids.Text(gridLines, "Quantity", "Số lượng");
            LineGrids.Text(
                    gridLines,
                    "UnitPrice",
                    "Đơn giá",
                    true,
                    "N0"
                );
            LineGrids.Text(gridLines, "Note", "Ghi chú");
            LineGrids.Text(
                    gridLines,
                    "Amount",
                    "Thành tiền",
                    true,
                    "N0"
                );
            LineGrids.Setup(gridExtras, false);
            LineGrids.Text(gridExtras, "Name", "Dịch vụ", true);
            LineGrids.Text(gridExtras, "Quantity", "Số lượng");
            LineGrids.Text(
                    gridExtras,
                    "UnitPrice",
                    "Đơn giá",
                    true,
                    "N0"
                );
            LineGrids.Text(
                    gridExtras,
                    "Amount",
                    "Thành tiền",
                    true,
                    "N0"
                );
            lines = new BindingList<SaleLine>(sale.Lines);
            extras = new BindingList<ServiceLine>(sale.Extras);
            gridLines.DataSource = lines;
            gridExtras.DataSource = extras;
            method.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" });
            method.SelectedItem = sale.PaymentMethod;
            if (method.SelectedIndex < 0)
            {
                method.SelectedIndex = 0;
            }

            discount.Value = sale.Discount;
            received.Value = sale.Received;
            gridLines.CellEndEdit += (_, _) => Total();
            gridExtras.CellEndEdit += (_, _) => Total();
            gridLines.CellValidating += ValidateQuantity;
            gridExtras.CellValidating += ValidateQuantity;
            bool edit = sale.Status == "Đang phục vụ";
            foreach (var c in new Control[]
            {
                table,
                customer,
                food,
                service,
                qty,
                btnAddFood,
                btnAddService,
                btnRemove,
                btnSave,
                btnCheckout,
                discount,
                received,
                method
            }

            )
            {
                c.Enabled = edit;
            }

            gridLines.ReadOnly = !edit;
            gridExtras.ReadOnly = !edit;
            gridLines.AllowUserToDeleteRows = edit;
            gridExtras.AllowUserToDeleteRows = edit;
            Total();
        }
        );
    }

    private void ValidateQuantity(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (sender is DataGridView g && g.Columns[e.ColumnIndex].DataPropertyName == "Quantity"
            && (!int.TryParse(e.FormattedValue?.ToString(), out var n) || n <= 0 || n > 10000))
        {
            e.Cancel = true;
            MessageBox.Show(this, "Số lượng phải từ 1 đến 10.000.");
        }
    }

    private void AddFood()
    {
        var item = snapshot.Foods.Find(x => x.Id == Ui.ChoiceId(food)) ?? throw new InvalidOperationException("Chọn món đang bán.");
        lines.Add(
            new SaleLine { FoodId = item.Id, Name = item.Name, UnitPrice = item.Price, Quantity = (int)qty.Value }
        );
        tabs.SelectedIndex = 0;
        Total();
    }

    private void AddService()
    {
        var item = snapshot.Services.Find(x => x.Id == Ui.ChoiceId(service)) ?? throw new InvalidOperationException("Chọn dịch vụ.");
        extras.Add(
            new ServiceLine { ServiceId = item.Id, Name = item.Name, UnitPrice = item.Price, Quantity = (int)qty.Value }
        );
        tabs.SelectedIndex = 1;
        Total();
    }

    private void Total()
    {
        gridLines.Refresh();
        gridExtras.Refresh();
        var subtotal = lines.Sum(x => x.Amount) + extras.Sum(x => x.Amount);
        var net = subtotal - discount.Value;
        summary.Text = $"Tiền hàng + dịch vụ: {Reports.Money(subtotal)}    |    Phải thu: {Reports.Money(net)}    |    Tiền thừa: {Reports.Money(method.Text == "Tiền mặt" ? Math.Max(0, received.Value - net) : 0)}";
        received.Enabled = sale.Status == "Đang phục vụ" && method.Text == "Tiền mặt";
    }

    private void Save(bool checkout)
    {
        LineGrids.End(gridLines);
        LineGrids.End(gridExtras);
        sale.TableId = string.IsNullOrEmpty(Ui.ChoiceId(table)) ? null : Ui.ChoiceId(table);
        sale.Customer = customer.Text;
        sale.Lines = lines.ToList();
        sale.Extras = extras.ToList();
        sale.Discount = discount.Value;
        sale.Received = received.Value;
        sale.PaymentMethod = method.Text;
        sale.Id = RestaurantStore.Current.SaveSale(sale, checkout);
        DialogResult = DialogResult.OK;
        Close();
    }
}
