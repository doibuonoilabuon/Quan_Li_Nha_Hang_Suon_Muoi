#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmRecipe
{
    private System.ComponentModel.IContainer components;
    private Label heading;
    private DataGridView gridLines;
    private TextBox instructions;
    private FlowLayoutPanel actions;
    private Button btnSave;
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
        heading = new Label();
        gridLines = new DataGridView();
        instructions = new TextBox();
        actions = new FlowLayoutPanel();
        btnSave = new Button();
        btnRemove = new Button();
        btnClose = new Button();
        SuspendLayout();
        heading.Name = "heading";
        heading.Dock = DockStyle.Top;
        heading.Height = 55;
        heading.Text = "Công thức / Pha chế • Định lượng theo đơn vị cơ sở trong kho";
        gridLines.Name = "gridLines";
        gridLines.Dock = DockStyle.Fill;
        instructions.Name = "instructions";
        instructions.Dock = DockStyle.Bottom;
        instructions.Height = 140;
        instructions.Multiline = true;
        instructions.ScrollBars = ScrollBars.Vertical;
        instructions.PlaceholderText = "Các bước chế biến / pha chế...";
        actions.Dock = DockStyle.Bottom;
        actions.Height = 58;
        actions.Padding = new Padding(0, 10, 0, 0);
        btnSave.Name = "btnSave";
        btnSave.Text = "Lưu công thức";
        btnSave.Size = new Size(150, 38);
        btnRemove.Name = "btnRemove";
        btnRemove.Text = "Xóa dòng";
        btnRemove.Size = new Size(120, 38);
        btnClose.Text = "Đóng";
        btnClose.Size = new Size(100, 38);
        btnClose.DialogResult = DialogResult.Cancel;
        actions.Controls.Add(btnSave);
        actions.Controls.Add(btnRemove);
        actions.Controls.Add(btnClose);
        Controls.Add(gridLines);
        Controls.Add(instructions);
        Controls.Add(actions);
        Controls.Add(heading);
        CancelButton = btnClose;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        MinimumSize = new Size(900, 650);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(255, 247, 237);
        Padding = new Padding(20);
        StartPosition = FormStartPosition.CenterParent;
        Name = "frmRecipe";
        Text = "frmRecipe";
        ResumeLayout(false);
    }
}
