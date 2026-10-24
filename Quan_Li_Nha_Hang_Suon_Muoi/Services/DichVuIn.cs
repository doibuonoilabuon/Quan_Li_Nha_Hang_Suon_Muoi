using System.Drawing.Printing;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;

public static class DichVuIn
{
    public static string Tien(decimal soTien) => soTien.ToString("N0") + " đ";
    public static void XemHoaDon(IWin32Window owner, HoaDon hd)
    {
        var dong = new List<string>{
            "SƯỜN MƯỜI",hd.TrangThai=="Đã thanh toán" ? "HÓA ĐƠN THANH TOÁN" : "PHIẾU TẠM TÍNH",
            "Số: "+hd.SoHoaDon,"Ngày: "+hd.NgayLap.ToString("dd/MM/yyyy HH:mm"),
            "Khách: "+hd.TenKhachHang,"Nhân viên: "+hd.NguoiLap,new string('-',55)};
        foreach (var ct in hd.ChiTiet)
            dong.Add(ct.TenMon + " x " + ct.SoLuong + " @ " + Tien(ct.DonGia) + " = " + Tien(ct.ThanhTien));
        dong.AddRange([new string('-',55),"Tổng tiền: "+Tien(hd.TongTien),"Giảm giá: "+Tien(hd.TienGiam),
            "PHẢI THU: "+Tien(hd.ThanhTien),"Trạng thái: "+hd.TrangThai]);
        if (hd.TrangThai == "Đã thanh toán")
            dong.AddRange(["Phương thức: " + hd.PhuongThuc, "Khách đưa: " + Tien(hd.TienKhachDua), "Tiền thừa: " + Tien(hd.TienThua)]);
        XemTruoc(owner, "Hóa đơn " + hd.SoHoaDon, dong);
    }

    public static void XemTruoc(IWin32Window owner, string tieuDe, List<string> cacDong)
    {
        using var document = new PrintDocument { DocumentName = tieuDe };
        document.DefaultPageSettings.Margins = new Margins(45, 45, 45, 45);
        using var font = new Font("Segoe UI", 10);
        int viTri = 0, offset = 0, soTrang = 0;
        document.BeginPrint += (_, _) =>
        {
            viTri = 0;
            offset = 0;
            soTrang = 0;
        };
        document.PrintPage += (_, e) =>
        {
            float y = e.MarginBounds.Top;
            while (viTri < cacDong.Count)
            {
                string text = cacDong[viTri][offset..];
                float conLai = e.MarginBounds.Bottom - 30 - y;
                if (conLai < font.GetHeight(e.Graphics!) + 8)
                    break;
                var size = e.Graphics!.MeasureString(text.Length == 0 ? " " : text, font,
                    new SizeF(e.MarginBounds.Width, conLai - 8), StringFormat.GenericDefault, out int kyTu, out int soDong);
                if (text.Length > 0 && kyTu == 0)
                    break;
                string phan = text.Length == 0 ? " " : text[..Math.Min(kyTu, text.Length)];
                e.Graphics!.DrawString(phan, font, Brushes.Black, new RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, size.Height));
                y += size.Height + 8;
                offset += kyTu;
                if (offset >= cacDong[viTri].Length)
                {
                    viTri++;
                    offset = 0;
                }
            }
            e.Graphics!.DrawString("Trang " + (++soTrang), font, Brushes.Gray, e.MarginBounds.Left, e.MarginBounds.Bottom - 20);
            e.HasMorePages = viTri < cacDong.Count;
        };
        using var preview = new PrintPreviewDialog { Document = document, Width = 1050, Height = 780, UseAntiAlias = true };
        preview.ShowDialog(owner);
    }
}
