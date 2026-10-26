namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;

public static class XuLyAnh
{
   
    public static string ThuMucDuLieu
    {
        get;
        set;
    } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LuaMoc", "Data");

    public static string LayThuMucChonAnh()
    {
        DirectoryInfo? thuMuc = new DirectoryInfo(AppContext.BaseDirectory);
        while (thuMuc != null &&
            !File.Exists(Path.Combine(thuMuc.FullName, "Quan_Li_Nha_Hang_Suon_Muoi.csproj")))
        {
            thuMuc = thuMuc.Parent;
        }

        string duongDan = Path.Combine(thuMuc?.FullName ?? AppContext.BaseDirectory, "AnhMonAn");
        Directory.CreateDirectory(duongDan);
        return duongDan;
    }

    public static string DuongDan(string anh)
    {
        if (string.IsNullOrWhiteSpace(anh))
            return "";
        return Path.IsPathRooted(anh) ? anh : Path.Combine(ThuMucDuLieu, anh);
    }

    public static Image? MoAnh(string duongDan)
    {
        if (string.IsNullOrEmpty(duongDan) || !File.Exists(duongDan))
            return null;
        using var stream = File.OpenRead(duongDan);
        using var image = Image.FromStream(stream);
        return new Bitmap(image);
        
    }

    public static string LuuAnh(string duongDanGoc)
    {
        var file = new FileInfo(duongDanGoc);
        if (!file.Exists || file.Length > 10 * 1024 * 1024)
            throw new InvalidOperationException("Chọn ảnh có dung lượng tối đa 10 MB.");
        string ext = file.Extension.ToLowerInvariant();
        if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".bmp")
            throw new InvalidOperationException("Chỉ nhận ảnh JPG, PNG hoặc BMP.");
        using (var image = MoAnh(duongDanGoc))
        {
            if (image == null || (long)image.Width * image.Height > 20000000)
                throw new InvalidOperationException("Ảnh không hợp lệ hoặc quá lớn.");
        }
        string ten = Guid.NewGuid().ToString("N") + ext;
        string thuMuc = Path.Combine(ThuMucDuLieu, "Images");
        Directory.CreateDirectory(thuMuc);
        File.Copy(duongDanGoc, Path.Combine(thuMuc, ten));
        return Path.Combine("Images", ten);
    }

    public static void HienThi(PictureBox picture, string duongDan)
    {
        var cu = picture.Image;
        picture.Image = null;
        cu?.Dispose();
        if (!string.IsNullOrEmpty(duongDan))
            picture.Image = MoAnh(duongDan);
    }
}
