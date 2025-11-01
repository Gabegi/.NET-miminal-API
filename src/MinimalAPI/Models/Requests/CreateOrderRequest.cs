using Infrastructure.Entities;

namespace MinimalAPI.Models.Requests;

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
