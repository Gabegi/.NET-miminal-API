using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Handles seeding of initial data for development and testing.
/// </summary>
public class DataSeeder
{
    public void SeedData(ModelBuilder modelBuilder)
    {
        SeedRoles(modelBuilder);
        SeedUsers(modelBuilder);
        SeedProducts(modelBuilder);
        SeedCustomers(modelBuilder);
        SeedOrders(modelBuilder);
        SeedOrderItems(modelBuilder);
    }

    private static void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Administrator with full access" },
            new Role { Id = 2, Name = "User", Description = "Regular user with read/write access" },
            new Role { Id = 3, Name = "Guest", Description = "Guest with read-only access" }
        );
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        // Default admin password: "Admin@123"
        var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
        // Default user password: "User@123"
        var userPasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = adminPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null
            },
            new User
            {
                Id = 2,
                Username = "user",
                Email = "user@example.com",
                PasswordHash = userPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null
            }
        );

        // Assign roles (many-to-many)
        modelBuilder.Entity("UserRoles").HasData(
            new { UserId = 1, RoleId = 1 },  // admin -> Admin role
            new { UserId = 2, RoleId = 2 }   // user -> User role
        );
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
