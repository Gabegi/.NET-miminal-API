using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using MinimalAPI.Extensions;
using MinimalAPI.Models.Requests;

namespace MinimalAPI.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProducts(this WebApplication app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/", GetAllProducts);
        group.MapGet("/{id}", GetProductById);
        group.MapPost("/", CreateProduct)
            .WithValidation<CreateProductRequest>()
            .RequireAuthorization();
        group.MapPut("/{id}", UpdateProduct)
            .WithValidation<UpdateProductRequest>()
            .RequireAuthorization();
        group.MapDelete("/{id}", DeleteProduct)
            .RequireAuthorization("AdminOnly");
    }

    private static async Task<IResult> GetAllProducts(IRepository<Product> repository)
    {
        var products = await repository.GetAllAsync();
        return Results.Ok(products);
    }

    private static async Task<IResult> GetProductById(int id, IRepository<Product> repository)
    {
        var product = await repository.GetByIdAsync(id);
        return product is not null ? Results.Ok(product) : Results.NotFound();
    }

    private static async Task<IResult> CreateProduct(CreateProductRequest request, IRepository<Product> repository)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await repository.AddAsync(product);
        await repository.SaveChangesAsync();

        return Results.Created($"/products/{product.Id}", product);
    }

    private static async Task<IResult> UpdateProduct(int id, UpdateProductRequest request, IRepository<Product> repository)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return Results.NotFound();

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        await repository.UpdateAsync(product);
        await repository.SaveChangesAsync();

        return Results.Ok(product);
    }

    private static async Task<IResult> DeleteProduct(int id, IRepository<Product> repository)
    {
        var deleted = await repository.RemoveAsync(id);
        if (!deleted)
            return Results.NotFound();

        await repository.SaveChangesAsync();
        return Results.NoContent();
    }
}
