#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmHelp
{
    private System.ComponentModel.IContainer components;
    private TextBox instructions;
    private FlowLayoutPanel actions;
    private Button btnBackup;
    private Button btnFolder;
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
        instructions = new TextBox();
        actions = new FlowLayoutPanel();
        btnBackup = new Button();
        btnFolder = new Button();
        SuspendLayout();
        instructions.Name = "instructions";
        instructions.Dock = DockStyle.Fill;
        instructions.Multiline = true;
        instructions.ReadOnly = true;
        instructions.ScrollBars = ScrollBars.Vertical;
        instructions.BackColor = Color.White;
        actions.Dock = DockStyle.Bottom;
        actions.Height = 60;
        actions.Padding = new Padding(0, 10, 0, 0);
        btnBackup.Name = "btnBackup";
        btnBackup.Text = "Sao lưu ZIP";
        btnBackup.Size = new Size(150, 40);
        btnFolder.Name = "btnFolder";
        btnFolder.Text = "Mở thư mục dữ liệu";
        btnFolder.Size = new Size(200, 40);
        actions.Controls.Add(btnBackup);
        actions.Controls.Add(btnFolder);
        Controls.Add(instructions);
        Controls.Add(actions);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        MinimumSize = new Size(900, 650);
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(255, 247, 237);
        Padding = new Padding(20);
        StartPosition = FormStartPosition.CenterParent;
        Name = "frmHelp";
        Text = "frmHelp";
        ResumeLayout(false);
    }
}
