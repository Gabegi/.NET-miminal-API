using Infrastructure.Entities;

namespace MinimalAPI.Models.Requests;

public class UpdateOrderRequest
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
