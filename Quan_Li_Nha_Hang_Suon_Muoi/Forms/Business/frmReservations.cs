using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmReservations : frmEntityList
{
    private readonly string? preferredTable;
    public frmReservations() : this(null)
    {
    }

    public frmReservations(string? tableId)
    {
        preferredTable = tableId;
        InitializeComponent();
        ActionButton("+ Đặt bàn", NewBooking);
        ActionButton("Sửa lịch", () => Edit(Ui.SelectedId(grid)));
        ActionButton("Nhận bàn", Receive);
        ActionButton(
            "Hủy lịch",
            () => Ui.Confirm(
                this,
                "Hủy lịch đặt bàn này?",
                () =>
        {
            RestaurantStore.Current.SetReservationStatus(Ui.SelectedId(grid), "Đã hủy");
            RefreshRows();
        }
            )
        );
        AddRefresh();
    }

    public void NewBooking()
    {
        Ui.Run(this, () => Edit(null));
    }

    protected override void RefreshRows()
    {
        var s = RestaurantStore.Current.Snapshot();
        Bind(
            s.Reservations.OrderByDescending(x => x.Time).Where(x => Matches(x.Customer, x.Phone, x.Status))
                .Select(
                    x => new { x.Id, Khách_hàng = x.Customer, Điện_thoại = x.Phone, Bàn = s.Tables.Find(t => t.Id == x.TableId)?.Name, Giờ_đặt = x.Time, Số_khách = x.Guests, Trạng_thái = x.Status }
                )
        );
    }

    private void Edit(string? id)
    {
        var s = RestaurantStore.Current.Snapshot();
        var item = id == null ? new Reservation
        {
            TableId = preferredTable ?? s.Tables.FirstOrDefault()?.Id ?? ""
        }

        : s.Reservations.Single(x => x.Id == id);
        using var f = new frmEntry();
        var table = f.ChoiceField(
            "Bàn (mỗi lịch giữ bàn trong khoảng 2 giờ)",
            s.Tables.Select(x => new Choice(x.Id, $"{x.Name} • {x.Seats} chỗ")),
            item.TableId
        );
        var name = f.TextField("Tên khách", item.Customer);
        var phone = f.TextField("Số điện thoại", item.Phone);
        var time = f.DateField("Giờ khách đến", item.Time);
        var guests = f.NumberField("Số khách", item.Guests, 100);
        if (f.Edit(
            this,
            "Đặt bàn",
            () =>
        {
            item.TableId = Ui.ChoiceId(table);
            item.Customer = name.Text;
            item.Phone = phone.Text;
            item.Time = time.Value;
            item.Guests = (int)guests.Value;
            RestaurantStore.Current.SaveReservation(item);
        }
        ))
        {
            RefreshRows();
        }
    }

    private void Receive()
    {
        var s = RestaurantStore.Current.Snapshot();
        var booking = s.Reservations.Single(x => x.Id == Ui.SelectedId(grid));
        if (booking.Status != "Đã đặt")
        {
            throw new InvalidOperationException("Lịch này đã được xử lý.");
        }

        if (s.Sales.Any(x => x.TableId == booking.TableId && x.Status == "Đang phục vụ"))
        {
            throw new InvalidOperationException("Bàn đang phục vụ khách khác.");
        }

        Ui.Confirm(
            this,
            "Xác nhận khách đã đến nhận bàn?",
            () =>
        {
            RestaurantStore.Current.SetReservationStatus(booking.Id, "Đã nhận bàn");
            RefreshRows();
        }
        );
    }
}
