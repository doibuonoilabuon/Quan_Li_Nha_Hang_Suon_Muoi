using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;


public static class XuatExcel
{
    public static void Luu(DataTable table, string file, bool monBanChay,
        DateTime tuNgay, DateTime denNgay, string ky, bool moSauKhiLuu = true)
    {
        string[] cot = monBanChay ? ["TenMon", "SoLuong", "TienMon"] : ["Ky", "SoHoaDon", "TongTien", "TienGiam", "DoanhThu"];
        bool theoThang = ky == "Theo tháng";
        string[] tenCot = monBanChay ? ["STT", "Tên món ăn", "Số lượng", "Tiền món (đồng)"]
            : ["STT", theoThang ? "Tháng" : "Ngày", "Số hóa đơn", "Tiền hàng (đồng)", "Giảm giá (đồng)", "Doanh thu (đồng)"];
        if (cot.Any(c => !table.Columns.Contains(c))) throw new InvalidOperationException("Hãy bấm Xem báo cáo trước khi xuất Excel.");
        if (table.Rows.Count > 1048500) throw new InvalidOperationException("Hãy chọn khoảng ngày ngắn hơn.");
        TaoTep(file, monBanChay ? "Món bán chạy" : "Doanh thu", 8, moSauKhiLuu, sheet =>
        {
            int cuoiCot = tenCot.Length;
            int dongTong = 9 + Math.Max(1, table.Rows.Count);
            KhungDau(sheet, cuoiCot, dongTong + 8, monBanChay ? "BÁO CÁO MÓN BÁN CHẠY" : "BÁO CÁO DOANH THU");
            GopChu(sheet, $"A5:{Cot(cuoiCot)}5", $"Từ ngày {tuNgay:dd/MM/yyyy} đến ngày {denNgay:dd/MM/yyyy}" + (monBanChay ? "" : $" ({ky.ToLower()})"));
            GopChu(sheet, $"A6:{Cot(cuoiCot)}6", monBanChay ? "Chỉ tính hóa đơn đã thanh toán. Tiền món trước giảm giá." : "Chỉ tính hóa đơn đã thanh toán.");
            TieuDeBang(sheet, 8, tenCot);
            if (table.Rows.Count > 0)
            {
                var values = new object[table.Rows.Count, tenCot.Length];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    values[i, 0] = i + 1;
                    for (int j = 0; j < cot.Length; j++)
                    {
                        object value = table.Rows[i][cot[j]];
                        if (!monBanChay && j == 0)
                            value = DateTime.ParseExact(Convert.ToString(value)!, theoThang ? "yyyy-MM" : "yyyy-MM-dd", CultureInfo.InvariantCulture).ToOADate();
                        values[i, j + 1] = value is string text ? VanBan(text) : value;
                    }
                }
                Vung(sheet, $"A9:{Cot(cuoiCot)}{8 + table.Rows.Count}", r => r.Value2 = values);
                Vung(sheet, $"A8:{Cot(cuoiCot)}{8 + table.Rows.Count}", r => r.AutoFilter());
                if (!monBanChay) Vung(sheet, $"B9:B{8 + table.Rows.Count}", r => r.NumberFormat = theoThang ? "mm/yyyy" : "dd/mm/yyyy");
            }
            else GopChu(sheet, $"A9:{Cot(cuoiCot)}9", "Không có dữ liệu trong khoảng thời gian đã chọn.");
            GopChu(sheet, $"A{dongTong}:B{dongTong}", "TỔNG CỘNG", true);
            for (int col = 3; col <= cuoiCot; col++)
                Vung(sheet, $"{Cot(col)}{dongTong}", r =>
                {
                    if (table.Rows.Count == 0) r.Value2 = 0;
                    else r.Formula = $"=SUM({Cot(col)}9:{Cot(col)}{8 + table.Rows.Count})";
                });
            DinhDangChu(sheet, $"A{dongTong}:{Cot(cuoiCot)}{dongTong}", true);
            KeBang(sheet, $"A8:{Cot(cuoiCot)}{dongTong}");
            Vung(sheet, $"C9:{Cot(cuoiCot)}{dongTong}", r => { r.NumberFormat = "#,##0"; r.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; });
            Vung(sheet, $"A9:A{dongTong - 1}", r => r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter);
            if (!monBanChay && table.Rows.Count > 0)
                Vung(sheet, $"B9:B{8 + table.Rows.Count}", r => r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter);
            double[] rong = monBanChay ? [7, 40, 17, 25] : [7, 19, 18, 25, 23, 26];
            for (int i = 0; i < rong.Length; i++) Vung(sheet, $"{Cot(i + 1)}:{Cot(i + 1)}", r => r.ColumnWidth = rong[i]);
            if (monBanChay && table.Rows.Count > 0) CaoDongDuLieu(sheet, $"B9:B{8 + table.Rows.Count}");
            int giua = monBanChay ? 2 : 3;
            GopChu(sheet, $"A{dongTong + 3}:{Cot(giua)}{dongTong + 3}", "Người lập báo cáo", true);
            GopChu(sheet, $"{Cot(giua + 1)}{dongTong + 3}:{Cot(cuoiCot)}{dongTong + 3}", "Quản lý nhà hàng", true);
            GopChu(sheet, $"A{dongTong + 4}:{Cot(giua)}{dongTong + 4}", "(Ký, ghi rõ họ tên)");
            GopChu(sheet, $"{Cot(giua + 1)}{dongTong + 4}:{Cot(cuoiCot)}{dongTong + 4}", "(Ký, ghi rõ họ tên)");
            Vung(sheet, $"A{dongTong + 3}:{Cot(cuoiCot)}{dongTong + 4}", r => r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter);
            TrangIn(sheet, cuoiCot, dongTong + 8, 8, !monBanChay);
        });
    }

    public static void LuuHoaDon(HoaDon hd, string file, bool moSauKhiLuu = true)
    {
        if (hd.ChiTiet.Count == 0) throw new InvalidOperationException("Hóa đơn chưa có món ăn.");
        TaoTep(file, "Hóa đơn", 12, moSauKhiLuu, sheet =>
        {
            int tong = 13 + hd.ChiTiet.Count;
            KhungDau(sheet, 5, tong + 12, "HÓA ĐƠN BÁN HÀNG");
            GopChu(sheet, "A6:E6", $"Số hóa đơn: {hd.SoHoaDon}                  Ngày lập: {hd.NgayLap:dd/MM/yyyy HH:mm}");
            GopChu(sheet, "A7:E7", "Khách hàng: " + hd.TenKhachHang);
            GopChu(sheet, "A8:E8", "Người lập: " + hd.NguoiLap);
            GopChu(sheet, "A9:E9", "Phương thức thanh toán: " + hd.PhuongThuc);
            GopChu(sheet, "A10:E10", "Trạng thái: " + hd.TrangThai);
            TieuDeBang(sheet, 12, ["STT", "Tên món ăn", "Số lượng", "Đơn giá (đồng)", "Thành tiền (đồng)"]);
            var values = new object[hd.ChiTiet.Count, 5];
            for (int i = 0; i < hd.ChiTiet.Count; i++)
            {
                var dong = hd.ChiTiet[i];
                values[i, 0] = i + 1; values[i, 1] = VanBan(dong.TenMon);
                values[i, 2] = dong.SoLuong; values[i, 3] = dong.DonGia; values[i, 4] = dong.ThanhTien;
            }
            Vung(sheet, $"A13:E{tong - 1}", r => r.Value2 = values);
            KeBang(sheet, $"A12:E{tong - 1}");
            Vung(sheet, $"C13:E{tong + 4}", r => { r.NumberFormat = "#,##0"; r.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; });
            GopChu(sheet, $"A{tong}:C{tong}", "Tổng tiền hàng:", true);
            GopChu(sheet, $"D{tong}:E{tong}", "");
            Vung(sheet, $"D{tong}", r => r.Formula = $"=SUM(E13:E{tong - 1})");
            GopChu(sheet, $"A{tong + 1}:C{tong + 1}", "Giảm giá:");
            GopChu(sheet, $"D{tong + 1}:E{tong + 1}", "");
            Vung(sheet, $"D{tong + 1}", r => r.Value2 = hd.TienGiam);
            GopChu(sheet, $"A{tong + 2}:C{tong + 2}", "Thành tiền:", true);
            GopChu(sheet, $"D{tong + 2}:E{tong + 2}", "");
            Vung(sheet, $"D{tong + 2}", r => r.Formula = $"=D{tong}-D{tong + 1}");
            DinhDangChu(sheet, $"A{tong + 2}:E{tong + 2}", true, 14, Color.Red);
            GopChu(sheet, $"A{tong + 3}:C{tong + 3}", "Tiền khách đưa:");
            GopChu(sheet, $"D{tong + 3}:E{tong + 3}", "");
            Vung(sheet, $"D{tong + 3}", r => r.Value2 = hd.TienKhachDua);
            GopChu(sheet, $"A{tong + 4}:C{tong + 4}", "Tiền thừa:");
            GopChu(sheet, $"D{tong + 4}:E{tong + 4}", "");
            Vung(sheet, $"D{tong + 4}", r =>
            {
                if (hd.PhuongThuc == "Tiền mặt" && hd.TrangThai == "Đã thanh toán") r.Formula = $"=MAX(0,D{tong + 3}-D{tong + 2})";
                else r.Value2 = 0;
            });
            GopChu(sheet, $"A{tong + 6}:E{tong + 6}", "Ghi chú: " + hd.GhiChu);
            Vung(sheet, $"A{tong + 6}:E{tong + 6}", r => { r.WrapText = true; r.RowHeight = 36; });
            GopChu(sheet, $"A{tong + 8}:B{tong + 8}", "Khách hàng", true);
            GopChu(sheet, $"D{tong + 8}:E{tong + 8}", "Người lập hóa đơn", true);
            GopChu(sheet, $"A{tong + 9}:B{tong + 9}", "(Ký, ghi rõ họ tên)");
            GopChu(sheet, $"D{tong + 9}:E{tong + 9}", "(Ký, ghi rõ họ tên)");
            Vung(sheet, $"A{tong + 8}:E{tong + 9}", r => r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter);
            double[] rong = [7, 39, 15, 23, 25];
            for (int i = 0; i < rong.Length; i++) Vung(sheet, $"{Cot(i + 1)}:{Cot(i + 1)}", r => r.ColumnWidth = rong[i]);
            CaoDongDuLieu(sheet, $"B13:B{tong - 1}");
            TrangIn(sheet, 5, tong + 12, 12, false);
        });
    }


    private static void TaoTep(string file, string tenSheet, int dongTieuDe, bool moSauKhiLuu, Action<Excel.Worksheet> ghiDuLieu)
    {
        string dich = Path.GetFullPath(file);
        if (!dich.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Hãy lưu báo cáo với đuôi .xlsx.");
        string tam = Path.Combine(Path.GetDirectoryName(dich)!, Guid.NewGuid() + ".xlsx");
        Excel.Application? exApp = null; Excel.Workbooks? books = null;
        Excel.Workbook? book = null; Excel.Sheets? sheets = null;
        Excel.Worksheet? sheet = null; Excel.Window? window = null;
        try
        {
            try
            {
                try { exApp = new Excel.Application(); }
                catch (COMException ex) { throw new InvalidOperationException("Cần cài Microsoft Excel để xuất file theo cách trong video của cô.", ex); }
                exApp.Visible = false; exApp.DisplayAlerts = false; exApp.ScreenUpdating = false;
                books = exApp.Workbooks; book = books.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                sheets = book.Worksheets; sheet = (Excel.Worksheet)sheets[1]; sheet.Name = tenSheet;
                ghiDuLieu(sheet);
                window = exApp.ActiveWindow; window.DisplayGridlines = false;
                window.SplitRow = dongTieuDe; window.FreezePanes = true;
                exApp.CalculateFull();
                book.SaveAs(tam, Excel.XlFileFormat.xlOpenXMLWorkbook);
            }
            finally
            {
                try { if (book != null) book.Close(false); }
                catch (COMException) { }
                finally
                {
                    GiaiPhong(window); GiaiPhong(sheet); GiaiPhong(sheets); GiaiPhong(book); GiaiPhong(books);
                    try { if (exApp != null) exApp.Quit(); }
                    catch (COMException) { }
                    finally { GiaiPhong(exApp); }
                }
            }
            File.Move(tam, dich, true);
        }
        finally { if (File.Exists(tam)) File.Delete(tam); }
        if (moSauKhiLuu) Process.Start(new ProcessStartInfo(dich) { UseShellExecute = true });
    }

    private static string Cot(int so) => ((char)('A' + so - 1)).ToString();
    private static string VanBan(string text) => text.Length > 0 && "=+-@".Contains(text[0]) ? "'" + text : text;
    private static void GiaiPhong(object? obj) { if (obj != null && Marshal.IsComObject(obj)) Marshal.ReleaseComObject(obj); }

    private static void Vung(Excel.Worksheet sheet, string diaChi, Action<Excel.Range> thayDoi)
    {
        Excel.Range range = sheet.Range[diaChi];
        try { thayDoi(range); } finally { GiaiPhong(range); }
    }

    private static void GopChu(Excel.Worksheet sheet, string diaChi, string text, bool dam = false)
    {
        Vung(sheet, diaChi, r => { r.Merge(); r.Value2 = VanBan(text); });
        if (dam) DinhDangChu(sheet, diaChi, true);
    }

    private static void DinhDangChu(Excel.Worksheet sheet, string diaChi, bool dam = false, int co = 12, Color? mau = null)
    {
        Vung(sheet, diaChi, r =>
        {
            Excel.Font font = r.Font;
            try { font.Name = "Times New Roman"; font.Size = co; font.Bold = dam; font.Color = ColorTranslator.ToOle(mau ?? Color.Black); }
            finally { GiaiPhong(font); }
        });
    }

    private static void KeBang(Excel.Worksheet sheet, string diaChi)
    {
        Vung(sheet, diaChi, r =>
        {
            Excel.Borders borders = r.Borders;
            try { borders.LineStyle = Excel.XlLineStyle.xlContinuous; borders.Weight = Excel.XlBorderWeight.xlThin; }
            finally { GiaiPhong(borders); }
        });
    }

    private static void KhungDau(Excel.Worksheet sheet, int cotCuoi, int dongCuoi, string tieuDe)
    {
        string vung = $"A1:{Cot(cotCuoi)}{dongCuoi}";
        DinhDangChu(sheet, vung);
        Vung(sheet, vung, r => { r.RowHeight = 23; r.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter; });
        GopChu(sheet, "A1:C1", "NHÀ HÀNG SƯỜN MƯỜI", true);
        DinhDangChu(sheet, "A1:C1", true, 14, Color.Blue);
        GopChu(sheet, "A2:C2", "Đơn vị tính: Đồng");
        GopChu(sheet, $"A4:{Cot(cotCuoi)}4", tieuDe, true);
        DinhDangChu(sheet, $"A4:{Cot(cotCuoi)}4", true, 20, Color.Red);
        Vung(sheet, $"A4:{Cot(cotCuoi)}4", r => { r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; r.RowHeight = 34; });
    }

    private static void TieuDeBang(Excel.Worksheet sheet, int dong, string[] tenCot)
    {
        for (int i = 0; i < tenCot.Length; i++) Vung(sheet, Cot(i + 1) + dong, r => r.Value2 = tenCot[i]);
        DinhDangChu(sheet, $"A{dong}:{Cot(tenCot.Length)}{dong}", true);
        Vung(sheet, $"A{dong}:{Cot(tenCot.Length)}{dong}", r => { r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; r.WrapText = true; r.RowHeight = 34; });
    }

    private static void CaoDongDuLieu(Excel.Worksheet sheet, string diaChi)
    {
        Vung(sheet, diaChi, r =>
        {
            r.WrapText = true;
            Excel.Range rows = r.EntireRow;
            try
            {
                rows.AutoFit();
                for (int i = 1; i <= rows.Count; i++)
                {
                    Excel.Range row = rows[i];
                    try { if (Convert.ToDouble(row.RowHeight) < 23) row.RowHeight = 23; }
                    finally { GiaiPhong(row); }
                }
            }
            finally { GiaiPhong(rows); }
        });
    }

    private static void TrangIn(Excel.Worksheet sheet, int cotCuoi, int dongCuoi, int dongTieuDe, bool ngang)
    {
        Excel.PageSetup trang = sheet.PageSetup;
        try
        {
            trang.PaperSize = Excel.XlPaperSize.xlPaperA4;
            trang.Orientation = ngang ? Excel.XlPageOrientation.xlLandscape : Excel.XlPageOrientation.xlPortrait;
            trang.Zoom = false; trang.FitToPagesWide = 1; trang.FitToPagesTall = false;
            trang.PrintArea = $"A1:{Cot(cotCuoi)}{dongCuoi}"; trang.PrintTitleRows = $"${dongTieuDe}:${dongTieuDe}";
            trang.LeftMargin = 21.6; trang.RightMargin = 21.6;
            trang.TopMargin = 28.8; trang.BottomMargin = 28.8;
            trang.CenterHorizontally = true; trang.CenterFooter = "Trang &P / &N";
        }
        finally { GiaiPhong(trang); }
    }
}
