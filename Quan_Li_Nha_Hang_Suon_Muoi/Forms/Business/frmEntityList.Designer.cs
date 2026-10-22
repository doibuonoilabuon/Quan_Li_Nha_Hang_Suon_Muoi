#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
partial class frmEntityList
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel page;
    protected DataGridView grid;
    protected FlowLayoutPanel toolbar;
    protected TextBox search;
    protected Label footer;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        page = new TableLayoutPanel();
        grid = new DataGridView();
        toolbar = new FlowLayoutPanel();
        search = new TextBox();
        footer = new Label();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        page.Name = "page";
        page.Dock = DockStyle.Fill;
        page.ColumnCount = 1;
        page.RowCount = 3;
        page.Margin = Padding.Empty;
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        grid.Name = "grid";
        grid.Dock = DockStyle.Fill;
        grid.Margin = new Padding(3);
        toolbar.Name = "toolbar";
        toolbar.Dock = DockStyle.Fill;
        toolbar.AutoScroll = true;
        toolbar.WrapContents = false;
        toolbar.Padding = new Padding(0, 10, 0, 0);
        search.Name = "search";
        search.Width = 240;
        search.PlaceholderText = "Tìm kiếm...";
        search.Margin = new Padding(0, 2, 12, 0);
        footer.Name = "footer";
        footer.Dock = DockStyle.Fill;
        footer.TextAlign = ContentAlignment.MiddleLeft;
        footer.ForeColor = Color.Gray;
        toolbar.Controls.Add(search);
        page.Controls.Add(toolbar, 0, 0);
        page.Controls.Add(grid, 0, 1);
        page.Controls.Add(footer, 0, 2);
        Controls.Add(page);
        AutoScaleDimensions = new SizeF(7, 15);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(255, 247, 237);
        Padding = new Padding(22);
        ClientSize = new Size(1100, 700);
        Name = "frmEntityList";
        Text = "Danh sách";
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
    }
}
