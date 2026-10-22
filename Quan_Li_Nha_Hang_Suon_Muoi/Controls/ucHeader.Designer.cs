using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
partial class ucHeader
{
    private Label lblTitle = null!, lblSubtitle = null!, lblUser = null!;
    private Button btnLogout = null!;
    private void InitializeComponent()
    {
        lblTitle = new Label();
        lblSubtitle = new Label();
        btnLogout = new Button();
        lblUser = new Label();
        SuspendLayout();
        //
        // lblTitle
        //
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(100, 23);
        lblTitle.TabIndex = 0;
        //
        // lblSubtitle
        //
        lblSubtitle.Location = new Point(0, 0);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(100, 23);
        lblSubtitle.TabIndex = 1;
        //
        // btnLogout
        //
        btnLogout.Location = new Point(0, 0);
        btnLogout.Name = "btnLogout";
        btnLogout.Size = new Size(75, 23);
        btnLogout.TabIndex = 3;
        btnLogout.Click += LogoutClick;
        //
        // lblUser
        //
        lblUser.Location = new Point(0, 0);
        lblUser.Name = "lblUser";
        lblUser.Size = new Size(100, 23);
        lblUser.TabIndex = 2;
        //
        // ucHeader
        //
        BackColor = Color.White;
        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(lblUser);
        Controls.Add(btnLogout);
        Name = "ucHeader";
        Size = new Size(1428, 192);
        ResumeLayout(false);
    }
}
