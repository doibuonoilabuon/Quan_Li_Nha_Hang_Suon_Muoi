using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;
/// <summary>Exports the visible list to a real .xlsx package without requiring Excel or COM.</summary>
public static class ExcelExport
{
    private const string Main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private const string Relationships = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    private const string PackageRelationships = "http://schemas.openxmlformats.org/package/2006/relationships";
    public static void Show(IWin32Window owner, DataGridView grid, string title)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Xuất Excel • " + title,
            Filter = "Excel Workbook (*.xlsx)|*.xlsx",
            DefaultExt = "xlsx",
            AddExtension = true,
            OverwritePrompt = true,
            FileName = title + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
        };
        if (dialog.ShowDialog(owner) != DialogResult.OK)
        {
            return;
        }

        var count = SaveGrid(grid, dialog.FileName, title);
        MessageBox.Show(
            owner,
            $"Đã xuất {count} dòng đang hiển thị.\n{dialog.FileName}",
            "Xuất Excel",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    public static int SaveGrid(DataGridView grid, string file, string sheetName)
    {
        var columns = grid.Columns.Cast<DataGridViewColumn>().Where(x => x.Visible).OrderBy(x => x.DisplayIndex).ToList();
        var rows = grid.Rows.Cast<DataGridViewRow>().Where(x => !x.IsNewRow && x.Visible).ToList();
        if (columns.Count == 0)
        {
            throw new InvalidOperationException("Danh sách chưa có cột để xuất.");
        }

        if (columns.Count > 16384 || rows.Count > 1048575)
        {
            throw new InvalidOperationException("Danh sách vượt giới hạn một trang tính Excel.");
        }

        var destination = Path.GetFullPath(file);
        var temporary = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    WritePart(
                        zip,
                        "[Content_Types].xml",
                        """
                    <?xml version="1.0" encoding="utf-8"?>
                    <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
                      <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
                      <Default Extension="xml" ContentType="application/xml"/>
                      <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
                      <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
                      <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
                    </Types>
                    """
                    );
                    WriteXml(
                        zip,
                        "_rels/.rels",
                        x =>
                    {
                        x.WriteStartElement("Relationships", PackageRelationships);
                        Relation(x, "rId1", Relationships + "/officeDocument", "xl/workbook.xml");
                        x.WriteEndElement();
                    }
                    );
                    WriteXml(
                        zip,
                        "xl/_rels/workbook.xml.rels",
                        x =>
                    {
                        x.WriteStartElement("Relationships", PackageRelationships);
                        Relation(x, "rId1", Relationships + "/worksheet", "worksheets/sheet1.xml");
                        Relation(x, "rId2", Relationships + "/styles", "styles.xml");
                        x.WriteEndElement();
                    }
                    );
                    WriteXml(
                        zip,
                        "xl/workbook.xml",
                        x =>
                    {
                        x.WriteStartElement("workbook", Main);
                        x.WriteStartElement("sheets", Main);
                        x.WriteStartElement("sheet", Main);
                        x.WriteAttributeString("name", SheetName(sheetName));
                        x.WriteAttributeString("sheetId", "1");
                        x.WriteAttributeString("r", "id", Relationships, "rId1");
                        x.WriteEndElement();
                        x.WriteEndElement();
                        x.WriteEndElement();
                    }
                    );
                    WritePart(zip, "xl/styles.xml", Styles);
                    WriteXml(
                        zip,
                        "xl/worksheets/sheet1.xml",
                        x =>
                    {
                        x.WriteStartElement("worksheet", Main);
                        var range = "A1:" + ColumnName(columns.Count - 1) + (rows.Count + 1);
                        x.WriteStartElement("dimension", Main);
                        x.WriteAttributeString("ref", range);
                        x.WriteEndElement();
                        x.WriteStartElement("sheetViews", Main);
                        x.WriteStartElement("sheetView", Main);
                        x.WriteAttributeString("workbookViewId", "0");
                        x.WriteStartElement("pane", Main);
                        x.WriteAttributeString("ySplit", "1");
                        x.WriteAttributeString("topLeftCell", "A2");
                        x.WriteAttributeString("activePane", "bottomLeft");
                        x.WriteAttributeString("state", "frozen");
                        x.WriteEndElement();
                        x.WriteEndElement();
                        x.WriteEndElement();
                        x.WriteStartElement("sheetFormatPr", Main);
                        x.WriteAttributeString("defaultRowHeight", "21");
                        x.WriteEndElement();
                        x.WriteStartElement("cols", Main);
                        for (int c = 0; c < columns.Count; c++)
                        {
                            int length = Math.Max(
                                    Header(columns[c]).Length,
                                    rows.Take(200).Select(r => Convert.ToString(r.Cells[columns[c].Index].FormattedValue)?.Length ?? 0)
                                        .DefaultIfEmpty(0)
                                        .Max()
                                );
                            x.WriteStartElement("col", Main);
                            x.WriteAttributeString("min", (c + 1).ToString());
                            x.WriteAttributeString("max", (c + 1).ToString());
                            x.WriteAttributeString("width", Math.Clamp(length + 3, 16, 55).ToString(CultureInfo.InvariantCulture));
                            x.WriteAttributeString("customWidth", "1");
                            x.WriteEndElement();
                        }

                        x.WriteEndElement();
                        x.WriteStartElement("sheetData", Main);
                        x.WriteStartElement("row", Main);
                        x.WriteAttributeString("r", "1");
                        x.WriteAttributeString("ht", "27");
                        x.WriteAttributeString("customHeight", "1");
                        for (int c = 0; c < columns.Count; c++)
                        {
                            TextCell(x, ColumnName(c) + "1", Header(columns[c]), 1);
                        }

                        x.WriteEndElement();
                        for (int r = 0; r < rows.Count; r++)
                        {
                            x.WriteStartElement("row", Main);
                            x.WriteAttributeString("r", (r + 2).ToString());
                            for (int c = 0; c < columns.Count; c++)
                            {
                                Cell(x, ColumnName(c) + (r + 2), rows[r].Cells[columns[c].Index].Value);
                            }

                            x.WriteEndElement();
                        }

                        x.WriteEndElement();
                        x.WriteStartElement("autoFilter", Main);
                        x.WriteAttributeString("ref", range);
                        x.WriteEndElement();
                        x.WriteStartElement("pageMargins", Main);
                        x.WriteAttributeString("left", "0.3");
                        x.WriteAttributeString("right", "0.3");
                        x.WriteAttributeString("top", "0.5");
                        x.WriteAttributeString("bottom", "0.5");
                        x.WriteAttributeString("header", "0.2");
                        x.WriteAttributeString("footer", "0.2");
                        x.WriteEndElement();
                        x.WriteEndElement();
                    }
                    );
                }
            } // Keep an existing export intact if creating the new package fails.

            File.Move(temporary, destination, true);
            return rows.Count;
        }
        finally
        {
            if (File.Exists(temporary))
            {
                File.Delete(temporary);
            }
        }
    }

    private static string Header(DataGridViewColumn column)
    {
        return column.HeaderText.Replace('_', ' ');
    }

    private static string SheetName(string title)
    {
        var clean = new string(title.Where(c => !"[]:*?/\\".Contains(c) && !char.IsControl(c)).ToArray()).Trim()
            .Trim('\'');
        if (clean.Length == 0)
        {
            return "Danh sách";
        }

        return clean.Length > 31 ? clean[..31] : clean;
    }

    private static string ColumnName(int index)
    {
        string result = "";
        for (int n = index + 1; n > 0; n = (n - 1) / 26)
        {
            result = (char)('A' + (n - 1) % 26) + result;
        }

        return result;
    }

    private static string XmlText(string text)
    {
        var result = new StringBuilder();
        foreach (var rune in text.EnumerateRunes())
        {
            int n = rune.Value;
            if (!(n is 9 or 10 or 13 || n >= 32 && n <= 0xD7FF || n >= 0xE000 && n <= 0xFFFD
                || n >= 0x10000 && n <= 0x10FFFF))
            {
                continue;
            }

            if (result.Length + rune.Utf16SequenceLength > 32767)
            {
                break;
            }

            result.Append(rune.ToString());
        }

        return result.ToString();
    }

    private static void TextCell(XmlWriter x, string address, string value, int style = 0)
    {
        x.WriteStartElement("c", Main);
        x.WriteAttributeString("r", address);
        x.WriteAttributeString("t", "inlineStr");
        x.WriteAttributeString("s", style.ToString());
        x.WriteStartElement("is", Main);
        x.WriteStartElement("t", Main);
        x.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
        x.WriteString(XmlText(value));
        x.WriteEndElement();
        x.WriteEndElement();
        x.WriteEndElement();
    }

    private static void Cell(XmlWriter x, string address, object? value)
    {
        if (value is null or DBNull)
        {
            return;
        }

        if (value is DateTime date && date >= new DateTime(1900, 1, 1))
        {
            NumericCell(x, address, date.ToOADate().ToString("R", CultureInfo.InvariantCulture), 3);
            return;
        }

        if (value is bool flag)
        {
            x.WriteStartElement("c", Main);
            x.WriteAttributeString("r", address);
            x.WriteAttributeString("t", "b");
            x.WriteElementString("v", Main, flag ? "1" : "0");
            x.WriteEndElement();
            return;
        }

        if (value is byte
            or sbyte
            or short
            or ushort
            or int
            or uint
            or long
            or ulong
            or decimal
            or float
            or double)
        {
            if (value is double d && !double.IsFinite(d) || value is float f && !float.IsFinite(f))
            {
                TextCell(x, address, value.ToString() ?? "");
                return;
            }

            var number = Convert.ToString(value, CultureInfo.InvariantCulture)!;
            // Excel stores numeric values with 15 digits of precision.
            if (decimal.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out var exact)
                && (exact >= 1_000_000_000_000_000m || exact <= -1_000_000_000_000_000m))
            {
                TextCell(x, address, number);
            }
            else
            {
                NumericCell(x, address, number, 2);
            }

            return;
        }

        // A leading '=' remains text rather than an executable Excel formula.
        TextCell(x, address, Convert.ToString(value, CultureInfo.CurrentCulture) ?? "");
    }

    private static void NumericCell(XmlWriter x, string address, string value, int style)
    {
        x.WriteStartElement("c", Main);
        x.WriteAttributeString("r", address);
        x.WriteAttributeString("s", style.ToString());
        x.WriteAttributeString("t", "n");
        x.WriteElementString("v", Main, value);
        x.WriteEndElement();
    }

    private static void Relation(XmlWriter x, string id, string type, string target)
    {
        x.WriteStartElement("Relationship", PackageRelationships);
        x.WriteAttributeString("Id", id);
        x.WriteAttributeString("Type", type);
        x.WriteAttributeString("Target", target);
        x.WriteEndElement();
    }

    private static void WritePart(ZipArchive zip, string name, string content)
    {
        using var writer = new StreamWriter(zip.CreateEntry(name).Open(), new UTF8Encoding(false));
        writer.Write(content);
    }

    private static void WriteXml(ZipArchive zip, string name, Action<XmlWriter> write)
    {
        using var stream = zip.CreateEntry(name).Open();
        using var writer = XmlWriter.Create(stream, new XmlWriterSettings { Encoding = new UTF8Encoding(false) });
        writer.WriteStartDocument();
        write(writer);
        writer.WriteEndDocument();
    }

    private const string Styles = """
        <?xml version="1.0" encoding="utf-8"?>
        <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
          <numFmts count="2"><numFmt numFmtId="164" formatCode="dd/mm/yyyy hh:mm"/><numFmt numFmtId="165" formatCode="#,##0.########"/></numFmts>
          <fonts count="2"><font><sz val="11"/><name val="Calibri"/></font><font><b/><color rgb="FFFFFFFF"/><sz val="11"/><name val="Calibri"/></font></fonts>
          <fills count="3"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill><fill><patternFill patternType="solid"><fgColor rgb="FFA83F2B"/><bgColor indexed="64"/></patternFill></fill></fills>
          <borders count="1"><border><left/><right/><top/><bottom/><diagonal/></border></borders>
          <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
          <cellXfs count="4"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/><xf numFmtId="0" fontId="1" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1"><alignment vertical="center" wrapText="1"/></xf><xf numFmtId="165" fontId="0" fillId="0" borderId="0" xfId="0" applyNumberFormat="1"/><xf numFmtId="164" fontId="0" fillId="0" borderId="0" xfId="0" applyNumberFormat="1"/></cellXfs>
          <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
        </styleSheet>
        """;
}
