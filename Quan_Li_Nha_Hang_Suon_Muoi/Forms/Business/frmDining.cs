using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmDining : frmEntityList
{
    public frmDining()
    {
        InitializeComponent();
        ActionButton("+ Bàn", () => Edit(null), true);
        ActionButton("Sửa bàn", () => Edit(Ui.SelectedId(grid)), true);
        ActionButton(
            "Xóa bàn",
            () => Ui.Confirm(
                this,
                "Xóa bàn đang chọn?",
                () =>
        {
            RestaurantStore.Current.DeleteTable(Ui.SelectedId(grid));
            RefreshRows();
        }
            ),
            true
        );
        ActionButton(
            "+ Đặt bàn",
            () =>
        {
            using var f = new frmReservations(Ui.SelectedId(grid));
            f.NewBooking();
            RefreshRows();
        }
        );
        ActionButton("Mở hóa đơn", OpenSale);
        AddRefresh();
        grid.CellDoubleClick += (_, e) =>
        {
            if (e.RowIndex >= 0)
            {
                Ui.Run(this, OpenSale);
            }
        };
    }

    protected override void RefreshRows()
    {
        var s = RestaurantStore.Current.Snapshot();
        Bind(
            s.Tables.Where(x => Matches(x.Name))
                .Select(
                    x => new
                    {
                        x.Id,
                        Bàn = x.Name,
                        Số_chỗ = x.Seats,
                        Trạng_thái = RestaurantStore.TableStatus(s, x.Id),
                        Khách_đã_đặt = s.Reservations.Where(r => r.TableId == x.Id && r.Status == "Đã đặt" && r.Time >= DateTime.Today)
                        .OrderBy(r => r.Time)
                        .FirstOrDefault()?.Customer ?? ""
                    }
                )
        );
        foreach (DataGridViewRow r in grid.Rows)
        {
            r.DefaultCellStyle.BackColor = r.Cells["Trạng_thái"].Value?.ToString() switch
            {
                "Đang phục vụ" => Color.MistyRose,
                "Đã đặt" => Color.LightGoldenrodYellow,
                _ => Color.Honeydew
            };
        }
    }

    private void Edit(string? id)
    {
        var item = id == null ? new DiningTable() : RestaurantStore.Current.Snapshot().Tables.Single(x => x.Id == id);
        using var f = new frmEntry();
        var name = f.TextField("Tên bàn", item.Name);
        var seats = f.NumberField("Số chỗ", item.Seats, 100);
        if (f.Edit(
            this,
            "Thông tin bàn",
            () =>
        {
            item.Name = name.Text;
            item.Seats = (int)seats.Value;
            RestaurantStore.Current.SaveTable(item);
        }
        ))
        {
            RefreshRows();
        }
    }

    private void OpenSale()
    {
        var id = Ui.SelectedId(grid);
        var s = RestaurantStore.Current.Snapshot();
        var sale = s.Sales.Find(x => x.TableId == id && x.Status == "Đang phục vụ") ?? new Sale
        {
            TableId = id
        };
        using var form = new frmSaleEdit(sale);
        form.ShowDialog(this);
        RefreshRows();
    }
}
