using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Quan_Li_Nha_Hang_Suon_Muoi.Database;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms;

public partial class frmSuaKhach : Form
{
    private readonly DBProcess db = new();
    private readonly KhachHang khach;
    public frmSuaKhach() : this(new KhachHang()) { }
    public frmSuaKhach(KhachHang duLieu)
    {
        khach = duLieu;
        InitializeComponent();
        // Designer chỉ dựng giao diện, không mở form con hoặc kết nối database.
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
        txtTen.Text = khach.HoTen;
        txtSdt.Text = khach.DienThoai;
        txtEmail.Text = khach.Email;
        btnLuu.Click += btnLuu_Click;
    }
    private void btnLuu_Click(object? sender, EventArgs e)
    {
        try
        {
            khach.HoTen = txtTen.Text;
            khach.DienThoai = txtSdt.Text;
            khach.Email = txtEmail.Text;
            LuuKhach(khach);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void LuuKhach(KhachHang khach)
    {
        PhienDangNhap.KiemTraQuyenQuanTri();
        if (string.IsNullOrWhiteSpace(khach.HoTen))
            throw new InvalidOperationException("Nhập họ tên khách hàng.");
        // Hai trường được để trống; nếu đã nhập thì phải đúng định dạng.
        khach.DienThoai = khach.DienThoai.Trim();
        khach.Email = khach.Email.Trim();
        if (khach.DienThoai.Length > 0 && !Regex.IsMatch(khach.DienThoai, @"\A0[0-9]{9}\z"))
            throw new InvalidOperationException("Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0. Ví dụ: 0912345678.");
        if (khach.Email.Length > 0)
        {
            bool emailHopLe = MailAddress.TryCreate(khach.Email, out var diaChi)
                && diaChi.Address == khach.Email
                && !diaChi.User.StartsWith('.')
                && !diaChi.User.EndsWith('.')
                && diaChi.Host.Contains('.')
                && !diaChi.Host.EndsWith('.')
                && diaChi.Host.Split('.').All(phan => phan.Length > 0
                    && phan[0] != '-' && phan[^1] != '-'
                    && phan.All(kyTu => char.IsAsciiLetterOrDigit(kyTu) || kyTu == '-'))
                && !khach.Email.Contains("..")
                && !khach.Email.Any(char.IsWhiteSpace);
            if (!emailHopLe)
                throw new InvalidOperationException("Email không đúng định dạng. Ví dụ: ten@example.com.");
        }
        bool themMoi = string.IsNullOrEmpty(khach.MaKhach);
        if (themMoi)
            khach.MaKhach = "KH" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        string sql = themMoi
            ? "INSERT dbo.Customers(Code,Name,Phone,Email,Status) VALUES(@ma,@ten,@sdt,@email,N'Hoạt động')"
            : "UPDATE dbo.Customers SET Name=@ten,Phone=@sdt,Email=@email WHERE Code=@ma";
        db.DataChange(sql, new SqlParameter("@ma", khach.MaKhach), new SqlParameter("@ten", khach.HoTen.Trim()),
            new SqlParameter("@sdt", khach.DienThoai.Trim()), new SqlParameter("@email", khach.Email.Trim()));
    }
}
