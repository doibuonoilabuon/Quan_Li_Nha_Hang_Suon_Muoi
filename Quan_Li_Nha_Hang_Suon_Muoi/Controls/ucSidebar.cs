using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
public partial class ucSidebar : UserControl
{
    public event Action<string, string>? NavigationSelected;
    private readonly Dictionary<string, Button> buttons = new();
    private readonly Dictionary<string, ToolStripMenuItem> extraItems = new();
    private readonly HashSet<string> adminKeys = new()
    {
        "category",
        "foodoption",
        "combo",
        "vouchers",
        "customers",
        "banners",
        "openinghours"
    };
    public ucSidebar()
    {
        InitializeComponent();
        Bind("dashboard", "Tổng quan", btn_dashboard);
        Bind("food", "Món ăn", btn_food);
        Bind("category", "Danh mục", btn_category);
        Bind("foodoption", "Tùy chọn món", btn_foodoption);
        Bind("combo", "Combo", btn_combo);
        Bind("orders", "Đơn hàng", btn_orders);
        Bind("vouchers", "Voucher", btn_vouchers);
        Bind("customers", "Khách hàng", btn_customers);
        Bind("banners", "Banner", btn_banners);
        Bind("openinghours", "Giờ mở cửa", btn_openinghours);
        BindExtra("tables", "Bàn / Trạng thái", menu_tables);
        BindExtra("reservations", "Đặt bàn", menu_reservations);
        BindExtra("purchases", "Nhập nguyên liệu", menu_purchases);
        BindExtra("ingredients", "Nguyên liệu / Tồn kho", menu_ingredients);
        BindExtra("services", "Dịch vụ kèm theo", menu_services);
        BindExtra("reports", "In / Thống kê", menu_reports);
        BindExtra("help", "Hướng dẫn / Sao lưu", menu_help);
        btnMore.Click += (_, _) => moreMenu.Show(btnMore, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
        ApplyRoleVisibility();
        SelectItem("dashboard");
    }

    private void Bind(string key, string title, Button button)
    {
        buttons[key] = button;
        button.Click += (_, _) => Navigate(key, title);
    }

    private void BindExtra(string key, string title, ToolStripMenuItem item)
    {
        extraItems[key] = item;
        item.Click += (_, _) => Navigate(key, title);
    }

    private void Navigate(string key, string title)
    {
        SelectItem(key);
        NavigationSelected?.Invoke(key, title);
    }

    public void SelectItem(string key)
    {
        foreach (var item in buttons)
        {
            item.Value.BackColor = item.Key == key ? Color.FromArgb(168, 63, 43) : Color.FromArgb(36, 32, 29);
        }

        foreach (var item in extraItems)
        {
            item.Value.Checked = item.Key == key;
        }

        btnMore.BackColor = extraItems.ContainsKey(key) ? Color.FromArgb(168, 63, 43) : Color.FromArgb(36, 32, 29);
    }

    public void ApplyRoleVisibility()
    {
        bool admin = Ui.IsDesigner(this) || Session.CurrentUser?.Role == UserRole.Admin;
        foreach (var item in buttons)
        {
            item.Value.Visible = admin || !adminKeys.Contains(item.Key);
        }
    }
}
