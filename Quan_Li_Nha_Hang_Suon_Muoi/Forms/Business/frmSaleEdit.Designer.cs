#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmSaleEdit
{
    private System.ComponentModel.IContainer components = null;
    private FlowLayoutPanel header;
    private Label lblTable;
    private ComboBox table;
    private Label lblCustomer;
    private TextBox customer;
    private FlowLayoutPanel picker;
    private ComboBox food;
    private NumericUpDown qty;
    private Button btnAddFood;
    private ComboBox service;
    private Button btnAddService;
    private TabControl tabs;
    private TabPage foodTab;
    private TabPage serviceTab;
    private DataGridView gridLines;
    private DataGridView gridExtras;
    private FlowLayoutPanel payments;
    private Label lblDiscount;
    private NumericUpDown discount;
    private Label lblMethod;
    private ComboBox method;
    private Label lblReceived;
    private NumericUpDown received;
    private Label summary;
    private FlowLayoutPanel actions;
    private Button btnSave;
    private Button btnCheckout;
    private Button btnPrint;
    private Button btnRemove;
    private Button btnClose;
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
        header = new FlowLayoutPanel();
        lblTable = new Label();
        table = new ComboBox();
        lblCustomer = new Label();
        customer = new TextBox();
        picker = new FlowLayoutPanel();
        food = new ComboBox();
        qty = new NumericUpDown();
        btnAddFood = new Button();
        service = new ComboBox();
        btnAddService = new Button();
        tabs = new TabControl();
        foodTab = new TabPage();
        gridLines = new DataGridView();
        serviceTab = new TabPage();
        gridExtras = new DataGridView();
        payments = new FlowLayoutPanel();
        lblDiscount = new Label();
        discount = new NumericUpDown();
        lblMethod = new Label();
        method = new ComboBox();
        lblReceived = new Label();
        received = new NumericUpDown();
        summary = new Label();
        actions = new FlowLayoutPanel();
        btnSave = new Button();
        btnCheckout = new Button();
        btnPrint = new Button();
        btnRemove = new Button();
        btnClose = new Button();
        header.SuspendLayout();
        picker.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)qty).BeginInit();
        tabs.SuspendLayout();
        foodTab.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridLines).BeginInit();
        serviceTab.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridExtras).BeginInit();
        payments.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)discount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)received).BeginInit();
        actions.SuspendLayout();
        SuspendLayout();
        //
        // header
        //
        header.Controls.Add(lblTable);
        header.Controls.Add(table);
        header.Controls.Add(lblCustomer);
        header.Controls.Add(customer);
        header.Dock = DockStyle.Top;
        header.Location = new Point(20, 20);
        header.Name = "header";
        header.Size = new Size(1160, 60);
        header.TabIndex = 2;
        //
        // lblTable
        //
        lblTable.AutoSize = true;
        lblTable.Location = new Point(3, 0);
        lblTable.Name = "lblTable";
        lblTable.Padding = new Padding(0, 7, 0, 0);
        lblTable.Size = new Size(39, 30);
        lblTable.TabIndex = 0;
        lblTable.Text = "Bàn";
        //
        // table
        //
        table.Location = new Point(48, 3);
        table.Name = "table";
        table.Size = new Size(280, 31);
        table.TabIndex = 1;
        //
        // lblCustomer
        //
        lblCustomer.AutoSize = true;
        lblCustomer.Location = new Point(334, 0);
        lblCustomer.Name = "lblCustomer";
        lblCustomer.Padding = new Padding(12, 7, 0, 0);
        lblCustomer.Size = new Size(69, 30);
        lblCustomer.TabIndex = 2;
        lblCustomer.Text = "Khách";
        //
        // customer
        //
        customer.Location = new Point(409, 3);
        customer.Name = "customer";
        customer.Size = new Size(290, 30);
        customer.TabIndex = 3;
        //
        // picker
        //
        picker.AutoScroll = true;
        picker.Controls.Add(food);
        picker.Controls.Add(qty);
        picker.Controls.Add(btnAddFood);
        picker.Controls.Add(service);
        picker.Controls.Add(btnAddService);
        picker.Dock = DockStyle.Top;
        picker.Location = new Point(20, 80);
        picker.Name = "picker";
        picker.Size = new Size(1160, 100);
        picker.TabIndex = 1;
        //
        // food
        //
        food.Location = new Point(3, 3);
        food.Name = "food";
        food.Size = new Size(285, 31);
        food.TabIndex = 0;
        //
        // qty
        //
        qty.Location = new Point(294, 3);
        qty.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        qty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        qty.Name = "qty";
        qty.Size = new Size(65, 30);
        qty.TabIndex = 1;
        qty.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // btnAddFood
        //
        btnAddFood.Location = new Point(365, 3);
        btnAddFood.Name = "btnAddFood";
        btnAddFood.Size = new Size(90, 35);
        btnAddFood.TabIndex = 2;
        btnAddFood.Text = "+ Món";
        //
        // service
        //
        service.Location = new Point(461, 3);
        service.Name = "service";
        service.Size = new Size(270, 31);
        service.TabIndex = 3;
        //
        // btnAddService
        //
        btnAddService.Location = new Point(737, 3);
        btnAddService.Name = "btnAddService";
        btnAddService.Size = new Size(110, 35);
        btnAddService.TabIndex = 4;
        btnAddService.Text = "+ Dịch vụ";
        //
        // tabs
        //
        tabs.Controls.Add(foodTab);
        tabs.Controls.Add(serviceTab);
        tabs.Dock = DockStyle.Fill;
        tabs.Location = new Point(20, 180);
        tabs.Name = "tabs";
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1160, 390);
        tabs.TabIndex = 0;
        //
        // foodTab
        //
        foodTab.Controls.Add(gridLines);
        foodTab.Location = new Point(4, 32);
        foodTab.Name = "foodTab";
        foodTab.Size = new Size(1152, 354);
        foodTab.TabIndex = 0;
        foodTab.Text = "Món / Đồ uống";
        //
        // gridLines
        //
        gridLines.ColumnHeadersHeight = 29;
        gridLines.Dock = DockStyle.Fill;
        gridLines.Location = new Point(0, 0);
        gridLines.Name = "gridLines";
        gridLines.RowHeadersWidth = 51;
        gridLines.Size = new Size(1152, 354);
        gridLines.TabIndex = 0;
        //
        // serviceTab
        //
        serviceTab.Controls.Add(gridExtras);
        serviceTab.Location = new Point(4, 29);
        serviceTab.Name = "serviceTab";
        serviceTab.Size = new Size(192, 67);
        serviceTab.TabIndex = 1;
        serviceTab.Text = "Dịch vụ kèm theo";
        //
        // gridExtras
        //
        gridExtras.ColumnHeadersHeight = 29;
        gridExtras.Dock = DockStyle.Fill;
        gridExtras.Location = new Point(0, 0);
        gridExtras.Name = "gridExtras";
        gridExtras.RowHeadersWidth = 51;
        gridExtras.Size = new Size(192, 67);
        gridExtras.TabIndex = 0;
        //
        // payments
        //
        payments.Controls.Add(lblDiscount);
        payments.Controls.Add(discount);
        payments.Controls.Add(lblMethod);
        payments.Controls.Add(method);
        payments.Controls.Add(lblReceived);
        payments.Controls.Add(received);
        payments.Dock = DockStyle.Bottom;
        payments.Location = new Point(20, 570);
        payments.Name = "payments";
        payments.Padding = new Padding(0, 10, 0, 0);
        payments.Size = new Size(1160, 85);
        payments.TabIndex = 3;
        //
        // lblDiscount
        //
        lblDiscount.AutoSize = true;
        lblDiscount.Location = new Point(3, 10);
        lblDiscount.Name = "lblDiscount";
        lblDiscount.Padding = new Padding(0, 6, 0, 0);
        lblDiscount.Size = new Size(75, 29);
        lblDiscount.TabIndex = 0;
        lblDiscount.Text = "Giảm (đ)";
        //
        // discount
        //
        discount.Location = new Point(84, 13);
        discount.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
        discount.Name = "discount";
        discount.Size = new Size(140, 30);
        discount.TabIndex = 1;
        discount.ThousandsSeparator = true;
        //
        // lblMethod
        //
        lblMethod.AutoSize = true;
        lblMethod.Location = new Point(230, 10);
        lblMethod.Name = "lblMethod";
        lblMethod.Padding = new Padding(12, 6, 0, 0);
        lblMethod.Size = new Size(110, 29);
        lblMethod.TabIndex = 2;
        lblMethod.Text = "Thanh toán";
        //
        // method
        //
        method.DropDownStyle = ComboBoxStyle.DropDownList;
        method.Location = new Point(346, 13);
        method.Name = "method";
        method.Size = new Size(150, 31);
        method.TabIndex = 3;
        //
        // lblReceived
        //
        lblReceived.AutoSize = true;
        lblReceived.Location = new Point(502, 10);
        lblReceived.Name = "lblReceived";
        lblReceived.Padding = new Padding(12, 6, 0, 0);
        lblReceived.Size = new Size(128, 29);
        lblReceived.TabIndex = 4;
        lblReceived.Text = "Khách đưa (đ)";
        //
        // received
        //
        received.Location = new Point(636, 13);
        received.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
        received.Name = "received";
        received.Size = new Size(160, 30);
        received.TabIndex = 5;
        received.ThousandsSeparator = true;
        //
        // summary
        //
        summary.Dock = DockStyle.Bottom;
        summary.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        summary.Location = new Point(20, 655);
        summary.Name = "summary";
        summary.Size = new Size(1160, 65);
        summary.TabIndex = 4;
        summary.TextAlign = ContentAlignment.MiddleLeft;
        //
        // actions
        //
        actions.AutoScroll = true;
        actions.Controls.Add(btnSave);
        actions.Controls.Add(btnCheckout);
        actions.Controls.Add(btnPrint);
        actions.Controls.Add(btnRemove);
        actions.Controls.Add(btnClose);
        actions.Dock = DockStyle.Bottom;
        actions.Location = new Point(20, 720);
        actions.Name = "actions";
        actions.Size = new Size(1160, 60);
        actions.TabIndex = 5;
        actions.WrapContents = false;
        //
        // btnSave
        //
        btnSave.Location = new Point(3, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(160, 40);
        btnSave.TabIndex = 0;
        btnSave.Text = "Lưu hóa đơn mở";
        //
        // btnCheckout
        //
        btnCheckout.BackColor = Color.FromArgb(168, 63, 43);
        btnCheckout.ForeColor = Color.White;
        btnCheckout.Location = new Point(169, 3);
        btnCheckout.Name = "btnCheckout";
        btnCheckout.Size = new Size(140, 40);
        btnCheckout.TabIndex = 1;
        btnCheckout.Text = "Thanh toán";
        btnCheckout.UseVisualStyleBackColor = false;
        //
        // btnPrint
        //
        btnPrint.Location = new Point(315, 3);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(110, 40);
        btnPrint.TabIndex = 2;
        btnPrint.Text = "In đơn hàng";
        //
        // btnRemove
        //
        btnRemove.Location = new Point(431, 3);
        btnRemove.Name = "btnRemove";
        btnRemove.Size = new Size(110, 40);
        btnRemove.TabIndex = 3;
        btnRemove.Text = "Xóa dòng";
        //
        // btnClose
        //
        btnClose.Location = new Point(547, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(100, 40);
        btnClose.TabIndex = 4;
        btnClose.Text = "Đóng";
        //
        // frmSaleEdit
        //
        AutoScaleDimensions = new SizeF(9F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(1200, 800);
        Controls.Add(tabs);
        Controls.Add(picker);
        Controls.Add(header);
        Controls.Add(payments);
        Controls.Add(summary);
        Controls.Add(actions);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(900, 650);
        Name = "frmSaleEdit";
        Padding = new Padding(20);
        StartPosition = FormStartPosition.CenterParent;
        Text = "frmSaleEdit";
        header.ResumeLayout(false);
        header.PerformLayout();
        picker.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)qty).EndInit();
        tabs.ResumeLayout(false);
        foodTab.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridLines).EndInit();
        serviceTab.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridExtras).EndInit();
        payments.ResumeLayout(false);
        payments.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)discount).EndInit();
        ((System.ComponentModel.ISupportInitialize)received).EndInit();
        actions.ResumeLayout(false);
        ResumeLayout(false);
    }
}
