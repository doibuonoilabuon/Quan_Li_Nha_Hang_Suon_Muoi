using System.Data;
using System.Text;
namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;

public static class XuatCsv
{
    public static void Luu(DataTable table, string file)
    {
        static string O(string text)
        {
            if (text.Length > 0 && "=+-@".Contains(text[0]))
                text = "'" + text;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
        var lines = new List<string> { string.Join(",", table.Columns.Cast<DataColumn>().Select(c => O(c.ColumnName))) };
        foreach (DataRow row in table.Rows)
            lines.Add(string.Join(",", row.ItemArray.Select(v => O(Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture) ?? ""))));
        File.WriteAllLines(file, lines, new UTF8Encoding(true));
    }
}
