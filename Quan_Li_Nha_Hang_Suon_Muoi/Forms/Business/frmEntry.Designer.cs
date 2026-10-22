#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmEntry
{
    private System.ComponentModel.IContainer components;
    private FlowLayoutPanel fields;
    private FlowLayoutPanel footer;
    private Button btnSave;
    private Button btnCancel;
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
        fields = new FlowLayoutPanel();
        footer = new FlowLayoutPanel();
        btnSave = new Button();
        btnCancel = new Button();
        SuspendLayout();
        fields.Name = "fields";
        fields.Dock = DockStyle.Fill;
        fields.AutoScroll = true;
        fields.FlowDirection = FlowDirection.TopDown;
        fields.WrapContents = false;
        fields.Padding = new Padding(20);
        footer.Dock = DockStyle.Bottom;
        footer.Height = 62;
        footer.Padding = new Padding(20, 8, 0, 0);
        btnSave.Text = "Lưu";
        btnSave.Size = new Size(110, 38);
        btnSave.BackColor = Color.FromArgb(168, 63, 43);
        btnSave.ForeColor = Color.White;
        btnCancel.Text = "Hủy";
        btnCancel.Size = new Size(110, 38);
        btnCancel.DialogResult = DialogResult.Cancel;
        footer.Controls.Add(btnSave);
        footer.Controls.Add(btnCancel);
        Controls.Add(fields);
        Controls.Add(footer);
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(600, 650);
        MinimumSize = new Size(600, 450);
        StartPosition = FormStartPosition.CenterParent;
        Name = "frmEntry";
        Text = "Thông tin";
        AcceptButton = btnSave;
        CancelButton = btnCancel;
        ResumeLayout(false);
    }
}
