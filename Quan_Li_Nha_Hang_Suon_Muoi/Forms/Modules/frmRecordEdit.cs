using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;
public partial class frmRecordEdit : Form
{
    private readonly DataTable table;
    private readonly DataRow? row;
    private readonly List<(DataColumn column, TextBox input)> inputs = new();
    public frmRecordEdit(DataTable table, DataRow? row)
    {
        InitializeComponent();
        this.table = table;
        this.row = row;
        Text = row == null ? "Thêm thông tin" : "Sửa thông tin";
        foreach (DataColumn c in table.Columns)
        {
            var panel = new Panel
            {
                Height = 64,
                Width = 430,
                Margin = new Padding(2)
            };
            var lbl = new Label
            {
                Text = c.ColumnName,
                Location = new Point(0, 0),
                Size = new Size(400, 23)
            };
            var txt = new TextBox
            {
                Text = row?[c]?.ToString() ?? "",
                Location = new Point(0, 25),
                Width = 410
            };
            panel.Controls.Add(lbl);
            panel.Controls.Add(txt);
            fields.Controls.Add(panel);
            inputs.Add((c, txt));
        }
    }

    private void SaveClick(object? sender, EventArgs e)
    {
        if (inputs.Any(x => string.IsNullOrWhiteSpace(x.input.Text)))
        {
            MessageBox.Show("Vui lòng nhập đủ các trường.");
            return;
        }

        var vals = inputs.Select(x => x.input.Text.Trim()).ToArray();
        if (row == null)
        {
            table.Rows.Add(vals);
        }
        else
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                row[inputs[i].column] = vals[i];
            }
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
