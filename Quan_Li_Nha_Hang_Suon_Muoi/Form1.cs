using System;
using System.Drawing;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Controls;
using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Dashboard;
using Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi;
public partial class Form1 : Form
{
    private Form? active;
    public Form1()
    {
        InitializeComponent();
        sidebar.NavigationSelected += ShowSection;
        header.LogoutRequested += (_, _) =>
        {
            if (MessageBox.Show("Đăng xuất khỏi hệ thống?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        };
        ShowSection("dashboard", "Tổng quan");
    }

    private void ShowSection(string key, string title)
    {
        if (Session.CurrentUser?.Role != UserRole.Admin
            && key is not ("dashboard"
            or "food"
            or "orders"
            or "tables"
            or "reservations"
            or "purchases"
            or "ingredients"
            or "services"
            or "reports"
            or "help"))
        {
            MessageBox.Show("Không có quyền truy cập.");
            return;
        }

        header.Visible = key != "dashboard";
        header.SetPage(title, "SƯỜN MƯỜI • Quản lý nhà hàng");
        sidebar.SelectItem(key);
        if (active != null)
        {
            contentHost.Controls.Remove(active);
            active.Dispose();
        }

        active = key switch
        {
            "dashboard" => new frmDashboard(),
            "food" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Food.frmFood(),
            "category" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Category.frmCategory(),
            "foodoption" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.FoodOption.frmFoodOption(),
            "combo" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Combo.frmCombo(),
            "orders" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Order.frmOrder(),
            "vouchers" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Voucher.frmVoucher(),
            "customers" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Customer.frmCustomer(),
            "banners" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.Banner.frmBanner(),
            "openinghours" => new Quan_Li_Nha_Hang_Suon_Muoi.Forms.OpeningHour.frmOpeningHour(),
            "tables" => new frmDining(),
            "reservations" => new frmReservations(),
            "purchases" => new frmPurchases(),
            "ingredients" => new frmIngredients(),
            "services" => new frmServices(),
            "reports" => new frmReports(),
            "help" => new frmHelp(),
            _ => new frmDashboard()
        };
        active.TopLevel = false;
        active.FormBorderStyle = FormBorderStyle.None;
        active.Dock = DockStyle.Fill;
        contentHost.Controls.Add(active);
        active.Show();
    }

    private void sidebar_Load(object sender, EventArgs e)
    {

    }
}
