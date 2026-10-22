using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public static class LineGrids
{
    public static void Setup(DataGridView grid, bool allowNew)
    {
        Ui.Grid(grid);
        grid.ReadOnly = false;
        grid.AutoGenerateColumns = false;
        grid.AllowUserToAddRows = allowNew;
        grid.AllowUserToDeleteRows = true;
        grid.EditMode = DataGridViewEditMode.EditOnEnter;
        grid.DataError += (_, e) =>
        {
            e.ThrowException = false;
            e.Cancel = true;
            MessageBox.Show(grid.FindForm(), "Giá trị chưa hợp lệ. Chọn nguyên liệu trong danh sách và nhập số dương.");
        };
        grid.CellValidating += (_, e) =>
        {
            if (grid.Rows[e.RowIndex].IsNewRow || grid.Columns[e.ColumnIndex].ReadOnly)
            {
                return;
            }

            var property = grid.Columns[e.ColumnIndex].DataPropertyName;
            if (property is not ("Quantity" or "UnitCost"))
            {
                return;
            }

            if (!decimal.TryParse(
                e.FormattedValue?.ToString(),
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out var value
            )
                || value <= 0
                || value > 1_000_000_000m)
            {
                e.Cancel = true;
                MessageBox.Show(grid.FindForm(), "Số lượng/đơn giá phải lớn hơn 0 và tối đa 1 tỷ.");
            }
        };
    }

    public static void Text(DataGridView grid, string property, string header, bool readOnly = false, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            Name = property,
            DataPropertyName = property,
            HeaderText = header,
            ReadOnly = readOnly,
            SortMode = DataGridViewColumnSortMode.NotSortable
        };
        if (format != null)
        {
            column.DefaultCellStyle.Format = format;
        }

        grid.Columns.Add(column);
    }

    public static void Ingredients(DataGridView grid, IEnumerable<Ingredient> ingredients)
    {
        grid.Columns.Add(
            new DataGridViewComboBoxColumn
            {
                Name = "IngredientId",
                DataPropertyName = "IngredientId",
                HeaderText = "Nguyên liệu / Đơn vị",
                DataSource = new[] { new Choice("", "— Chọn nguyên liệu —") }.Concat(ingredients.Select(x => new Choice(x.Id, x.Name + " (" + x.Unit + ")")))
                    .ToList(),
                DisplayMember = "Text",
                ValueMember = "Id",
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Flat
            }
        );
    }

    public static void End(DataGridView grid)
    {
        if (!grid.EndEdit())
        {
            throw new InvalidOperationException("Có ô chưa hợp lệ trong bảng.");
        }

        if (grid.DataSource is { } source && grid.BindingContext is { } context)
        {
            context[source]?.EndCurrentEdit();
        }
    }

    public static void Remove(DataGridView grid)
    {
        if (grid.CurrentRow is { IsNewRow: false } row)
        {
            grid.Rows.Remove(row);
        }
    }
}
