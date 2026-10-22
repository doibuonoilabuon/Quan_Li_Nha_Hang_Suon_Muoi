using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
partial class ucStatCard
{
    private Label lblTitle = null!, lblValue = null!;
    private void InitializeComponent()
    {
        Size = new Size(250, 115);
        BackColor = Color.White;
        lblTitle = new Label
        {
            Dock = DockStyle.Top,
            Height = 42,
            Text = "Thống kê",
            TextAlign = ContentAlignment.MiddleCenter
        };
        lblValue = new Label
        {
            Dock = DockStyle.Fill,
            Text = "0",
            Font = new Font("Segoe UI", 23, FontStyle.Bold),
            ForeColor = Color.FromArgb(168, 63, 43),
            TextAlign = ContentAlignment.MiddleCenter
        };
        Controls.Add(lblValue);
        Controls.Add(lblTitle);
    }
}
