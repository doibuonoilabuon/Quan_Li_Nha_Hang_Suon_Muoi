namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;
partial class frmChinh
{
    private FlowLayoutPanel menu = null!;
    private Label lblNguoi = null!;
    private Button btnMonAn = null!;
    private Button btnKhachHang = null!;
    private Button btnHoaDon = null!;
    private Button btnBaoCao = null!;
    private Button btnDangXuat = null!;
    private Panel noiDung = null!;
    private void InitializeComponent()
    {
        menu = new FlowLayoutPanel();
        menu.Name = "menu";
        lblNguoi = new Label();
        lblNguoi.Name = "lblNguoi";
        btnMonAn = new Button();
        btnMonAn.Name = "btnMonAn";
        btnKhachHang = new Button();
        btnKhachHang.Name = "btnKhachHang";
        btnHoaDon = new Button();
        btnHoaDon.Name = "btnHoaDon";
        btnBaoCao = new Button();
        btnBaoCao.Name = "btnBaoCao";
        btnDangXuat = new Button();
        btnDangXuat.Name = "btnDangXuat";
        noiDung = new Panel();
        noiDung.Name = "noiDung";
        SuspendLayout();
        menu.Dock=DockStyle.Top;
        menu.Height=68;
        menu.Padding=new Padding(12);
        menu.BackColor=Color.FromArgb(36,32,29);
        lblNguoi.AutoSize=true;
        lblNguoi.Padding=new Padding(8,10,18,0);
        lblNguoi.ForeColor=Color.White;
        btnMonAn.Text="Món ăn";
        btnKhachHang.Text="Khách hàng";
        btnHoaDon.Text="Hóa đơn";
        btnBaoCao.Text="Báo cáo thống kê";
        btnDangXuat.Text="Đăng xuất";
        btnMonAn.Size=new Size(120,38);
        btnKhachHang.Size=new Size(120,38);
        btnHoaDon.Size=new Size(120,38);
        btnBaoCao.Size=new Size(165,38);
        btnDangXuat.Size=new Size(110,38);
        btnMonAn.BackColor=Color.White;
        btnKhachHang.BackColor=Color.White;
        btnHoaDon.BackColor=Color.White;
        btnBaoCao.BackColor=Color.White;
        btnDangXuat.BackColor=Color.White;
        menu.Controls.AddRange(new Control[] { lblNguoi,btnMonAn,btnKhachHang,btnHoaDon,btnBaoCao,btnDangXuat });
        noiDung.Dock=DockStyle.Fill;
        WindowState=FormWindowState.Maximized;
        Controls.Add(noiDung);
        Controls.Add(menu);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI",10F);
        BackColor = Color.FromArgb(255,247,237);
        ClientSize = new Size(1250,800);
        MinimumSize = new Size(950,620);
        StartPosition = FormStartPosition.CenterParent;
        Text = "SƯỜN MƯỜI - Quản lý bán hàng";
        Name = "frmChinh";
        ResumeLayout(false);
        PerformLayout();
    }
}
