namespace Quan_Li_Nha_Hang_Suon_Muoi.Models;
public sealed class RestaurantState
{
    public int Version { get; set; } = 1;
    public long Revision
    {
        get; set;
    }
    public int ReceiptSequence
    {
        get; set;
    }
    public int PurchaseSequence
    {
        get; set;
    }
    public List<MenuItem> Foods { get; set; } = [];
    public List<Ingredient> Ingredients { get; set; } = [];
    public List<RecipeLine> Recipes { get; set; } = [];
    public List<ExtraService> Services { get; set; } = [];
    public List<DiningTable> Tables { get; set; } = [];
    public List<Reservation> Reservations { get; set; } = [];
    public List<Purchase> Purchases { get; set; } = [];
    public List<Sale> Sales { get; set; } = [];
}

public sealed class MenuItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price
    {
        get; set;
    }
    public bool Active { get; set; } = true;
    public string ImagePath { get; set; } = "";
    public string Instructions { get; set; } = "";
}

public sealed class Ingredient
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "g";
    public decimal Stock
    {
        get; set;
    }
    public decimal Minimum
    {
        get; set;
    }
    public decimal AverageCost
    {
        get; set;
    }
}

public sealed class RecipeLine
{
    public string FoodId { get; set; } = "";
    public string IngredientId { get; set; } = "";
    public decimal Quantity
    {
        get; set;
    }
}

public sealed class ExtraService
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public decimal Price
    {
        get; set;
    }
    public bool Active { get; set; } = true;
}

public sealed class DiningTable
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public int Seats { get; set; } = 4;
}

public sealed class Reservation
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string TableId { get; set; } = "";
    public string Customer { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime Time { get; set; } = DateTime.Now.AddHours(1);
    public int Guests { get; set; } = 2;
    public string Status { get; set; } = "Đã đặt";
}

public sealed class PurchaseLine
{
    public string IngredientId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "";
    public decimal Quantity
    {
        get; set;
    }
    public decimal UnitCost
    {
        get; set;
    }

    public decimal Amount
    {
        get
        {
            return Quantity * UnitCost;
        }
    }
}

public sealed class Purchase
{
    public string Id { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public string Supplier { get; set; } = "";
    public string Operator { get; set; } = "";
    public List<PurchaseLine> Lines { get; set; } = [];

    public decimal Total
    {
        get
        {
            return Lines.Sum(x => x.Amount);
        }
    }
}

public sealed class SaleLine
{
    public string FoodId { get; set; } = "";
    public string Name { get; set; } = "";
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice
    {
        get; set;
    }
    public string Note { get; set; } = "";

    public decimal Amount
    {
        get
        {
            return Quantity * UnitPrice;
        }
    }
}

public sealed class ServiceLine
{
    public string ServiceId { get; set; } = "";
    public string Name { get; set; } = "";
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice
    {
        get; set;
    }

    public decimal Amount
    {
        get
        {
            return Quantity * UnitPrice;
        }
    }
}

public sealed class Sale
{
    public string Id { get; set; } = "";
    public string? TableId
    {
        get; set;
    }
    public string TableName { get; set; } = "Mang về";
    public string Customer { get; set; } = "Khách lẻ";
    public string Operator { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PaidAt
    {
        get; set;
    }
    public string Status { get; set; } = "Đang phục vụ";
    public List<SaleLine> Lines { get; set; } = [];
    public List<ServiceLine> Extras { get; set; } = [];
    public decimal Discount
    {
        get; set;
    }
    public string PaymentMethod { get; set; } = "Tiền mặt";
    public decimal Received
    {
        get; set;
    }

    public decimal Subtotal
    {
        get
        {
            return Lines.Sum(x => x.Amount) + Extras.Sum(x => x.Amount);
        }
    }

    public decimal Total
    {
        get
        {
            return Subtotal - Discount;
        }
    }

    public decimal Change
    {
        get
        {
            return Status == "Đã thanh toán" && PaymentMethod == "Tiền mặt" ? Received - Total : 0;
        }
    }
}
