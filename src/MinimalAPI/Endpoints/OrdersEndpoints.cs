using Infrastructure.Entities;
using Infrastructure.Repositories;
using MinimalAPI.Extensions;
using MinimalAPI.Models.Requests;

namespace MinimalAPI.Endpoints;

public static class OrdersEndpoints
{
    public static void MapOrders(this WebApplication app)
    {
        var group = app.MapGroup("/orders");

        group.MapGet("/", GetAllOrders);
        group.MapGet("/{id}", GetOrderById);
        group.MapGet("/customer/{customerId}", GetOrdersByCustomer);
        group.MapPost("/", CreateOrder).WithValidation<CreateOrderRequest>();
        group.MapPut("/{id}", UpdateOrder).WithValidation<UpdateOrderRequest>();
        group.MapDelete("/{id}", DeleteOrder);
    }

    private static async Task<IResult> GetAllOrders(IRepository<Order> repository)
    {
        var orders = await repository.GetAllAsync();
        return Results.Ok(orders);
    }

    private static async Task<IResult> GetOrderById(int id, IRepository<Order> repository)
    {
        var order = await repository.GetByIdAsync(id);
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> GetOrdersByCustomer(int customerId, IRepository<Order> repository)
    {
        var orders = await repository.FindAsync(o => o.CustomerId == customerId);
        return Results.Ok(orders);
    }

    private static async Task<IResult> CreateOrder(CreateOrderRequest request, IRepository<Order> repository)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            Items = request.Items
        };

        await repository.AddAsync(order);
        await repository.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order);
    }

    private static async Task<IResult> UpdateOrder(int id, UpdateOrderRequest request, IRepository<Order> repository)
    {
        var order = await repository.GetByIdAsync(id);
        if (order is null)
            return Results.NotFound();

        order.CustomerId = request.CustomerId;
        order.Items = request.Items;

        await repository.UpdateAsync(order);
        await repository.SaveChangesAsync();

        return Results.Ok(order);
    }

    private static async Task<IResult> DeleteOrder(int id, IRepository<Order> repository)
    {
        var deleted = await repository.RemoveAsync(id);
        if (!deleted)
            return Results.NotFound();

        await repository.SaveChangesAsync();
        return Results.NoContent();
    }
}
