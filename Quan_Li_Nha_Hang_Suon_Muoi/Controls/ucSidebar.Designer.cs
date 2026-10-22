#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
partial class ucSidebar
{
    private System.ComponentModel.IContainer components = null;
    private Label brand;
    private Label subBrand;
    private FlowLayoutPanel navList;
    private Button btnMore;
    private ContextMenuStrip moreMenu;
    private Button btn_dashboard;
    private Button btn_food;
    private Button btn_category;
    private Button btn_foodoption;
    private Button btn_combo;
    private Button btn_orders;
    private Button btn_vouchers;
    private Button btn_customers;
    private Button btn_banners;
    private Button btn_openinghours;
    private ToolStripMenuItem menu_tables;
    private ToolStripMenuItem menu_reservations;
    private ToolStripMenuItem menu_purchases;
    private ToolStripMenuItem menu_ingredients;
    private ToolStripMenuItem menu_services;
    private ToolStripMenuItem menu_reports;
    private ToolStripMenuItem menu_help;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        brand = new Label();
        subBrand = new Label();
        navList = new FlowLayoutPanel();
        btnMore = new Button();
        moreMenu = new ContextMenuStrip(components);
        btn_dashboard = new Button();
        btn_food = new Button();
        btn_category = new Button();
        btn_foodoption = new Button();
        btn_combo = new Button();
        btn_orders = new Button();
        btn_vouchers = new Button();
        btn_customers = new Button();
        btn_banners = new Button();
        btn_openinghours = new Button();
        menu_tables = new ToolStripMenuItem();
        menu_reservations = new ToolStripMenuItem();
        menu_purchases = new ToolStripMenuItem();
        menu_ingredients = new ToolStripMenuItem();
        menu_services = new ToolStripMenuItem();
        menu_reports = new ToolStripMenuItem();
        menu_help = new ToolStripMenuItem();
        navList.SuspendLayout();
        moreMenu.SuspendLayout();
        SuspendLayout();
        // brand
        brand.Dock = DockStyle.Top;
        brand.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        brand.ForeColor = Color.White;
        brand.Location = Point.Empty;
        brand.Name = "brand";
        brand.Size = new Size(250, 67);
        brand.Text = "SƯỜN MƯỜI";
        brand.TextAlign = ContentAlignment.BottomCenter;
        // subBrand
        subBrand.Dock = DockStyle.Top;
        subBrand.Font = new Font("Segoe UI", 9F);
        subBrand.ForeColor = Color.FromArgb(220, 196, 179);
        subBrand.Location = new Point(0, 67);
        subBrand.Name = "subBrand";
        subBrand.Size = new Size(250, 35);
        subBrand.Text = "QUẢN TRỊ NHÀ HÀNG";
        subBrand.TextAlign = ContentAlignment.TopCenter;
        // navList
        navList.AutoScroll = true;
        navList.Dock = DockStyle.Fill;
        navList.FlowDirection = FlowDirection.TopDown;
        navList.Location = new Point(0, 102);
        navList.Name = "navList";
        navList.Padding = new Padding(12, 22, 6, 8);
        navList.Size = new Size(250, 756);
        navList.WrapContents = false;
        navList.Controls.Add(btn_dashboard);
        navList.Controls.Add(btn_food);
        navList.Controls.Add(btn_category);
        navList.Controls.Add(btn_foodoption);
        navList.Controls.Add(btn_combo);
        navList.Controls.Add(btn_orders);
        navList.Controls.Add(btn_vouchers);
        navList.Controls.Add(btn_customers);
        navList.Controls.Add(btn_banners);
        navList.Controls.Add(btn_openinghours);
        // btn_dashboard
        btn_dashboard.BackColor = Color.FromArgb(36, 32, 29);
        btn_dashboard.Cursor = Cursors.Hand;
        btn_dashboard.FlatAppearance.BorderSize = 0;
        btn_dashboard.FlatStyle = FlatStyle.Flat;
        btn_dashboard.Font = new Font("Segoe UI", 10F);
        btn_dashboard.ForeColor = Color.White;
        btn_dashboard.Location = new Point(12, 124);
        btn_dashboard.Margin = new Padding(0, 0, 0, 4);
        btn_dashboard.Name = "btn_dashboard";
        btn_dashboard.Size = new Size(220, 43);
        btn_dashboard.TabIndex = 0;
        btn_dashboard.Text = "  Tổng quan";
        btn_dashboard.TextAlign = ContentAlignment.MiddleLeft;
        btn_dashboard.UseVisualStyleBackColor = false;
        // btn_food
        btn_food.BackColor = Color.FromArgb(36, 32, 29);
        btn_food.Cursor = Cursors.Hand;
        btn_food.FlatAppearance.BorderSize = 0;
        btn_food.FlatStyle = FlatStyle.Flat;
        btn_food.Font = new Font("Segoe UI", 10F);
        btn_food.ForeColor = Color.White;
        btn_food.Location = new Point(12, 171);
        btn_food.Margin = new Padding(0, 0, 0, 4);
        btn_food.Name = "btn_food";
        btn_food.Size = new Size(220, 43);
        btn_food.TabIndex = 1;
        btn_food.Text = "  Món ăn";
        btn_food.TextAlign = ContentAlignment.MiddleLeft;
        btn_food.UseVisualStyleBackColor = false;
        // btn_category
        btn_category.BackColor = Color.FromArgb(36, 32, 29);
        btn_category.Cursor = Cursors.Hand;
        btn_category.FlatAppearance.BorderSize = 0;
        btn_category.FlatStyle = FlatStyle.Flat;
        btn_category.Font = new Font("Segoe UI", 10F);
        btn_category.ForeColor = Color.White;
        btn_category.Location = new Point(12, 218);
        btn_category.Margin = new Padding(0, 0, 0, 4);
        btn_category.Name = "btn_category";
        btn_category.Size = new Size(220, 43);
        btn_category.TabIndex = 2;
        btn_category.Text = "  Danh mục";
        btn_category.TextAlign = ContentAlignment.MiddleLeft;
        btn_category.UseVisualStyleBackColor = false;
        // btn_foodoption
        btn_foodoption.BackColor = Color.FromArgb(36, 32, 29);
        btn_foodoption.Cursor = Cursors.Hand;
        btn_foodoption.FlatAppearance.BorderSize = 0;
        btn_foodoption.FlatStyle = FlatStyle.Flat;
        btn_foodoption.Font = new Font("Segoe UI", 10F);
        btn_foodoption.ForeColor = Color.White;
        btn_foodoption.Location = new Point(12, 265);
        btn_foodoption.Margin = new Padding(0, 0, 0, 4);
        btn_foodoption.Name = "btn_foodoption";
        btn_foodoption.Size = new Size(220, 43);
        btn_foodoption.TabIndex = 3;
        btn_foodoption.Text = "  Tùy chọn món";
        btn_foodoption.TextAlign = ContentAlignment.MiddleLeft;
        btn_foodoption.UseVisualStyleBackColor = false;
        // btn_combo
        btn_combo.BackColor = Color.FromArgb(36, 32, 29);
        btn_combo.Cursor = Cursors.Hand;
        btn_combo.FlatAppearance.BorderSize = 0;
        btn_combo.FlatStyle = FlatStyle.Flat;
        btn_combo.Font = new Font("Segoe UI", 10F);
        btn_combo.ForeColor = Color.White;
        btn_combo.Location = new Point(12, 312);
        btn_combo.Margin = new Padding(0, 0, 0, 4);
        btn_combo.Name = "btn_combo";
        btn_combo.Size = new Size(220, 43);
        btn_combo.TabIndex = 4;
        btn_combo.Text = "  Combo";
        btn_combo.TextAlign = ContentAlignment.MiddleLeft;
        btn_combo.UseVisualStyleBackColor = false;
        // btn_orders
        btn_orders.BackColor = Color.FromArgb(36, 32, 29);
        btn_orders.Cursor = Cursors.Hand;
        btn_orders.FlatAppearance.BorderSize = 0;
        btn_orders.FlatStyle = FlatStyle.Flat;
        btn_orders.Font = new Font("Segoe UI", 10F);
        btn_orders.ForeColor = Color.White;
        btn_orders.Location = new Point(12, 359);
        btn_orders.Margin = new Padding(0, 0, 0, 4);
        btn_orders.Name = "btn_orders";
        btn_orders.Size = new Size(220, 43);
        btn_orders.TabIndex = 5;
        btn_orders.Text = "  Đơn hàng";
        btn_orders.TextAlign = ContentAlignment.MiddleLeft;
        btn_orders.UseVisualStyleBackColor = false;
        // btn_vouchers
        btn_vouchers.BackColor = Color.FromArgb(36, 32, 29);
        btn_vouchers.Cursor = Cursors.Hand;
        btn_vouchers.FlatAppearance.BorderSize = 0;
        btn_vouchers.FlatStyle = FlatStyle.Flat;
        btn_vouchers.Font = new Font("Segoe UI", 10F);
        btn_vouchers.ForeColor = Color.White;
        btn_vouchers.Location = new Point(12, 406);
        btn_vouchers.Margin = new Padding(0, 0, 0, 4);
        btn_vouchers.Name = "btn_vouchers";
        btn_vouchers.Size = new Size(220, 43);
        btn_vouchers.TabIndex = 6;
        btn_vouchers.Text = "  Voucher";
        btn_vouchers.TextAlign = ContentAlignment.MiddleLeft;
        btn_vouchers.UseVisualStyleBackColor = false;
        // btn_customers
        btn_customers.BackColor = Color.FromArgb(36, 32, 29);
        btn_customers.Cursor = Cursors.Hand;
        btn_customers.FlatAppearance.BorderSize = 0;
        btn_customers.FlatStyle = FlatStyle.Flat;
        btn_customers.Font = new Font("Segoe UI", 10F);
        btn_customers.ForeColor = Color.White;
        btn_customers.Location = new Point(12, 453);
        btn_customers.Margin = new Padding(0, 0, 0, 4);
        btn_customers.Name = "btn_customers";
        btn_customers.Size = new Size(220, 43);
        btn_customers.TabIndex = 7;
        btn_customers.Text = "  Khách hàng";
        btn_customers.TextAlign = ContentAlignment.MiddleLeft;
        btn_customers.UseVisualStyleBackColor = false;
        // btn_banners
        btn_banners.BackColor = Color.FromArgb(36, 32, 29);
        btn_banners.Cursor = Cursors.Hand;
        btn_banners.FlatAppearance.BorderSize = 0;
        btn_banners.FlatStyle = FlatStyle.Flat;
        btn_banners.Font = new Font("Segoe UI", 10F);
        btn_banners.ForeColor = Color.White;
        btn_banners.Location = new Point(12, 500);
        btn_banners.Margin = new Padding(0, 0, 0, 4);
        btn_banners.Name = "btn_banners";
        btn_banners.Size = new Size(220, 43);
        btn_banners.TabIndex = 8;
        btn_banners.Text = "  Banner";
        btn_banners.TextAlign = ContentAlignment.MiddleLeft;
        btn_banners.UseVisualStyleBackColor = false;
        // btn_openinghours
        btn_openinghours.BackColor = Color.FromArgb(36, 32, 29);
        btn_openinghours.Cursor = Cursors.Hand;
        btn_openinghours.FlatAppearance.BorderSize = 0;
        btn_openinghours.FlatStyle = FlatStyle.Flat;
        btn_openinghours.Font = new Font("Segoe UI", 10F);
        btn_openinghours.ForeColor = Color.White;
        btn_openinghours.Location = new Point(12, 547);
        btn_openinghours.Margin = new Padding(0, 0, 0, 4);
        btn_openinghours.Name = "btn_openinghours";
        btn_openinghours.Size = new Size(220, 43);
        btn_openinghours.TabIndex = 9;
        btn_openinghours.Text = "  Giờ mở cửa";
        btn_openinghours.TextAlign = ContentAlignment.MiddleLeft;
        btn_openinghours.UseVisualStyleBackColor = false;
        // btnMore
        btnMore.BackColor = Color.FromArgb(36, 32, 29);
        btnMore.Cursor = Cursors.Hand;
        btnMore.Dock = DockStyle.Bottom;
        btnMore.FlatAppearance.BorderSize = 0;
        btnMore.FlatStyle = FlatStyle.Flat;
        btnMore.Font = new Font("Segoe UI", 9F);
        btnMore.ForeColor = Color.FromArgb(220, 196, 179);
        btnMore.Location = new Point(0, 858);
        btnMore.Name = "btnMore";
        btnMore.Padding = new Padding(14, 0, 0, 0);
        btnMore.Size = new Size(250, 42);
        btnMore.TabIndex = 10;
        btnMore.Text = "Nghiệp vụ khác  ▴";
        btnMore.TextAlign = ContentAlignment.MiddleLeft;
        btnMore.UseVisualStyleBackColor = false;
        // moreMenu
        moreMenu.Name = "moreMenu";
        menu_tables.Name = "menu_tables";
        menu_tables.Text = "Bàn / Trạng thái";
        moreMenu.Items.Add(menu_tables);
        menu_reservations.Name = "menu_reservations";
        menu_reservations.Text = "Đặt bàn";
        moreMenu.Items.Add(menu_reservations);
        menu_purchases.Name = "menu_purchases";
        menu_purchases.Text = "Nhập nguyên liệu";
        moreMenu.Items.Add(menu_purchases);
        menu_ingredients.Name = "menu_ingredients";
        menu_ingredients.Text = "Nguyên liệu / Tồn kho";
        moreMenu.Items.Add(menu_ingredients);
        menu_services.Name = "menu_services";
        menu_services.Text = "Dịch vụ kèm theo";
        moreMenu.Items.Add(menu_services);
        menu_reports.Name = "menu_reports";
        menu_reports.Text = "In / Thống kê";
        moreMenu.Items.Add(menu_reports);
        menu_help.Name = "menu_help";
        menu_help.Text = "Hướng dẫn / Sao lưu";
        moreMenu.Items.Add(menu_help);
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(36, 32, 29);
        Controls.Add(navList);
        Controls.Add(btnMore);
        Controls.Add(subBrand);
        Controls.Add(brand);
        Dock = DockStyle.Left;
        Name = "ucSidebar";
        Size = new Size(250, 900);
        navList.ResumeLayout(false);
        moreMenu.ResumeLayout(false);
        ResumeLayout(false);
    }
}
