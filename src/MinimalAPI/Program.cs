using FluentValidation;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRandomProductRepository, RandomProductRepository>();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Map endpoints
app.MapProducts();
app.MapCustomers();
app.MapOrders();

app.Run();
