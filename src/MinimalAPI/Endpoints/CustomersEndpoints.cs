using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using MinimalAPI.Extensions;
using MinimalAPI.Models.Requests;

namespace MinimalAPI.Endpoints;

public static class CustomersEndpoints
{
    public static void MapCustomers(this WebApplication app)
    {
        var group = app.MapGroup("/customers")
            .WithTags("Customers");

        group.MapGet("/", GetAllCustomers)
            .WithName("GetAllCustomers")
            .WithDescription("Retrieve all customers")
            .Produces<List<Customer>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetCustomerById)
            .WithName("GetCustomerById")
            .WithDescription("Retrieve a specific customer by ID")
            .Produces<Customer>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateCustomer)
            .WithName("CreateCustomer")
            .WithDescription("Create a new customer (requires authentication)")
            .WithValidation<CreateCustomerRequest>()
            .RequireAuthorization()
            .Produces<Customer>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id}", UpdateCustomer)
            .WithName("UpdateCustomer")
            .WithDescription("Update an existing customer (requires authentication)")
            .WithValidation<UpdateCustomerRequest>()
            .RequireAuthorization()
            .Produces<Customer>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id}", DeleteCustomer)
            .WithName("DeleteCustomer")
            .WithDescription("Delete a customer (requires Admin role)")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllCustomers(IRepository<Customer> repository)
    {
        var customers = await repository.GetAllAsync();
        return Results.Ok(customers);
    }

    private static async Task<IResult> GetCustomerById(int id, IRepository<Customer> repository)
    {
        var customer = await repository.GetByIdAsync(id);
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    }

    private static async Task<IResult> CreateCustomer(CreateCustomerRequest request, IRepository<Customer> repository)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email
        };

        await repository.AddAsync(customer);
        await repository.SaveChangesAsync();

        return Results.Created($"/customers/{customer.Id}", customer);
    }

    private static async Task<IResult> UpdateCustomer(int id, UpdateCustomerRequest request, IRepository<Customer> repository)
    {
        var customer = await repository.GetByIdAsync(id);
        if (customer is null)
            return Results.NotFound();

        customer.Name = request.Name;
        customer.Email = request.Email;

        await repository.UpdateAsync(customer);
        await repository.SaveChangesAsync();

        return Results.Ok(customer);
    }

    private static async Task<IResult> DeleteCustomer(int id, IRepository<Customer> repository)
    {
        var deleted = await repository.RemoveAsync(id);
        if (!deleted)
            return Results.NotFound();

        await repository.SaveChangesAsync();
        return Results.NoContent();
    }
}
