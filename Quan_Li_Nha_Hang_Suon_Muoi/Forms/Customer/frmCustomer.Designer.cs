#nullable disable
namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Customer;
partial class frmCustomer
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel page;
    private FlowLayoutPanel toolbar;
    private TextBox txtSearch;
    private DataGridView grid;
    private Label lblCount;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnRefresh;
    private Button btnExcel;
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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        page = new TableLayoutPanel();
        toolbar = new FlowLayoutPanel();
        txtSearch = new TextBox();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnExcel = new Button();
        grid = new DataGridView();
        lblCount = new Label();
        page.SuspendLayout();
        toolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        SuspendLayout();
        //
        // page
        //
        page.ColumnCount = 1;
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        page.Controls.Add(toolbar, 0, 0);
        page.Controls.Add(grid, 0, 1);
        page.Controls.Add(lblCount, 0, 2);
        page.Dock = DockStyle.Fill;
        page.Location = new Point(22, 22);
        page.Margin = new Padding(0);
        page.Name = "page";
        page.RowCount = 3;
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        page.Size = new Size(1056, 656);
        page.TabIndex = 0;
        //
        // toolbar
        //
        toolbar.AutoScroll = true;
        toolbar.Controls.Add(txtSearch);
        toolbar.Controls.Add(btnAdd);
        toolbar.Controls.Add(btnEdit);
        toolbar.Controls.Add(btnDelete);
        toolbar.Controls.Add(btnRefresh);
        toolbar.Controls.Add(btnExcel);
        toolbar.Dock = DockStyle.Fill;
        toolbar.Location = new Point(0, 0);
        toolbar.Margin = new Padding(0);
        toolbar.Name = "toolbar";
        toolbar.Padding = new Padding(0, 10, 0, 0);
        toolbar.Size = new Size(1056, 65);
        toolbar.TabIndex = 0;
        toolbar.WrapContents = false;
        //
        // txtSearch
        //
        txtSearch.Location = new Point(0, 12);
        txtSearch.Margin = new Padding(0, 2, 12, 0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Tìm kiếm...";
        txtSearch.Size = new Size(240, 30);
        txtSearch.TabIndex = 0;
        //
        // btnAdd
        //
        btnAdd.BackColor = Color.FromArgb(168, 63, 43);
        btnAdd.Cursor = Cursors.Hand;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(252, 10);
        btnAdd.Margin = new Padding(0, 0, 8, 0);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(105, 35);
        btnAdd.TabIndex = 1;
        btnAdd.Text = "+ Thêm";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
        //
        // btnEdit
        //
        btnEdit.BackColor = Color.White;
        btnEdit.Cursor = Cursors.Hand;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.ForeColor = Color.FromArgb(36, 32, 29);
        btnEdit.Location = new Point(365, 10);
        btnEdit.Margin = new Padding(0, 0, 8, 0);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(85, 35);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "Sửa";
        btnEdit.UseVisualStyleBackColor = false;
        //
        // btnDelete
        //
        btnDelete.BackColor = Color.White;
        btnDelete.Cursor = Cursors.Hand;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.ForeColor = Color.FromArgb(36, 32, 29);
        btnDelete.Location = new Point(458, 10);
        btnDelete.Margin = new Padding(0, 0, 8, 0);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(85, 35);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "Xóa";
        btnDelete.UseVisualStyleBackColor = false;
        //
        // btnRefresh
        //
        btnRefresh.BackColor = Color.White;
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.ForeColor = Color.FromArgb(36, 32, 29);
        btnRefresh.Location = new Point(551, 10);
        btnRefresh.Margin = new Padding(0, 0, 8, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(105, 35);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "Làm mới";
        btnRefresh.UseVisualStyleBackColor = false;
        //
        // btnExcel
        //
        btnExcel.BackColor = Color.White;
        btnExcel.Cursor = Cursors.Hand;
        btnExcel.FlatStyle = FlatStyle.Flat;
        btnExcel.ForeColor = Color.FromArgb(36, 32, 29);
        btnExcel.Location = new Point(664, 10);
        btnExcel.Margin = new Padding(0, 0, 8, 0);
        btnExcel.Name = "btnExcel";
        btnExcel.Size = new Size(130, 35);
        btnExcel.TabIndex = 5;
        btnExcel.Text = "Xuất Excel";
        btnExcel.UseVisualStyleBackColor = false;
        //
        // grid
        //
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(242, 234, 219);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = Color.FromArgb(36, 32, 29);
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.White;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 10F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(36, 32, 29);
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(242, 222, 214);
        dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(36, 32, 29);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        grid.DefaultCellStyle = dataGridViewCellStyle2;
        grid.Dock = DockStyle.Fill;
        grid.EnableHeadersVisualStyles = false;
        grid.Location = new Point(3, 68);
        grid.MultiSelect = false;
        grid.Name = "grid";
        grid.ReadOnly = true;
        grid.RowHeadersVisible = false;
        grid.RowHeadersWidth = 51;
        grid.RowTemplate.Height = 34;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.Size = new Size(1050, 551);
        grid.TabIndex = 1;
        //
        // lblCount
        //
        lblCount.Dock = DockStyle.Fill;
        lblCount.ForeColor = Color.Gray;
        lblCount.Location = new Point(3, 622);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(1050, 34);
        lblCount.TabIndex = 2;
        lblCount.TextAlign = ContentAlignment.MiddleLeft;
        //
        // frmCustomer
        //
        AutoScaleDimensions = new SizeF(9F, 23F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(255, 247, 237);
        ClientSize = new Size(1100, 700);
        Controls.Add(page);
        Font = new Font("Segoe UI", 10F);
        Name = "frmCustomer";
        Padding = new Padding(22);
        Text = "Khách hàng";
        page.ResumeLayout(false);
        toolbar.ResumeLayout(false);
        toolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();
        ResumeLayout(false);
    }
}
