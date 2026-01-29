using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using MinimalAPI.Configuration;
using MinimalAPI.Extensions;
using MinimalAPI.Models.Requests;
using MinimalAPI.Utilities;

namespace MinimalAPI.Endpoints;

public static class OrdersEndpoints
{
    public static void MapOrders(this WebApplication app)
    {
        var group = app.MapGroup("/orders")
            .WithTags("Orders");

        group.MapGet("/", GetAllOrders)
            .WithName("GetAllOrders")
            .WithDescription("Retrieve all orders")
            .Produces<List<Order>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetOrderById)
            .WithName("GetOrderById")
            .WithDescription("Retrieve a specific order by ID")
            .Produces<Order>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/customer/{customerId}", GetOrdersByCustomer)
            .WithName("GetOrdersByCustomer")
            .WithDescription("Retrieve all orders for a specific customer")
            .Produces<List<Order>>(StatusCodes.Status200OK);

        group.MapPost("/", CreateOrder)
            .WithName("CreateOrder")
            .WithDescription("Create a new order (requires authentication)")
            .WithValidation<CreateOrderRequest>()
            .RequireAuthorization()
            .Produces<Order>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id}", UpdateOrder)
            .WithName("UpdateOrder")
            .WithDescription("Update an existing order (requires authentication)")
            .WithValidation<UpdateOrderRequest>()
            .RequireAuthorization()
            .Produces<Order>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id}", DeleteOrder)
            .WithName("DeleteOrder")
            .WithDescription("Delete an order (requires Admin role)")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllOrders(
        IRepository<Order> repository,
        HybridCache cache,
        IOptions<CacheSettings> cacheSettings)
    {
        var orders = await cache.GetOrCreateAsync(
            CacheKeyBuilder.OrdersAll(),
            async cancel => await repository.GetAllAsync(),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(cacheSettings.Value.Ttl.OrdersListMinutes)
            });

        return Results.Ok(orders);
    }

    private static async Task<IResult> GetOrderById(
        int id,
        IRepository<Order> repository,
        HybridCache cache,
        IOptions<CacheSettings> cacheSettings)
    {
        var order = await cache.GetOrCreateAsync(
            CacheKeyBuilder.OrderById(id),
            async cancel => await repository.GetByIdAsync(id),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(cacheSettings.Value.Ttl.OrdersItemMinutes)
            });

        return order is not null ? Results.Ok(order) : Results.NotFound();
    }

    private static async Task<IResult> GetOrdersByCustomer(
        int customerId,
        IRepository<Order> repository,
        HybridCache cache,
        IOptions<CacheSettings> cacheSettings)
    {
        var orders = await cache.GetOrCreateAsync(
            CacheKeyBuilder.OrdersByCustomer(customerId),
            async cancel => await repository.FindAsync(o => o.CustomerId == customerId),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(cacheSettings.Value.Ttl.OrdersByCustomerMinutes)
            });

        return Results.Ok(orders);
    }

    private static async Task<IResult> CreateOrder(
        CreateOrderRequest request,
        IRepository<Order> repository,
        HybridCache cache)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            Items = request.Items
        };

        await repository.AddAsync(order);
        await repository.SaveChangesAsync();

        await cache.RemoveAsync(CacheKeyBuilder.OrdersAll());
        await cache.RemoveAsync(CacheKeyBuilder.OrdersByCustomer(order.CustomerId));

        return Results.Created($"/orders/{order.Id}", order);
    }

    private static async Task<IResult> UpdateOrder(
        int id,
        UpdateOrderRequest request,
        IRepository<Order> repository,
        HybridCache cache)
    {
        var order = await repository.GetByIdAsync(id);
        if (order is null)
            return Results.NotFound();

        var oldCustomerId = order.CustomerId;

        order.CustomerId = request.CustomerId;
        order.Items = request.Items;

        await repository.UpdateAsync(order);
        await repository.SaveChangesAsync();

        await cache.RemoveAsync(CacheKeyBuilder.OrderById(id));
        await cache.RemoveAsync(CacheKeyBuilder.OrdersAll());
        await cache.RemoveAsync(CacheKeyBuilder.OrdersByCustomer(oldCustomerId));

        if (oldCustomerId != request.CustomerId)
            await cache.RemoveAsync(CacheKeyBuilder.OrdersByCustomer(request.CustomerId));

        return Results.Ok(order);
    }

    private static async Task<IResult> DeleteOrder(
        int id,
        IRepository<Order> repository,
        HybridCache cache)
    {
        var order = await repository.GetByIdAsync(id);
        if (order is null)
            return Results.NotFound();

        var customerId = order.CustomerId;

        await repository.RemoveAsync(order);
        await repository.SaveChangesAsync();

        await cache.RemoveAsync(CacheKeyBuilder.OrderById(id));
        await cache.RemoveAsync(CacheKeyBuilder.OrdersAll());
        await cache.RemoveAsync(CacheKeyBuilder.OrdersByCustomer(customerId));

        return Results.NoContent();
    }
}
