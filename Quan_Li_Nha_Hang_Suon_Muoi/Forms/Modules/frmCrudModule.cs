using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Modules;
public partial class frmCrudModule : Form
{
    protected string ModuleKey = "food", ModuleTitle = "Món ăn";
    private DataView? view;
    public frmCrudModule()
    {
        InitializeComponent();
        btnExcel.Click += (_, _) => Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business.Ui.Run(this, () => ExcelExport.Show(this, grid, ModuleTitle));
        btnAdd.Click += (_, _) => Edit(null);
        btnEdit.Click += (_, _) =>
        {
            if (grid.CurrentRow?.DataBoundItem is DataRowView r)
            {
                Edit(r.Row);
            }
            else
            {
                Warn();
            }
        };
        btnDelete.Click += (_, _) => Delete();
        btnRefresh.Click += (_, _) =>
        {
            txtSearch.Clear();
            DemoModuleData.Reload(ModuleKey);
            LoadData();
        };
        txtSearch.TextChanged += (_, _) => Filter();
    }

    protected void SetupModule(string key, string title)
    {
        ModuleKey = key;
        ModuleTitle = title;
        Text = title;
        btnExcel.Visible = key is "customers" or "orders" or "food";
        if (!Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business.Ui.IsDesigner(this))
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        var t = DemoModuleData.Get(ModuleKey);
        view = new DataView(t);
        grid.DataSource = view;
        lblCount.Text = $"Tổng số: {t.Rows.Count} • SQL Server";
    }

    private void Filter()
    {
        if (view?.Table is not DataTable table)
        {
            return;
        }

        string q = txtSearch.Text.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
        view.RowFilter = string.IsNullOrWhiteSpace(q) ? "" : string.Join(
            " OR ",
            table.Columns.Cast<DataColumn>()
                .Select(c => $"CONVERT([{c.ColumnName}], 'System.String') LIKE '%{q}%'")
        );
        lblCount.Text = $"Hiển thị: {view.Count}/{table.Rows.Count}";
    }

    private void Edit(DataRow? row)
    {
        using var dialog = new frmRecordEdit(DemoModuleData.Get(ModuleKey), row);
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            Persist();
            LoadData();
            Filter();
        }
    }

    private void Delete()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView r)
        {
            Warn();
            return;
        }

        if (MessageBox.Show("Xóa dòng đang chọn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            r.Row.Delete();
            Persist();
            LoadData();
            Filter();
        }
    }

    private void Persist()
    {
        try
        {
            DemoModuleData.Save(ModuleKey);
        }
        catch (Exception e)
        {
            DemoModuleData.Reload(ModuleKey);
            MessageBox.Show("Không lưu được: " + e.Message);
        }
    }

    private void Warn()
    {
        MessageBox.Show("Vui lòng chọn một dòng.", ModuleTitle);
    }
}
