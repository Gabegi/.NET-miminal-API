using EShop.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Handles seeding of initial data for development and testing.
/// </summary>
public class DataSeeder
{
    public void SeedData(ModelBuilder modelBuilder)
    {
        SeedProducts(modelBuilder);
        SeedCustomers(modelBuilder);
        SeedOrders(modelBuilder);
        SeedOrderItems(modelBuilder);
    }

    private static void SeedProducts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "High-performance laptop",
                Price = 1299.99m
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Description = "Wireless mouse",
                Price = 29.99m
            },
            new Product
            {
                Id = 3,
                Name = "Keyboard",
                Description = "Mechanical keyboard",
                Price = 99.99m
            },
            new Product
            {
                Id = 4,
                Name = "Monitor",
                Description = "4K monitor",
                Price = 399.99m
            },
            new Product
            {
                Id = 5,
                Name = "Headphones",
                Description = "Noise-cancelling headphones",
                Price = 199.99m
            }
        );
    }

    private static void SeedCustomers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com"
            },
            new Customer
            {
                Id = 2,
                Name = "Jane Smith",
                Email = "jane@example.com"
            },
            new Customer
            {
                Id = 3,
                Name = "Bob Johnson",
                Email = "bob@example.com"
            }
        );
    }

    private static void SeedOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow.AddDays(-5)
            },
            new Order
            {
                Id = 2,
                CustomerId = 2,
                OrderDate = DateTime.UtcNow.AddDays(-2)
            },
            new Order
            {
                Id = 3,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow
            }
        );
    }

    private static void SeedOrderItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 1299.99m
            },
            new OrderItem
            {
                OrderId = 1,
                ProductId = 2,
                Quantity = 2,
                UnitPrice = 29.99m
            },
            new OrderItem
            {
                OrderId = 2,
                ProductId = 3,
                Quantity = 1,
                UnitPrice = 99.99m
            },
            new OrderItem
            {
                OrderId = 3,
                ProductId = 4,
                Quantity = 1,
                UnitPrice = 399.99m
            }
        );
    }
}
