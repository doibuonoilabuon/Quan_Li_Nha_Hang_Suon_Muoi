using Quan_Li_Nha_Hang_Suon_Muoi.DAL;
using Quan_Li_Nha_Hang_Suon_Muoi.Services;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Forms.Business;
public partial class frmHelp : Form
{
    public frmHelp()
    {
        InitializeComponent();
        Text = "Hướng dẫn / Sao lưu";
        btnFolder.Click += (_, _) => Ui.Run(
            this,
            () => System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo(RestaurantStore.Current.DirectoryPath) { UseShellExecute = true }
            )
        );
        btnBackup.Click += (_, _) => Ui.Run(
            this,
            () =>
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "Bản sao dữ liệu ZIP (*.zip)|*.zip",
                FileName = "SuonMuoi-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".zip"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var root = RestaurantStore.Current.DirectoryPath;
            if (Path.GetFullPath(dialog.FileName)
                    .StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Chọn nơi sao lưu nằm ngoài thư mục ảnh/dữ liệu nhập.");
            }

            var temporary = dialog.FileName + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (var output = new FileStream(temporary, FileMode.CreateNew))
                {
                    AppDatabase.WritePortableBackup(output, root);
                }

                File.Move(temporary, dialog.FileName, true);
            }
            finally
            {
                if (File.Exists(temporary))
                {
                    File.Delete(temporary);
                }
            }

            MessageBox.Show(this, "Đã xuất bản chụp dữ liệu SQL Server và ảnh món ăn.");
        }
        );
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (Ui.IsDesigner(this))
        {
            return;
        }

        instructions.Text = $"""
SƯỜN MƯỜI - QUY TRÌNH SỬ DỤNG

1. Quản trị viên khai báo nguyên liệu, đơn vị cơ sở, món/đồ uống và giá bán.
2. Món ăn → Công thức: nhập định lượng cho 1 phần, cùng đơn vị kho (g, ml, cái...). Nhập hướng dẫn chế biến/pha chế và chọn ảnh ở Sửa món.
3. Nhập nguyên liệu: chọn nhà cung cấp, nguyên liệu, số lượng và đơn giá. Ghi sổ cộng tồn kho. Tồn mẫu ban đầu bằng 0.
4. Đặt bàn: chọn giờ và số khách. Mỗi lịch giữ bàn trong khoảng 2 giờ. Khi khách đến, chọn Nhận bàn rồi mở hóa đơn từ màn hình Bàn.
5. Đơn hàng: tạo đơn, chọn bàn hoặc mang về, thêm món và dịch vụ. Có thể sửa số lượng, ghi chú, xóa dòng và lưu đơn mở.
6. Thanh toán: nhập giảm giá, hình thức trả tiền và tiền khách đưa. Chỉ thanh toán khi đủ nguyên liệu theo công thức. Transaction SQL đồng thời lưu hóa đơn, trừ kho và giải phóng bàn. Đơn đã thanh toán không sửa/xóa.
7. Chọn đơn → In đơn hàng để xem trước và chọn máy in hoặc Print to PDF. Khách hàng / Đơn hàng / Món ăn có nút Xuất Excel.
8. In / Thống kê: chọn khoảng ngày, tháng hoặc quý. Doanh thu chỉ tính đơn đã thanh toán. Bán chạy tính số phần và tiền món trước giảm.

PHÂN QUYỀN
Admin: cập nhật danh mục, giá, công thức. Staff: xem công thức, nhập hàng, đặt/nhận bàn, bán hàng, thanh toán, in và báo cáo.

SQL SERVER / SAO LƯU
Server: {AppDatabase.ServerName}
Database: {AppDatabase.DatabaseName}
Ảnh / dữ liệu nhập ban đầu: {RestaurantStore.Current.DirectoryPath}
Dữ liệu nghiệp vụ và tài khoản được đọc/ghi trong SQL Server. Tệp JSON cũ chỉ được nhập một lần khi khởi tạo DB, sau đó được giữ nguyên.
Sao lưu DB đầy đủ: SSMS → LuaMocDB → Tasks → Back Up → Full. Khôi phục bằng Restore Database trong SSMS.
Nút ZIP xuất bản chụp dữ liệu SQL hiện tại và ảnh, không gồm tài khoản/mật khẩu; dùng để chuyển sang DB trống ở máy khác. Chép JSON vào thư mục cũ không ghi đè DB đang hoạt động.
Tài khoản nằm trong bảng Users, mật khẩu PBKDF2 có salt; quyền Admin/Staff được giữ nguyên.
Chi tiết cài đặt và di chuyển dữ liệu: Database/README_SQL_SERVER.md.
""";
    }
}
