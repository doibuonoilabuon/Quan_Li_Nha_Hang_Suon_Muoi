using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;
partial class frmCrudModule
{
    protected DataGridView grid = null!;
    protected TextBox txtSearch = null!;
    protected Button btnAdd = null!, btnEdit = null!, btnDelete = null!, btnRefresh = null!, btnExcel = null!;
    protected Label lblCount = null!;
    private TableLayoutPanel page = null!;
    private FlowLayoutPanel toolbar = null!;
    private void InitializeComponent()
    {
        BackColor = Color.FromArgb(255, 247, 237);
        Padding = new Padding(22);
        Font = new Font("Segoe UI", 10);
        page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(0, 10, 0, 0)
        };
        txtSearch = new TextBox
        {
            Width = 240,
            PlaceholderText = "Tìm kiếm...",
            Margin = new Padding(0, 2, 12, 0)
        };
        Button Make(string text, int width, bool primary)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 35,
                Margin = new Padding(0, 0, 8, 0),
                BackColor = primary ? Color.FromArgb(168, 63, 43) : Color.White,
                ForeColor = primary ? Color.White : Color.FromArgb(36, 32, 29),
                FlatStyle = FlatStyle.Flat
            };
        }

        btnAdd = Make("+ Thêm", 105, true);
        btnEdit = Make("Sửa", 85, false);
        btnDelete = Make("Xóa", 85, false);
        btnRefresh = Make("Làm mới", 105, false);
        btnExcel = Make("Xuất Excel", 130, false);
        btnExcel.Name = "btnExcel";
        toolbar.Controls.AddRange(new Control[] { txtSearch, btnAdd, btnEdit, btnDelete, btnRefresh, btnExcel });
        grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            RowHeadersVisible = false
        };
        lblCount = new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.Gray
        };
        page.Controls.Add(toolbar, 0, 0);
        page.Controls.Add(grid, 0, 1);
        page.Controls.Add(lblCount, 0, 2);
        Controls.Add(page);
    }
}
