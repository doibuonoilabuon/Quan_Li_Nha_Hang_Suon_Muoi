using System.Text.Json;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;
using Food = Quan_Li_Nha_Hang_Suon_Muoi.Models.MenuItem;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Services;
/// <summary>Business rules backed by SQL Server transactions. DirectoryPath stores local image assets and legacy imports.</summary>
public sealed class RestaurantStore
{
    private static readonly Lazy<RestaurantStore> instance = new(
        () => new RestaurantStore(
            Environment.GetEnvironmentVariable("SUONMUOI_DATA_DIR") ?? Environment.GetEnvironmentVariable("LUAMOC_DATA_DIR") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LuaMoc", "Data")
        )
    );
    public static RestaurantStore Current
    {
        get
        {
            return instance.Value;
        }
    }

    private static readonly JsonSerializerOptions json = new()
    {
        WriteIndented = true
    };
    private readonly object gate = new();
    private RestaurantState state;
    public string DirectoryPath
    {
        get;
    }

    public string FilePath
    {
        get
        {
            return Path.Combine(DirectoryPath, "restaurant.json");
        }
    }

    public RestaurantStore(string directory)
    {
        DirectoryPath = Path.GetFullPath(directory);
        Directory.CreateDirectory(DirectoryPath);
        state = Quan_Li_Nha_Hang_Suon_Muoi.DAL.AppDatabase.Initialize(DirectoryPath, Seed);
    }

    public static T Clone<T>(T value)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, json), json)!;
    }

    public RestaurantState Snapshot()
    {
        lock (gate)
        {
            return Clone(state);
        }
    }

    private void Commit(Action<RestaurantState> change)
    {
        lock (gate)
        {
            state = Quan_Li_Nha_Hang_Suon_Muoi.DAL.AppDatabase.Change(state.Revision, change);
        }
    }

    public void Reload()
    {
        lock (gate)
        {
            state = Quan_Li_Nha_Hang_Suon_Muoi.DAL.AppDatabase.LoadState();
        }
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void Replace<T>(List<T> list, T item, Func<T, string> id)
    {
        var index = list.FindIndex(x => id(x) == id(item));
        if (index < 0)
        {
            list.Add(item);
        }
        else
        {
            list[index] = item;
        }
    }

    private static string Operator
    {
        get
        {
            return Session.CurrentUser?.DisplayName ?? "Nhân viên";
        }
    }

    public void SaveFood(Food food)
    {
        Commit(
            s =>
        {
            Require(
                    !string.IsNullOrWhiteSpace(food.Name) && !string.IsNullOrWhiteSpace(food.Category),
                    "Nhập tên món và danh mục."
                );
            Require(food.Price > 0 && food.Price <= 1_000_000_000m, "Giá món phải lớn hơn 0, tối đa 1 tỷ đồng.");
            Require(
                    !s.Foods.Any(x => x.Id != food.Id && x.Name.Equals(food.Name.Trim(), StringComparison.OrdinalIgnoreCase)),
                    "Tên món đã tồn tại."
                );
            var copy = Clone(food);
            copy.Name = copy.Name.Trim();
            copy.Category = copy.Category.Trim();
            Replace(s.Foods, copy, x => x.Id);
        }
        );
    }

    public void DeleteFood(string id)
    {
        Commit(
            s =>
        {
            Require(
                    !s.Sales.Any(x => x.Lines.Any(l => l.FoodId == id)),
                    "Món đã có trong hóa đơn. Hãy tạm ngừng bán thay vì xóa."
                );
            s.Foods.RemoveAll(x => x.Id == id);
            s.Recipes.RemoveAll(x => x.FoodId == id);
        }
        );
    }

    public void SaveIngredient(Ingredient ingredient)
    {
        Commit(
            s =>
        {
            Require(
                    !string.IsNullOrWhiteSpace(ingredient.Name) && !string.IsNullOrWhiteSpace(ingredient.Unit),
                    "Nhập tên và đơn vị nguyên liệu."
                );
            Require(ingredient.Minimum >= 0, "Mức tồn tối thiểu không được âm.");
            Require(
                    !s.Ingredients.Any(
                        x => x.Id != ingredient.Id && x.Name.Equals(ingredient.Name.Trim(), StringComparison.OrdinalIgnoreCase)
                    ),
                    "Tên nguyên liệu đã tồn tại."
                );
            var old = s.Ingredients.Find(x => x.Id == ingredient.Id);
            var copy = Clone(ingredient);
            copy.Name = copy.Name.Trim();
            copy.Unit = copy.Unit.Trim();
            Require(
                    old == null || old.Unit == copy.Unit
                        || (old.Stock == 0 && !s.Recipes.Any(x => x.IngredientId == old.Id)
                        && !s.Purchases.Any(x => x.Lines.Any(l => l.IngredientId == old.Id))),
                    "Không đổi đơn vị nguyên liệu đã có phát sinh kho/công thức."
                );
            copy.Stock = old?.Stock ?? 0;
            copy.AverageCost = old?.AverageCost ?? 0;
            Replace(s.Ingredients, copy, x => x.Id);
        }
        );
    }

    public void DeleteIngredient(string id)
    {
        Commit(
            s =>
        {
            Require(
                    !s.Recipes.Any(x => x.IngredientId == id)
                        && !s.Purchases.Any(x => x.Lines.Any(l => l.IngredientId == id)),
                    "Nguyên liệu đã dùng trong công thức hoặc phiếu nhập."
                );
            s.Ingredients.RemoveAll(x => x.Id == id);
        }
        );
    }

    public void SaveRecipe(string foodId, string instructions, List<RecipeLine> lines)
    {
        Commit(
            s =>
        {
            Require(s.Foods.Any(x => x.Id == foodId), "Không tìm thấy món.");
            Require(
                    lines.Count > 0
                        && lines.All(
                        x => x.Quantity > 0 && x.Quantity <= 1_000_000_000m && s.Ingredients.Any(i => i.Id == x.IngredientId)
                    ),
                    "Công thức cần nguyên liệu hợp lệ và định lượng trong khoảng 0 đến 1 tỷ."
                );
            Require(
                    lines.Select(x => x.IngredientId).Distinct().Count() == lines.Count,
                    "Nguyên liệu bị trùng trong công thức."
                );
            s.Recipes.RemoveAll(x => x.FoodId == foodId);
            foreach (var item in Clone(lines))
            {
                item.FoodId = foodId;
                s.Recipes.Add(item);
            }

            s.Foods.Single(x => x.Id == foodId).Instructions = instructions.Trim();
        }
        );
    }

    public void SaveService(ExtraService service)
    {
        Commit(
            s =>
        {
            Require(
                    !string.IsNullOrWhiteSpace(service.Name) && service.Price >= 0 && service.Price <= 1_000_000_000m,
                    "Nhập tên dịch vụ và giá từ 0 đến 1 tỷ."
                );
            Require(
                    !s.Services.Any(x => x.Id != service.Id && x.Name.Equals(service.Name.Trim(), StringComparison.OrdinalIgnoreCase)),
                    "Tên dịch vụ đã tồn tại."
                );
            Replace(s.Services, Clone(service), x => x.Id);
        }
        );
    }

    public void DeleteService(string id)
    {
        Commit(
            s =>
        {
            Require(
                    !s.Sales.Any(x => x.Extras.Any(l => l.ServiceId == id)),
                    "Dịch vụ đã có trong hóa đơn. Hãy ngừng cung cấp thay vì xóa."
                );
            s.Services.RemoveAll(x => x.Id == id);
        }
        );
    }

    public void SaveTable(DiningTable table)
    {
        Commit(
            s =>
        {
            Require(!string.IsNullOrWhiteSpace(table.Name) && table.Seats > 0, "Tên bàn và số chỗ phải hợp lệ.");
            Require(
                    !s.Tables.Any(x => x.Id != table.Id && x.Name.Equals(table.Name.Trim(), StringComparison.OrdinalIgnoreCase)),
                    "Tên bàn đã tồn tại."
                );
            Require(
                    !s.Reservations.Any(
                        x => x.TableId == table.Id && x.Status == "Đã đặt" && x.Time >= DateTime.Now && x.Guests > table.Seats
                    ),
                    "Số chỗ nhỏ hơn số khách đã đặt."
                );
            Replace(s.Tables, Clone(table), x => x.Id);
        }
        );
    }

    public void DeleteTable(string id)
    {
        Commit(
            s =>
        {
            Require(
                    !s.Sales.Any(x => x.TableId == id) && !s.Reservations.Any(x => x.TableId == id),
                    "Bàn đã có lịch đặt hoặc hóa đơn, không thể xóa."
                );
            s.Tables.RemoveAll(x => x.Id == id);
        }
        );
    }

    public void SaveReservation(Reservation reservation)
    {
        Commit(
            s =>
        {
            var table = s.Tables.Find(x => x.Id == reservation.TableId);
            Require(
                    table != null && reservation.Guests > 0 && reservation.Guests <= table!.Seats,
                    "Chọn bàn đủ chỗ cho số khách."
                );
            Require(
                    !string.IsNullOrWhiteSpace(reservation.Customer) && !string.IsNullOrWhiteSpace(reservation.Phone),
                    "Nhập tên và số điện thoại khách."
                );
            Require(reservation.Time > DateTime.Now, "Giờ đặt bàn phải ở tương lai.");
            var old = s.Reservations.Find(x => x.Id == reservation.Id);
            Require(old == null || old.Status == "Đã đặt", "Lịch đặt đã xử lý, không thể sửa.");
            Require(
                    !s.Reservations.Any(
                        x => x.Id != reservation.Id && x.Status == "Đã đặt" && x.TableId == reservation.TableId
                            && Math.Abs((x.Time - reservation.Time).TotalHours) < 2
                    ),
                    "Bàn đã được đặt trong khoảng 2 giờ này."
                );
            Require(
                    reservation.Time > DateTime.Now.AddHours(2)
                        || !s.Sales.Any(x => x.TableId == reservation.TableId && x.Status == "Đang phục vụ"),
                    "Bàn đang phục vụ khách, hãy chọn bàn hoặc giờ khác."
                );
            var copy = Clone(reservation);
            copy.Status = "Đã đặt";
            Replace(s.Reservations, copy, x => x.Id);
        }
        );
    }

    public void SetReservationStatus(string id, string status)
    {
        Commit(
            s =>
        {
            var item = s.Reservations.Single(x => x.Id == id);
            Require(item.Status == "Đã đặt" && status is "Đã nhận bàn" or "Đã hủy", "Lịch đặt đã xử lý.");
            if (status == "Đã nhận bàn")
            {
                Require(
                        Math.Abs((item.Time - DateTime.Now).TotalHours) <= 2,
                        "Chỉ nhận bàn trong khoảng 2 giờ quanh giờ đã đặt."
                    );
                Require(
                        !s.Sales.Any(x => x.TableId == item.TableId && x.Status == "Đang phục vụ"),
                        "Bàn đang phục vụ khách khác."
                    );
            }

            item.Status = status;
        }
        );
    }

    public string PostPurchase(Purchase purchase)
    {
        string result = "";
        Commit(
            s =>
        {
            Require(string.IsNullOrEmpty(purchase.Id), "Phiếu nhập đã được ghi sổ, không thể ghi lại.");
            Require(
                    !string.IsNullOrWhiteSpace(purchase.Supplier) && purchase.Lines.Count > 0,
                    "Nhập nhà cung cấp và ít nhất một nguyên liệu."
                );
            Require(purchase.Date.Date <= DateTime.Today, "Ngày nhập không được ở tương lai.");
            var receipt = Clone(purchase);
            receipt.Id = $"NH{DateTime.Today:yyyyMMdd}-{++s.PurchaseSequence:0000}";
            receipt.Operator = Operator;
            foreach (var line in receipt.Lines)
            {
                var ingredient = s.Ingredients.Find(x => x.Id == line.IngredientId);
                Require(
                        ingredient != null && line.Quantity > 0 && line.Quantity <= 1_000_000_000m && line.UnitCost > 0
                            && line.UnitCost <= 1_000_000_000m,
                        "Nguyên liệu, số lượng và đơn giá nhập phải hợp lệ (tối đa 1 tỷ)."
                    );
                line.Name = ingredient!.Name;
                line.Unit = ingredient.Unit;
                ingredient.AverageCost = (ingredient.Stock * ingredient.AverageCost + line.Amount) / (ingredient.Stock + line.Quantity);
                ingredient.Stock += line.Quantity;
            }

            s.Purchases.Add(receipt);
            result = receipt.Id;
        }
        );
        return result;
    }

    public string SaveSale(Sale sale, bool checkout)
    {
        string result = "";
        Commit(
            s =>
        {
            var old = s.Sales.Find(x => x.Id == sale.Id);
            Require(
                    old == null ? string.IsNullOrEmpty(sale.Id) : old.Status == "Đang phục vụ",
                    "Hóa đơn đã thanh toán/hủy hoặc không còn tồn tại."
                );
            Require(
                    sale.Lines.Count > 0 && sale.Lines.All(x => x.Quantity > 0 && x.Quantity <= 10000),
                    "Hóa đơn cần ít nhất một món có số lượng từ 1 đến 10.000."
                );
            var draft = Clone(sale);
            if (draft.TableId != null)
            {
                var table = s.Tables.Find(x => x.Id == draft.TableId);
                Require(table != null, "Bàn không còn tồn tại.");
                Require(
                        !s.Sales.Any(x => x.Id != draft.Id && x.TableId == draft.TableId && x.Status == "Đang phục vụ"),
                        "Bàn đang có hóa đơn mở."
                    );
                Require(
                        !s.Reservations.Any(
                            x => x.TableId == draft.TableId && x.Status == "Đã đặt" && x.Time > DateTime.Now.AddHours(-2)
                                && x.Time < DateTime.Now.AddHours(2)
                        ),
                        "Bàn có lịch đặt gần giờ này. Hãy nhận bàn tại màn hình Bàn / Đặt bàn trước."
                    );
                draft.TableName = table!.Name;
            }
            else
            {
                draft.TableName = "Mang về";
            }

            foreach (var line in draft.Lines)
            {
                var food = s.Foods.Find(x => x.Id == line.FoodId);
                Require(food != null, "Có món không còn trong thực đơn.");
                var existing = old?.Lines.FirstOrDefault(x => x.FoodId == line.FoodId && x.UnitPrice == line.UnitPrice);
                Require(existing != null || food!.Active, $"Món {food!.Name} đã ngừng bán.");
                line.Name = existing?.Name ?? food!.Name;
                line.UnitPrice = existing?.UnitPrice ?? food!.Price;
            }

            foreach (var line in draft.Extras)
            {
                var service = s.Services.Find(x => x.Id == line.ServiceId);
                Require(service != null && line.Quantity > 0 && line.Quantity <= 10000, "Dịch vụ/số lượng không hợp lệ.");
                var existing = old?.Extras.FirstOrDefault(x => x.ServiceId == line.ServiceId && x.UnitPrice == line.UnitPrice);
                Require(existing != null || service!.Active, "Dịch vụ đã ngừng cung cấp.");
                line.Name = existing?.Name ?? service!.Name;
                line.UnitPrice = existing?.UnitPrice ?? service!.Price;
            }

            Require(
                    draft.Discount >= 0 && draft.Discount <= draft.Subtotal,
                    "Tiền giảm phải từ 0 đến tổng tiền hàng/dịch vụ."
                );
            Require(
                    draft.PaymentMethod is "Tiền mặt" or "Chuyển khoản" or "Thẻ",
                    "Phương thức thanh toán không hợp lệ."
                );
            if (checkout)
            {
                Require(draft.PaymentMethod != "Tiền mặt" || draft.Received >= draft.Total, "Tiền khách đưa chưa đủ.");
                var usage = new Dictionary<string, decimal>();
                foreach (var line in draft.Lines)
                {
                    var recipe = s.Recipes.Where(x => x.FoodId == line.FoodId).ToList();
                    Require(recipe.Count > 0, $"Món {line.Name} chưa có công thức. Hãy khai báo trước khi thanh toán.");
                    foreach (var r in recipe)
                    {
                        usage[r.IngredientId] = usage.GetValueOrDefault(r.IngredientId) + r.Quantity * line.Quantity;
                    }
                }

                foreach (var use in usage)
                {
                    var ingredient = s.Ingredients.Single(x => x.Id == use.Key);
                    Require(
                            ingredient.Stock >= use.Value,
                            $"Thiếu {ingredient.Name}: cần {use.Value:N3} {ingredient.Unit}, tồn {ingredient.Stock:N3}."
                        );
                    ingredient.Stock -= use.Value;
                }

                draft.Status = "Đã thanh toán";
                draft.PaidAt = DateTime.Now;
                if (draft.PaymentMethod != "Tiền mặt")
                {
                    draft.Received = draft.Total;
                }
            }
            else
            {
                draft.Status = "Đang phục vụ";
                draft.PaidAt = null;
            }

            if (old == null)
            {
                draft.Id = $"HD{DateTime.Today:yyyyMMdd}-{++s.ReceiptSequence:0000}";
                draft.CreatedAt = DateTime.Now;
                draft.Operator = Operator;
            }
            else
            {
                draft.CreatedAt = old.CreatedAt;
                draft.Operator = old.Operator;
            }

            draft.Customer = string.IsNullOrWhiteSpace(draft.Customer) ? "Khách lẻ" : draft.Customer.Trim();
            Replace(s.Sales, draft, x => x.Id);
            result = draft.Id;
        }
        );
        return result;
    }

    public void CancelSale(string id)
    {
        Commit(
            s =>
        {
            var sale = s.Sales.Single(x => x.Id == id);
            Require(sale.Status == "Đang phục vụ", "Chỉ hủy được hóa đơn chưa thanh toán.");
            sale.Status = "Đã hủy";
        }
        );
    }

    public string ImportImage(string file)
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            return "";
        }

        var full = Path.GetFullPath(file);
        var assets = Path.Combine(DirectoryPath, "Images");
        Directory.CreateDirectory(assets);
        if (Path.GetDirectoryName(full) == assets)
        {
            return full;
        }

        using (var image = Image.FromFile(full))
        {
            if (image.Width < 1)
            {
                throw new InvalidDataException("Ảnh không hợp lệ.");
            }
        }

        var target = Path.Combine(assets, Guid.NewGuid().ToString("N") + Path.GetExtension(full));
        File.Copy(full, target);
        return target;
    }

    public static string TableStatus(RestaurantState s, string tableId)
    {
        return s.Sales.Any(x => x.TableId == tableId && x.Status == "Đang phục vụ") ? "Đang phục vụ" : s.Reservations.Any(
            x => x.TableId == tableId && x.Status == "Đã đặt" && x.Time > DateTime.Now.AddHours(-2)
                && x.Time < DateTime.Now.AddHours(2)
        ) ? "Đã đặt" : "Trống";
    }

    private static RestaurantState Seed()
    {
        var s = new RestaurantState();
        s.Ingredients.AddRange(
            [new() { Id = "suon", Name = "Sườn", Unit = "g", Minimum = 1000 }, new() { Id = "sot", Name = "Sốt mật ong", Unit = "ml", Minimum = 200 }, new() { Id = "khoai", Name = "Khoai tây", Unit = "g", Minimum = 500 }, new() { Id = "tra", Name = "Trà", Unit = "g", Minimum = 100 }, new() { Id = "dao", Name = "Siro đào", Unit = "ml", Minimum = 200 }]
        );
        s.Foods.AddRange(
            [new() { Id = "mon1", Name = "Sườn nướng mật ong", Category = "Món ăn", Price = 129000, Instructions = "Ướp sườn với sốt, nướng chín và trình bày." }, new() { Id = "mon2", Name = "Khoai tây chiên", Category = "Món ăn kèm", Price = 45000, Instructions = "Chiên khoai chín vàng, để ráo dầu." }, new() { Id = "nuoc1", Name = "Trà đào", Category = "Đồ uống", Price = 35000, Instructions = "Ủ trà, thêm siro đào và đá, khuấy đều." }]
        );
        s.Recipes.AddRange(
            [new() { FoodId = "mon1", IngredientId = "suon", Quantity = 300 }, new() { FoodId = "mon1", IngredientId = "sot", Quantity = 40 }, new() { FoodId = "mon2", IngredientId = "khoai", Quantity = 200 }, new() { FoodId = "nuoc1", IngredientId = "tra", Quantity = 8 }, new() { FoodId = "nuoc1", IngredientId = "dao", Quantity = 30 }]
        );
        for (int i = 1; i <= 10; i++)
        {
            s.Tables.Add(new()
            {
                Id = "ban" + i,
                Name = "Bàn " + i.ToString("00"),
                Seats = i > 6 ? 6 : 4
            });
        }

        s.Services.AddRange(
            [new() { Name = "Trang trí sinh nhật", Price = 150000 }, new() { Name = "Phòng riêng", Price = 200000 }]
        );
        return s;
    }
}
