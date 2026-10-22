using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmEntityList : Form
{
    public frmEntityList()
    {
        InitializeComponent();
        Ui.Grid(grid);
        search.TextChanged += (_, _) => Ui.Run(this, RefreshRows);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!Ui.IsDesigner(this))
        {
            Ui.Run(this, RefreshRows);
        }
    }

    protected virtual void RefreshRows()
    {
    }

    protected bool Matches(params string[] fields)
    {
        return string.IsNullOrWhiteSpace(search.Text)
            || fields.Any(x => x.Contains(search.Text.Trim(), StringComparison.CurrentCultureIgnoreCase));
    }

    protected void Bind<T>(IEnumerable<T> rows)
    {
        grid.DataSource = rows.ToList();
        if (grid.Columns["Id"] is { } idColumn)
        {
            idColumn.Visible = false;
        }

        footer.Text = $"{grid.Rows.Count} dòng • Dữ liệu SQL Server";
    }

    protected Button ActionButton(string text, Action action, bool adminOnly = false)
    {
        var primary = text.StartsWith("+");
        var b = new Button
        {
            Text = text,
            Name = "btnAction" + toolbar.Controls.Count,
            Width = Math.Max(text == "+ Thêm" ? 105 : 85, TextRenderer.MeasureText(text, Font).Width + 26),
            Height = 35,
            Font = Font,
            Margin = new Padding(0, 0, 8, 0),
            BackColor = primary ? Theme.Brick : Color.White,
            ForeColor = primary ? Color.White : Theme.Charcoal,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        b.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            if (adminOnly && !Ui.Admin)
            {
                throw new InvalidOperationException("Chỉ quản trị viên được thay đổi danh mục.");
            }

            action();
        }
        );
        toolbar.Controls.Add(b);
        return b;
    }

    protected void AddExcel(string title)
    {
        var button = ActionButton("Xuất Excel", () => ExcelExport.Show(this, grid, title));
        button.Name = "btnExcel";
    }

    protected void AddRefresh()
    {
        ActionButton(
            "Làm mới",
            () =>
        {
            RestaurantStore.Current.Reload();
            RefreshRows();
        }
        );
    }
}
