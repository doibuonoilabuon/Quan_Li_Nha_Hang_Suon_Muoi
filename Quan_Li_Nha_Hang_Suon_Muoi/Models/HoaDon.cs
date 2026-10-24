namespace Quan_Li_Nha_Hang_Suon_Muoi.Models;

public class ChiTietHoaDon
{
    public string MaMon
    {
        get;
        set;
    } = "";
    public string TenMon
    {
        get;
        set;
    } = "";
    public int SoLuong
    {
        get;
        set;
    } = 1;
    public decimal DonGia
    {
        get;
        set;
    }
    public decimal ThanhTien => SoLuong * DonGia;
}
public class HoaDon
{
    public int MaHoaDon
    {
        get;
        set;
    }
    public string MaDonCu
    {
        get;
        set;
    } = "";
    public string SoHoaDon => MaDonCu.Length > 0 ? MaDonCu : "HD" + MaHoaDon.ToString("D6");
    public DateTime NgayLap
    {
        get;
        set;
    } = DateTime.Now;
    public string MaKhachHang
    {
        get;
        set;
    } = "";
    public string TenKhachHang
    {
        get;
        set;
    } = "Khách lẻ";
    public string NguoiLap
    {
        get;
        set;
    } = "";
    public string TrangThai
    {
        get;
        set;
    } = "Chưa thanh toán";
    public string PhuongThuc
    {
        get;
        set;
    } = "Tiền mặt";
    public string GhiChu
    {
        get;
        set;
    } = "";
    public decimal TienGiam
    {
        get;
        set;
    }
    public decimal TienKhachDua
    {
        get;
        set;
    }
    public DateTime? NgayThanhToan
    {
        get;
        set;
    }
    public byte[] PhienBan
    {
        get;
        set;
    } = [];
    public List<ChiTietHoaDon> ChiTiet
    {
        get;
        set;
    } = [];
    public decimal TongTien => ChiTiet.Sum(dong => dong.ThanhTien);
    public decimal ThanhTien => TongTien - TienGiam;
    public decimal TienThua => PhuongThuc == "Tiền mặt" ? Math.Max(0, TienKhachDua - ThanhTien) : 0;
}
