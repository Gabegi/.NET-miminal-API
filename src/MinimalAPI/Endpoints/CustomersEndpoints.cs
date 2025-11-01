using Infrastructure.Entities;
using Infrastructure.Repositories;
using MinimalAPI.Extensions;
using MinimalAPI.Models.Requests;

namespace MinimalAPI.Endpoints;

public static class CustomersEndpoints
{
    public static void MapCustomers(this WebApplication app)
    {
        var group = app.MapGroup("/customers");

        group.MapGet("/", GetAllCustomers);
        group.MapGet("/{id}", GetCustomerById);
        group.MapPost("/", CreateCustomer).WithValidation<CreateCustomerRequest>();
        group.MapPut("/{id}", UpdateCustomer).WithValidation<UpdateCustomerRequest>();
        group.MapDelete("/{id}", DeleteCustomer);
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
