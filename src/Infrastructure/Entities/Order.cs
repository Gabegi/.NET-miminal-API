namespace Infrastructure.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);
}
