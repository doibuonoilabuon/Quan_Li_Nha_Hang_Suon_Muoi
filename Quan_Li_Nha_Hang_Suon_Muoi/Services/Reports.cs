using System.Drawing.Printing;
using System.Globalization;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;
public sealed record RevenueSummary(string Period, int Receipts, decimal Gross, decimal Discount, decimal Net);
public sealed record BestSeller(string Food, string Category, int Quantity, decimal Revenue);
public static class Reports
{
    public static List<Sale> Paid(RestaurantState state, DateTime from, DateTime to)
    {
        return state.Sales.Where(x => x.Status == "Đã thanh toán" && x.PaidAt >= from.Date && x.PaidAt < to.Date.AddDays(1))
            .ToList();
    }

    public static List<RevenueSummary> Revenue(RestaurantState state, DateTime from, DateTime to, bool quarterly)
    {
        return Paid(state, from, to)
            .GroupBy(
                x => quarterly ? $"{x.PaidAt!.Value.Year} / Quý {(x.PaidAt.Value.Month - 1) / 3 + 1}" : x.PaidAt!.Value.ToString("yyyy / MM")
            )
            .OrderBy(x => x.Key)
            .Select(
                g => new RevenueSummary(
                    g.Key,
                    g.Count(),
                    g.Sum(x => x.Subtotal),
                    g.Sum(x => x.Discount),
                    g.Sum(x => x.Total)
                )
            )
            .ToList();
    }

    public static List<BestSeller> Best(RestaurantState state, DateTime from, DateTime to)
    {
        return Paid(state, from, to).SelectMany(x => x.Lines).GroupBy(x => x.FoodId)
            .Select(
                g => new BestSeller(
                    g.First().Name,
                    state.Foods.Find(x => x.Id == g.Key)?.Category ?? "Đã ngừng bán",
                    g.Sum(x => x.Quantity),
                    g.Sum(x => x.Amount)
                )
            )
            .OrderByDescending(x => x.Quantity)
            .ThenByDescending(x => x.Revenue)
            .ToList();
    }

    public static string Money(decimal value)
    {
        return value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
    }

    public static List<string> SaleDocument(Sale sale)
    {
        var result = new List<string>
        {
            "SƯỜN MƯỜI",
            sale.Status == "Đã thanh toán" ? "HÓA ĐƠN THANH TOÁN" : "PHIẾU TẠM TÍNH",
            $"Số: {sale.Id}",
            $"Ngày: {sale.PaidAt ?? sale.CreatedAt:dd/MM/yyyy HH:mm}",
            $"Bàn: {sale.TableName}   |   Khách: {sale.Customer}",
            $"Nhân viên: {sale.Operator}",
            new string('-', 65)
        };
        foreach (var line in sale.Lines)
        {
            result.Add($"{line.Name}   x{line.Quantity}   @ {Money(line.UnitPrice)}   = {Money(line.Amount)}");
            if (!string.IsNullOrWhiteSpace(line.Note))
            {
                result.Add("   Ghi chú: " + line.Note);
            }
        }

        foreach (var line in sale.Extras)
        {
            result.Add($"Dịch vụ: {line.Name}   x{line.Quantity}   = {Money(line.Amount)}");
        }

        result.AddRange(
            [new string('-', 65), "Cộng: " + Money(sale.Subtotal), "Giảm giá: " + Money(sale.Discount), "THÀNH TIỀN: " + Money(sale.Total)]
        );
        if (sale.Status == "Đã thanh toán")
        {
            result.AddRange(
                ["Phương thức: " + sale.PaymentMethod, "Khách đưa: " + Money(sale.Received), "Tiền thừa: " + Money(sale.Change)]
            );
        }

        result.Add(sale.Status == "Đã hủy" ? "HÓA ĐƠN ĐÃ HỦY" : "Cảm ơn quý khách!");
        return result;
    }

    public static List<string> PurchaseDocument(Purchase p)
    {
        var lines = new List<string>
        {
            "SƯỜN MƯỜI - PHIẾU NHẬP NGUYÊN LIỆU",
            "Số: " + p.Id,
            $"Ngày: {p.Date:dd/MM/yyyy HH:mm}",
            "Nhà cung cấp: " + p.Supplier,
            "Nhân viên: " + p.Operator,
            new string('-', 65)
        };
        lines.AddRange(
            p.Lines.Select(x => $"{x.Name}: {x.Quantity:N3} {x.Unit} x {Money(x.UnitCost)} = {Money(x.Amount)}")
        );
        lines.Add("TỔNG TIỀN: " + Money(p.Total));
        return lines;
    }

    public static void Preview(IWin32Window owner, string title, IEnumerable<string> content)
    {
        using var doc = new PrintDocument
        {
            DocumentName = title
        };
        doc.DefaultPageSettings.Margins = new Margins(45, 45, 45, 45);
        var lines = content.ToList();
        int cursor = 0, offset = 0, page = 0;
        using var font = new Font("Segoe UI", 10F);
        doc.BeginPrint += (_, _) =>
        {
            cursor = 0;
            offset = 0;
            page = 0;
        };
        doc.PrintPage += (_, e) =>
        {
            var g = e.Graphics!;
            var bounds = e.MarginBounds;
            float y = bounds.Top;
            while (cursor < lines.Count)
            {
                var text = lines[cursor][offset..];
                var available = bounds.Bottom - 30 - y;
                if (available < font.GetHeight(g) + 7)
                {
                    break;
                }

                var size = g.MeasureString(
                    text.Length == 0 ? " " : text,
                    font,
                    new SizeF(bounds.Width, available - 7),
                    StringFormat.GenericDefault,
                    out int characters,
                    out int measuredLines
                );
                if (text.Length > 0 && characters == 0)
                {
                    break;
                }

                var part = text.Length == 0 ? " " : text[..Math.Min(characters, text.Length)];
                g.DrawString(part, font, Brushes.Black, new RectangleF(bounds.Left, y, bounds.Width, size.Height));
                y += size.Height + 7;
                offset += text.Length == 0 ? 0 : characters;
                if (offset >= lines[cursor].Length)
                {
                    cursor++;
                    offset = 0;
                }
            }

            g.DrawString(
                $"{title} • Trang {++page}",
                font,
                Brushes.Gray,
                bounds.Left,
                bounds.Bottom - 20
            );
            e.HasMorePages = cursor < lines.Count;
        };
        using var preview = new PrintPreviewDialog
        {
            Document = doc,
            Width = 1050,
            Height = 800,
            UseAntiAlias = true,
            ShowInTaskbar = true
        };
        preview.Shown += (_, _) => Forms.Business.Ui.Fit(preview);
        preview.ShowDialog(owner);
    }

    public static void ExportCsv(IWin32Window owner, string name, IEnumerable<string[]> rows)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "CSV UTF-8 (*.csv)|*.csv",
            FileName = name + ".csv"
        };
        if (dialog.ShowDialog(owner) != DialogResult.OK)
        {
            return;
        }

        static string Escape(string x)
        {
            if (x.Length > 0 && "=+-@".Contains(x[0]))
            {
                x = "'" + x;
            }

            return "\"" + x.Replace("\"", "\"\"") + "\"";
        }

        File.WriteAllLines(
            dialog.FileName,
            rows.Select(x => string.Join(",", x.Select(Escape))),
            new System.Text.UTF8Encoding(true)
        );
    }
}
