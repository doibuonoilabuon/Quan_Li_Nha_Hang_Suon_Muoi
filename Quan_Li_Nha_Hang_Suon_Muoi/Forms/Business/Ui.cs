using System.ComponentModel;
using System.Globalization;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public static class Ui
{
    public static bool IsDesigner(Control c)
    {
        return LicenseManager.UsageMode == LicenseUsageMode.Designtime || c.Site?.DesignMode == true;
    }

    public static bool Admin
    {
        get
        {
            return Session.CurrentUser?.Role == UserRole.Admin;
        }
    }

    public static void Fit(Form form)
    {
        if (!form.TopLevel || IsDesigner(form))
        {
            return;
        }

        var area = Screen.FromControl(form).WorkingArea;
        var max = new Size(Math.Max(300, area.Width - 30), Math.Max(300, area.Height - 30));
        form.MinimumSize = new Size(Math.Min(form.MinimumSize.Width, max.Width), Math.Min(form.MinimumSize.Height, max.Height));
        form.Size = new Size(Math.Min(form.Width, max.Width), Math.Min(form.Height, max.Height));
        form.Location = new Point(area.Left + (area.Width - form.Width) / 2, area.Top + (area.Height - form.Height) / 2);
    }

    public static void Run(IWin32Window owner, Action action)
    {
        try
        {
            action();
        }
        catch (Exception e) when (e is System.Data.Common.DbException
            or InvalidOperationException
            or IOException
            or UnauthorizedAccessException
            or ArgumentException
            or System.Text.Json.JsonException
            or System.Drawing.Printing.InvalidPrinterException)
        {
            MessageBox.Show(
                owner,
                e.Message,
                "SƯỜN MƯỜI",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

    public static void Confirm(IWin32Window owner, string text, Action action)
    {
        if (MessageBox.Show(
            owner,
            text,
            "Xác nhận",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        ) == DialogResult.Yes)
        {
            Run(owner, action);
        }
    }

    public static void Grid(DataGridView grid)
    {
        grid.AutoGenerateColumns = true;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.RowHeadersVisible = false;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.RowTemplate.Height = 34;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Theme.Sand;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grid.DefaultCellStyle.SelectionBackColor = Theme.SoftRed;
        grid.DefaultCellStyle.SelectionForeColor = Theme.Charcoal;
        grid.DataBindingComplete += (_, _) =>
        {
            if (grid.Columns["Id"] is { } idColumn)
            {
                idColumn.Visible = false;
            }
        };
    }

    public static string SelectedId(DataGridView grid)
    {
        return grid.CurrentRow?.Cells["Id"].Value?.ToString() ?? throw new InvalidOperationException("Vui lòng chọn một dòng.");
    }

    public static void Combo(ComboBox combo, IEnumerable<Choice> choices)
    {
        combo.DisplayMember = "Text";
        combo.ValueMember = "Id";
        combo.DataSource = choices.ToList();
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    public static string ChoiceId(ComboBox combo)
    {
        return (combo.SelectedItem as Choice)?.Id ?? "";
    }

    public static void Select(ComboBox combo, string? id)
    {
        foreach (var value in combo.Items.Cast<Choice>())
        {
            if (value.Id == (id ?? ""))
            {
                combo.SelectedItem = value;
                break;
            }
        }
    }

    public static decimal Decimal(object? value)
    {
        if (value is decimal d)
        {
            return d;
        }

        if (decimal.TryParse(Convert.ToString(value), NumberStyles.Number, CultureInfo.CurrentCulture, out var n))
        {
            return n;
        }

        throw new InvalidOperationException("Số lượng/đơn giá không hợp lệ.");
    }
}

public sealed record Choice(string Id, string Text);
