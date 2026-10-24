using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;

// File .xlsx là một gói ZIP gồm các tài liệu XML.
// Dùng thư viện có sẵn của .NET, không cần cài Microsoft Excel trên máy.
public static class XuatExcel
{
    private static readonly XNamespace Ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private const string Rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    public static void Luu(DataTable table, string file, bool monBanChay,
        DateTime tuNgay, DateTime denNgay, string ky)
    {
        string[] cot = monBanChay
            ? ["TenMon", "SoLuong", "TienMon"]
            : ["Ky", "SoHoaDon", "TongTien", "TienGiam", "DoanhThu"];
        bool theoThang = ky == "Theo tháng";
        string[] tieuDe = monBanChay
            ? ["STT", "Món ăn", "Số lượng", "Tiền món (VNĐ)"]
            : ["STT", theoThang ? "Tháng" : "Ngày", "Số hóa đơn", "Tiền hàng (VNĐ)", "Giảm giá (VNĐ)", "Doanh thu (VNĐ)"];
        if (cot.Any(c => !table.Columns.Contains(c)))
            throw new InvalidOperationException("Dữ liệu báo cáo chưa đầy đủ. Hãy bấm Xem báo cáo trước khi xuất.");
        if (table.Rows.Count > 1048566)
            throw new InvalidOperationException("Báo cáo quá lớn. Hãy chọn khoảng ngày ngắn hơn.");

        int cuoiCot = tieuDe.Length + 1;
        int cuoiDuLieu = 8 + table.Rows.Count;
        int dongTong = 9 + Math.Max(1, table.Rows.Count);
        string tenSheet = monBanChay ? "Món bán chạy" : "Doanh thu";
        var data = new XElement(Ns + "sheetData");
        var gopO = new List<string>();
        void TieuDe(int dong, string text, int style, int cao)
        {
            data.Add(Hang(dong, cao, O(dong, 2, text, style)));
            gopO.Add($"B{dong}:{ChuCot(cuoiCot)}{dong}");
        }
        TieuDe(2, "SƯỜN MƯỜI", 1, 30);
        TieuDe(3, monBanChay ? "BÁO CÁO MÓN BÁN CHẠY" : "BÁO CÁO DOANH THU", 2, 27);
        TieuDe(4, $"Từ {tuNgay:dd/MM/yyyy} đến {denNgay:dd/MM/yyyy}" + (monBanChay ? "" : $" · {ky}"), 3, 22);
        TieuDe(5, monBanChay
            ? "Hóa đơn đã thanh toán. Tiền món trước giảm giá. Đơn vị: VNĐ."
            : "Chỉ tính hóa đơn đã thanh toán. Đơn vị: VNĐ.", 3, 22);
        TieuDe(6, $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}", 3, 22);
        data.Add(Hang(8, 32, tieuDe.Select((t, i) => O(8, i + 2, t, 4)).ToArray()));

        // Ghi số và ngày dưới dạng dữ liệu Excel, không biến thành chuỗi hiển thị.
        for (int i = 0; i < table.Rows.Count; i++)
        {
            int dong = 9 + i;
            bool le = i % 2 == 1;
            var cells = new List<XElement> { O(dong, 2, i + 1, le ? 8 : 6) };
            for (int j = 0; j < cot.Length; j++)
            {
                object value = table.Rows[i][cot[j]];
                int style = j == 0 ? (le ? 7 : 5) : (le ? 8 : 6);
                if (!monBanChay && j == 0)
                {
                    value = DateTime.ParseExact(Convert.ToString(value, Culture)!, theoThang ? "yyyy-MM" : "yyyy-MM-dd", Culture);
                    style = theoThang ? (le ? 10 : 9) : (le ? 14 : 13);
                }
                cells.Add(O(dong, j + 3, value, style));
            }
            data.Add(Hang(dong, monBanChay ? 30 : 24, cells.ToArray()));
        }
        if (table.Rows.Count == 0)
        {
            data.Add(Hang(9, 28, O(9, 2, "Không có dữ liệu trong khoảng thời gian đã chọn.", 3)));
            gopO.Add($"B9:{ChuCot(cuoiCot)}9");
        }
        var tong = new List<XElement> { O(dongTong, 2, "TỔNG CỘNG", 11), O(dongTong, 3, "", 11) };
        for (int j = 1; j < cot.Length; j++)
        {
            int col = j + 3;
            decimal sum = table.AsEnumerable().Sum(r => Convert.ToDecimal(r[cot[j]], Culture));
            var cell = O(dongTong, col, sum, 12);
            // Lưu cả công thức và kết quả để xem được ngay, kể cả khi chưa tính lại.
            if (table.Rows.Count > 0)
                cell.AddFirst(new XElement(Ns + "f", $"SUM({ChuCot(col)}9:{ChuCot(col)}{cuoiDuLieu})"));
            tong.Add(cell);
        }
        data.Add(Hang(dongTong, 29, tong.ToArray()));
        gopO.Add($"B{dongTong}:C{dongTong}");

        double[] widths = monBanChay ? [3, 7, 48, 16, 25] : [3, 7, 18, 17, 24, 23, 25];
        var sheet = new XElement(Ns + "worksheet",
            new XElement(Ns + "sheetPr", new XElement(Ns + "tabColor", new XAttribute("rgb", "FFA83F2B")),
                new XElement(Ns + "pageSetUpPr", new XAttribute("fitToPage", 1))),
            new XElement(Ns + "dimension", new XAttribute("ref", $"A1:{ChuCot(cuoiCot)}{dongTong}")),
            new XElement(Ns + "sheetViews", new XElement(Ns + "sheetView", new XAttribute("workbookViewId", 0), new XAttribute("showGridLines", 0),
                new XElement(Ns + "pane", new XAttribute("ySplit", 8), new XAttribute("topLeftCell", "B9"), new XAttribute("activePane", "bottomLeft"), new XAttribute("state", "frozen")),
                new XElement(Ns + "selection", new XAttribute("pane", "bottomLeft"), new XAttribute("activeCell", "B9"), new XAttribute("sqref", "B9")))),
            new XElement(Ns + "sheetFormatPr", new XAttribute("defaultRowHeight", 22)),
            new XElement(Ns + "cols", widths.Select((w, i) => new XElement(Ns + "col", new XAttribute("min", i + 1), new XAttribute("max", i + 1), new XAttribute("width", w), new XAttribute("customWidth", 1)))),
            data);
        if (table.Rows.Count > 0)
            sheet.Add(new XElement(Ns + "autoFilter", new XAttribute("ref", $"B8:{ChuCot(cuoiCot)}{cuoiDuLieu}")));
        sheet.Add(new XElement(Ns + "mergeCells", new XAttribute("count", gopO.Count), gopO.Select(r => new XElement(Ns + "mergeCell", new XAttribute("ref", r)))),
            new XElement(Ns + "pageMargins", new XAttribute("left", 0.3), new XAttribute("right", 0.3), new XAttribute("top", 0.4), new XAttribute("bottom", 0.4), new XAttribute("header", 0.2), new XAttribute("footer", 0.2)),
            new XElement(Ns + "pageSetup", new XAttribute("paperSize", 9), new XAttribute("orientation", monBanChay ? "portrait" : "landscape"), new XAttribute("fitToWidth", 1), new XAttribute("fitToHeight", 0)),
            new XElement(Ns + "headerFooter", new XElement(Ns + "oddFooter", "&LSƯỜN MƯỜI&CTrang &P / &N")));

      
        string temp = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(file))!, Guid.NewGuid() + ".tmp");
        try
        {
            using (var zip = ZipFile.Open(temp, ZipArchiveMode.Create))
            {
                Ghi(zip, "[Content_Types].xml", XDocument.Parse(ContentTypes));
                Ghi(zip, "_rels/.rels", XDocument.Parse(RootRels));
                XNamespace rel = Rel;
                var workbook = new XElement(Ns + "workbook", new XAttribute(XNamespace.Xmlns + "r", Rel),
                    new XElement(Ns + "bookViews", new XElement(Ns + "workbookView")),
                    new XElement(Ns + "sheets", new XElement(Ns + "sheet", new XAttribute("name", tenSheet), new XAttribute("sheetId", 1), new XAttribute(rel + "id", "rId1"))),
                    new XElement(Ns + "definedNames",
                        new XElement(Ns + "definedName", new XAttribute("name", "_xlnm.Print_Area"), new XAttribute("localSheetId", 0), $"'{tenSheet}'!$B$2:${ChuCot(cuoiCot)}${dongTong}"),
                        new XElement(Ns + "definedName", new XAttribute("name", "_xlnm.Print_Titles"), new XAttribute("localSheetId", 0), $"'{tenSheet}'!$8:$8")),
                    new XElement(Ns + "calcPr", new XAttribute("calcId", 191029), new XAttribute("fullCalcOnLoad", 1)));
                Ghi(zip, "xl/workbook.xml", new XDocument(workbook));
                Ghi(zip, "xl/_rels/workbook.xml.rels", XDocument.Parse(WorkbookRels));
                Ghi(zip, "xl/styles.xml", XDocument.Parse(Styles));
                Ghi(zip, "xl/worksheets/sheet1.xml", new XDocument(sheet));
            }
            File.Move(temp, file, true);
        }
        finally
        {
            if (File.Exists(temp)) File.Delete(temp);
        }
    }

    private static XElement Hang(int row, int height, params XElement[] cells) =>
        new(Ns + "row", new XAttribute("r", row), new XAttribute("ht", height), new XAttribute("customHeight", 1), cells);

    private static XElement O(int row, int col, object value, int style)
    {
        var cell = new XElement(Ns + "c", new XAttribute("r", ChuCot(col) + row), new XAttribute("s", style));
        if (value is DateTime date)
            cell.Add(new XElement(Ns + "v", date.ToOADate().ToString(Culture)));
        else if (value is decimal or int or long or double)
            cell.Add(new XElement(Ns + "v", Convert.ToString(value, Culture)));
        else
        {
            
            cell.Add(new XAttribute("t", "inlineStr"), new XElement(Ns + "is", new XElement(Ns + "t", new XAttribute(XNamespace.Xml + "space", "preserve"), Convert.ToString(value, Culture))));
        }
        return cell;
    }

    private static string ChuCot(int col) => ((char)('A' + col - 1)).ToString(); 
    private static void Ghi(ZipArchive zip, string path, XDocument xml)
    {
        using var writer = new StreamWriter(zip.CreateEntry(path).Open(), new UTF8Encoding(false));
        xml.Save(writer);
    }

    private const string ContentTypes = """
        <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
          <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
          <Default Extension="xml" ContentType="application/xml"/>
          <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
          <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
          <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
        </Types>
        """;
    private const string RootRels = """
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
          <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
        </Relationships>
        """;
    private const string WorkbookRels = """
        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
          <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
          <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
        </Relationships>
        """;
 
    private const string Styles = """
        <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
          <numFmts count="2"><numFmt numFmtId="164" formatCode="dd/mm/yyyy"/><numFmt numFmtId="165" formatCode="mm/yyyy"/></numFmts>
          <fonts count="6">
            <font><sz val="11"/><color rgb="FF24201D"/><name val="Arial"/></font>
            <font><b/><sz val="17"/><color rgb="FFA83F2B"/><name val="Arial"/></font>
            <font><b/><sz val="14"/><color rgb="FF24201D"/><name val="Arial"/></font>
            <font><i/><sz val="10"/><color rgb="FF72665D"/><name val="Arial"/></font>
            <font><b/><sz val="11"/><color rgb="FFFFFFFF"/><name val="Arial"/></font>
            <font><b/><sz val="11"/><color rgb="FF85301F"/><name val="Arial"/></font>
          </fonts>
          <fills count="5">
            <fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFA83F2B"/><bgColor indexed="64"/></patternFill></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFFFF7ED"/><bgColor indexed="64"/></patternFill></fill>
            <fill><patternFill patternType="solid"><fgColor rgb="FFF5EBDD"/><bgColor indexed="64"/></patternFill></fill>
          </fills>
          <borders count="3">
            <border><left/><right/><top/><bottom/><diagonal/></border>
            <border><left/><right style="thin"><color rgb="FFFFFFFF"/></right><top/><bottom/><diagonal/></border>
            <border><left/><right/><top style="thin"><color rgb="FFA83F2B"/></top><bottom/><diagonal/></border>
          </borders>
          <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
          <cellXfs count="15">
            <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"><alignment vertical="center"/></xf>
            <xf numFmtId="0" fontId="1" fillId="0" borderId="0" xfId="0" applyFont="1"><alignment vertical="center"/></xf>
            <xf numFmtId="0" fontId="2" fillId="0" borderId="0" xfId="0" applyFont="1"><alignment vertical="center"/></xf>
            <xf numFmtId="0" fontId="3" fillId="0" borderId="0" xfId="0" applyFont="1"><alignment vertical="center"/></xf>
            <xf numFmtId="0" fontId="4" fillId="2" borderId="1" xfId="0" applyFont="1" applyFill="1" applyBorder="1"><alignment horizontal="center" vertical="center" wrapText="1"/></xf>
            <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"><alignment horizontal="left" vertical="center" wrapText="1" indent="1"/></xf>
            <xf numFmtId="3" fontId="0" fillId="0" borderId="0" xfId="0" applyNumberFormat="1"><alignment horizontal="right" vertical="center" indent="1"/></xf>
            <xf numFmtId="0" fontId="0" fillId="3" borderId="0" xfId="0" applyFill="1"><alignment horizontal="left" vertical="center" wrapText="1" indent="1"/></xf>
            <xf numFmtId="3" fontId="0" fillId="3" borderId="0" xfId="0" applyFill="1" applyNumberFormat="1"><alignment horizontal="right" vertical="center" indent="1"/></xf>
            <xf numFmtId="165" fontId="0" fillId="0" borderId="0" xfId="0" applyNumberFormat="1"><alignment horizontal="center" vertical="center"/></xf>
            <xf numFmtId="165" fontId="0" fillId="3" borderId="0" xfId="0" applyFill="1" applyNumberFormat="1"><alignment horizontal="center" vertical="center"/></xf>
            <xf numFmtId="0" fontId="5" fillId="4" borderId="2" xfId="0" applyFont="1" applyFill="1" applyBorder="1"><alignment vertical="center" indent="1"/></xf>
            <xf numFmtId="3" fontId="5" fillId="4" borderId="2" xfId="0" applyFont="1" applyFill="1" applyBorder="1" applyNumberFormat="1"><alignment horizontal="right" vertical="center" indent="1"/></xf>
            <xf numFmtId="164" fontId="0" fillId="0" borderId="0" xfId="0" applyNumberFormat="1"><alignment horizontal="center" vertical="center"/></xf>
            <xf numFmtId="164" fontId="0" fillId="3" borderId="0" xfId="0" applyFill="1" applyNumberFormat="1"><alignment horizontal="center" vertical="center"/></xf>
          </cellXfs>
          <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
        </styleSheet>
        """;
}
