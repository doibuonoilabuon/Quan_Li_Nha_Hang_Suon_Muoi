#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmPurchaseEdit
{
    private System.ComponentModel.IContainer components;
    private FlowLayoutPanel header;
    private Label lblSupplier;
    private TextBox supplier;
    private DateTimePicker date;
    private DataGridView gridLines;
    private FlowLayoutPanel actions;
    private Label total;
    private Button btnPost;
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
        components = new System.ComponentModel.Container();
        header = new FlowLayoutPanel();
        lblSupplier = new Label();
        supplier = new TextBox();
        date = new DateTimePicker();
        gridLines = new DataGridView();
        actions = new FlowLayoutPanel();
        total = new Label();
        btnPost = new Button();
        btnRemove = new Button();
        btnClose = new Button();
        SuspendLayout();
        header.Dock = DockStyle.Top;
        header.Height = 65;
        header.AutoScroll = true;
        lblSupplier.Text = "Nhà cung cấp";
        lblSupplier.AutoSize = true;
        lblSupplier.Padding = new Padding(0, 8, 0, 0);
        supplier.Name = "supplier";
        supplier.Width = 330;
        date.Name = "date";
        date.Format = DateTimePickerFormat.Custom;
        date.CustomFormat = "dd/MM/yyyy HH:mm";
        date.Width = 220;
        header.Controls.Add(lblSupplier);
        header.Controls.Add(supplier);
        header.Controls.Add(date);
        gridLines.Name = "gridLines";
        gridLines.Dock = DockStyle.Fill;
        actions.Dock = DockStyle.Bottom;
        actions.Height = 65;
        actions.Padding = new Padding(0, 10, 0, 0);
        btnPost.Name = "btnPost";
        btnPost.Text = "Ghi sổ / Nhập kho";
        btnPost.Size = new Size(175, 40);
        btnRemove.Name = "btnRemove";
        btnRemove.Text = "Xóa dòng";
        btnRemove.Size = new Size(110, 40);
        btnClose.Text = "Đóng";
        btnClose.Size = new Size(95, 40);
        btnClose.DialogResult = DialogResult.Cancel;
        total.Name = "total";
        total.AutoSize = true;
        total.Padding = new Padding(12, 10, 0, 0);
        actions.Controls.Add(btnPost);
        actions.Controls.Add(btnRemove);
        actions.Controls.Add(btnClose);
        actions.Controls.Add(total);
        Controls.Add(gridLines);
        Controls.Add(actions);
        Controls.Add(header);
        CancelButton = btnClose;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        MinimumSize = new Size(900, 650);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(255, 247, 237);
        Padding = new Padding(20);
        StartPosition = FormStartPosition.CenterParent;
        Name = "frmPurchaseEdit";
        Text = "frmPurchaseEdit";
        ResumeLayout(false);
    }
}
