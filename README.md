# .NET Minimal API Template

A production-ready .NET 9.0 Minimal API with clean architecture, JWT auth, hybrid caching, and FluentValidation.

## Tech Stack

- .NET 9.0 / Entity Framework Core 9.0 (SQL Server)
- JWT Authentication + Role-based Authorization
- HybridCache (L1 in-memory + optional L2 Redis)
- FluentValidation / Serilog / Swagger

## Project Structure

```
src/
├── Infrastructure/          # Data access layer
│   ├── Entities/            # Product, Customer, Order, User, Role
│   ├── Repositories/        # Generic IRepository<T>
│   ├── Services/            # JWT token service
│   └── Data/                # DbContext, migrations, seeding
│
├── MinimalAPI/              # Presentation layer
│   ├── Endpoints/           # Auth, Products, Customers, Orders
│   ├── Middleware/           # Exception handling, request logging
│   ├── Filters/             # Validation filter
│   ├── Validators/          # FluentValidation rules
│   ├── Configuration/       # Settings classes
│   ├── Utilities/           # CacheKeyBuilder
│   └── Program.cs           # App configuration
│
└── MinimalAPIPlayground.sln
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or Express)

### Setup

```bash
cd src

# Configure secrets
cd MinimalAPI
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=MinimalApiDb;Trusted_Connection=true;"
dotnet user-secrets set "JwtSettings:Secret" "your-super-secret-key-minimum-32-characters-long"

# Run (migrations apply automatically)
dotnet run
```

### Default Credentials

| User  | Username | Password   | Role  |
|-------|----------|------------|-------|
| Admin | admin    | Admin@123  | Admin |
| User  | user     | User@123   | User  |

## API Endpoints

| Method | Route                          | Auth       |
|--------|--------------------------------|------------|
| POST   | `/auth/login`                  | Public     |
| GET    | `/products`                    | Public     |
| GET    | `/products/{id}`               | Public     |
| POST   | `/products`                    | Authenticated |
| PUT    | `/products/{id}`               | Authenticated |
| DELETE | `/products/{id}`               | Admin only |
| GET    | `/customers`                   | Public     |
| GET    | `/customers/{id}`              | Public     |
| POST   | `/customers`                   | Authenticated |
| PUT    | `/customers/{id}`              | Authenticated |
| DELETE | `/customers/{id}`              | Admin only |
| GET    | `/orders`                      | Public     |
| GET    | `/orders/{id}`                 | Public     |
| GET    | `/orders/customer/{customerId}`| Public     |
| POST   | `/orders`                      | Authenticated |
| PUT    | `/orders/{id}`                 | Authenticated |
| DELETE | `/orders/{id}`                 | Admin only |

## Features

- **Auth**: JWT Bearer tokens, BCrypt password hashing, role-based policies (AdminOnly, UserOrAdmin)
- **Validation**: FluentValidation on all request models via `.WithValidation<T>()` endpoint filter
- **Caching**: HybridCache on all GET endpoints with entity-specific TTLs, cache invalidation on mutations
- **Error Handling**: Global exception middleware with ProblemDetails responses, correlation ID tracking
- **Logging**: Serilog with console + rolling file sinks, request/response logging with performance metrics
- **CORS**: Environment-based configuration (dev/prod)
- **Swagger**: OpenAPI docs at root in development, with JWT auth support

## Useful Commands

```bash
dotnet build                    # Build solution
dotnet run --project MinimalAPI # Run the API
dotnet test                     # Run tests

# EF Core migrations
dotnet ef migrations add <Name> --project Infrastructure --startup-project MinimalAPI
dotnet ef database update --project Infrastructure --startup-project MinimalAPI
```

## License

MIT
