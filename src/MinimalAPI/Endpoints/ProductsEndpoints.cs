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

public static class ProductsEndpoints
{
    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");

        group.MapGet("/", GetAllProducts)
            .WithName("GetAllProducts")
            .WithDescription("Retrieve all products from the catalog")
            .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetProductById)
            .WithName("GetProductById")
            .WithDescription("Retrieve a specific product by ID")
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateProduct)
            .WithName("CreateProduct")
            .WithDescription("Create a new product (requires authentication)")
            .WithValidation<CreateProductRequest>()
            .RequireAuthorization()
            .Produces<Product>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithDescription("Update an existing product (requires authentication)")
            .WithValidation<UpdateProductRequest>()
            .RequireAuthorization()
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithDescription("Delete a product (requires Admin role)")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllProducts(
        IRepository<Product> repository,
        HybridCache cache,
        IOptions<CacheSettings> cacheSettings)
    {
        var products = await cache.GetOrCreateAsync(
            CacheKeyBuilder.ProductsAll(),
            async cancel => await repository.GetAllAsync(),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(cacheSettings.Value.Ttl.ProductsListMinutes)
            });

        return Results.Ok(products);
    }

    private static async Task<IResult> GetProductById(
        int id,
        IRepository<Product> repository,
        HybridCache cache,
        IOptions<CacheSettings> cacheSettings)
    {
        var product = await cache.GetOrCreateAsync(
            CacheKeyBuilder.ProductById(id),
            async cancel => await repository.GetByIdAsync(id),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(cacheSettings.Value.Ttl.ProductsItemMinutes)
            });

        return product is not null ? Results.Ok(product) : Results.NotFound();
    }

    private static async Task<IResult> CreateProduct(
        CreateProductRequest request,
        IRepository<Product> repository,
        HybridCache cache)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await repository.AddAsync(product);
        await repository.SaveChangesAsync();

        await cache.RemoveAsync(CacheKeyBuilder.ProductsAll());

        return Results.Created($"/products/{product.Id}", product);
    }

    private static async Task<IResult> UpdateProduct(
        int id,
        UpdateProductRequest request,
        IRepository<Product> repository,
        HybridCache cache)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return Results.NotFound();

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        await repository.UpdateAsync(product);
        await repository.SaveChangesAsync();

        await cache.RemoveAsync(CacheKeyBuilder.ProductById(id));
        await cache.RemoveAsync(CacheKeyBuilder.ProductsAll());

        return Results.Ok(product);
    }

    private static async Task<IResult> DeleteProduct(
        int id,
        IRepository<Product> repository,
        HybridCache cache)
    {
        var deleted = await repository.RemoveAsync(id);
        if (!deleted)
            return Results.NotFound();

        await repository.SaveChangesAsync();

        await cache.RemoveAsync(CacheKeyBuilder.ProductById(id));
        await cache.RemoveAsync(CacheKeyBuilder.ProductsAll());

        return Results.NoContent();
    }
}
