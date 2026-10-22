#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi;
partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private Quan_Li_Nha_Hang_Suon_Muoi.Controls.ucSidebar sidebar;
    private Quan_Li_Nha_Hang_Suon_Muoi.Controls.ucHeader header;
    private Panel contentHost;
    private Panel mainArea;
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
        sidebar = new Quan_Li_Nha_Hang_Suon_Muoi.Controls.ucSidebar();
        header = new Quan_Li_Nha_Hang_Suon_Muoi.Controls.ucHeader();
        contentHost = new Panel();
        mainArea = new Panel();
        mainArea.SuspendLayout();
        SuspendLayout();
        //
        // sidebar
        //
        sidebar.BackColor = Color.FromArgb(36, 32, 29);
        sidebar.Dock = DockStyle.Left;
        sidebar.Location = new Point(0, 0);
        sidebar.Name = "sidebar";
        sidebar.Size = new Size(250, 900);
        sidebar.TabIndex = 0;
        sidebar.Load += sidebar_Load;
        //
        // header
        //
        header.BackColor = Color.White;
        header.Dock = DockStyle.Top;
        header.Location = new Point(0, 0);
        header.Name = "header";
        header.Size = new Size(1190, 82);
        header.TabIndex = 0;
        //
        // contentHost
        //
        contentHost.BackColor = Color.FromArgb(255, 247, 237);
        contentHost.Dock = DockStyle.Fill;
        contentHost.Location = new Point(0, 82);
        contentHost.Name = "contentHost";
        contentHost.Size = new Size(1190, 818);
        contentHost.TabIndex = 1;
        //
        // mainArea
        //
        mainArea.Controls.Add(contentHost);
        mainArea.Controls.Add(header);
        mainArea.Dock = DockStyle.Fill;
        mainArea.Location = new Point(250, 0);
        mainArea.Name = "mainArea";
        mainArea.Size = new Size(1190, 900);
        mainArea.TabIndex = 1;
        //
        // Form1
        //
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(1440, 900);
        Controls.Add(mainArea);
        Controls.Add(sidebar);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1080, 800);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SƯỜN MƯỜI | Quản trị nhà hàng";
        WindowState = FormWindowState.Maximized;
        mainArea.ResumeLayout(false);
        ResumeLayout(false);
    }
}
