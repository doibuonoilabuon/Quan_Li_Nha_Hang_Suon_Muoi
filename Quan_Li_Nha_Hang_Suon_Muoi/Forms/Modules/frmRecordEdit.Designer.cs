using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;
partial class frmRecordEdit
{
    private FlowLayoutPanel fields = null!;
    private Button btnSave = null!, btnCancel = null!;
    private Panel footer = null!;
    private void InitializeComponent()
    {
        Text = "Thông tin";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(510, 570);
        MinimumSize = new Size(500, 420);
        BackColor = Color.FromArgb(255, 247, 237);
        Font = new Font("Segoe UI", 10);
        fields = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(20)
        };
        footer = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 64
        };
        btnSave = new Button
        {
            Text = "Lưu",
            Location = new Point(260, 12),
            Size = new Size(100, 38),
            BackColor = Color.FromArgb(168, 63, 43),
            ForeColor = Color.White
        };
        btnSave.Click += SaveClick;
        btnCancel = new Button
        {
            Text = "Hủy",
            Location = new Point(370, 12),
            Size = new Size(100, 38),
            DialogResult = DialogResult.Cancel
        };
        footer.Controls.AddRange(new Control[] { btnSave, btnCancel });
        Controls.Add(fields);
        Controls.Add(footer);
        AcceptButton = btnSave;
        CancelButton = btnCancel;
    }
}
