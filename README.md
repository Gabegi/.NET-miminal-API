# .NET Minimal API - Enterprise Template

A production-ready .NET 9.0 Minimal API template with modern architecture, security, and best practices.

---

## 🎯 Project Overview

This is a comprehensive Minimal API template for building scalable, secure, and maintainable .NET applications. The project includes complete e-commerce domain (Products, Customers, Orders) with enterprise-grade features.

**Tech Stack:**
- .NET 9.0
- Entity Framework Core 9.0 (SQL Server)
- JWT Authentication
- FluentValidation
- BCrypt password hashing
- Generic Repository Pattern

---

## ✅ Completed Features

### Phase 1: Database & Persistence ✓
- [x] Entity Framework Core integration with SQL Server (LocalDB)
- [x] DbContext with fluent configuration
- [x] Entity configurations (IEntityTypeConfiguration)
- [x] Database migrations support
- [x] Data seeding with default products, customers, orders, users, and roles
- [x] Proper relationships (one-to-many, many-to-many)
- [x] Database constraints and indexes

### Phase 2: Repository Pattern ✓
- [x] Generic `IRepository<T>` interface
- [x] Generic `Repository<T>` implementation
- [x] CRUD operations (GetAll, GetById, Find, Add, Update, Remove)
- [x] Transaction support (BeginTransaction, CommitTransaction, RollbackTransaction)
- [x] SaveChangesAsync for persistence
- [x] Scoped DI registration
- [x] No separate Unit of Work pattern - management in repository

### Phase 3: CORS Configuration ✓
- [x] Environment-based CORS settings (Development, Production)
- [x] Development configuration with localhost:3000, 3001, 4200
- [x] Production configuration (empty origins, customizable)
- [x] CORS middleware integration
- [x] Support for credentials and custom headers

### Phase 4: Secrets Management ✓
- [x] User Secrets integration (development)
- [x] Environment-based configuration loading
- [x] JwtSettings in appsettings.json
- [x] Connection strings configurable via secrets
- [x] No sensitive data in source code
- [x] appsettings.Development.json for dev overrides

### Phase 5: Input Validation ✓
- [x] FluentValidation framework (v12.0.0)
- [x] Custom validators for all request models
- [x] Inheritance-based validator structure
- [x] ValidationFilter with IEndpointFilter
- [x] Extension method `.WithValidation<T>()`
- [x] ProblemDetails error responses
- [x] Grouped error messages by property

### Phase 6: JWT Authentication & Authorization ✓
- [x] User and Role entities
- [x] Many-to-many relationship (User ↔ Role)
- [x] JwtTokenService for token generation
- [x] JWT Bearer authentication configured
- [x] Token validation (issuer, audience, lifetime, signature)
- [x] Login endpoint (`POST /auth/login`)
- [x] Password hashing with BCrypt
- [x] Role claims in JWT payload
- [x] Default seeded users (admin/user)

### Phase 6a: Authorization Policies ✓
- [x] "AdminOnly" policy (requires Admin role)
- [x] "UserOrAdmin" policy (requires Admin or User role)
- [x] Endpoint authorization attributes
- [x] GET endpoints public, POST/PUT protected, DELETE admin-only
- [x] Authorization middleware configured

### API Endpoints

#### Authentication
- `POST /auth/login` - Login with username/password, returns JWT token

#### Products (Protected)
- `GET /products` - List all products (public)
- `GET /products/{id}` - Get product by ID (public)
- `POST /products` - Create product (requires authentication)
- `PUT /products/{id}` - Update product (requires authentication)
- `DELETE /products/{id}` - Delete product (requires Admin role)

#### Customers (Protected)
- `GET /customers` - List all customers (public)
- `GET /customers/{id}` - Get customer by ID (public)
- `POST /customers` - Create customer (requires authentication)
- `PUT /customers/{id}` - Update customer (requires authentication)
- `DELETE /customers/{id}` - Delete customer (requires Admin role)

#### Orders (Protected)
- `GET /orders` - List all orders (public)
- `GET /orders/{id}` - Get order by ID (public)
- `GET /orders/customer/{customerId}` - Get orders by customer (public)
- `POST /orders` - Create order (requires authentication)
- `PUT /orders/{id}` - Update order (requires authentication)
- `DELETE /orders/{id}` - Delete order (requires Admin role)

---

## 📋 Remaining Features (Planned)

### Phase 7: Global Exception Handling ⏳
- [ ] Custom exception middleware
- [ ] Custom exception types (EntityNotFoundException, ValidationException, etc.)
- [ ] ProblemDetails standard error responses
- [ ] Error logging to external service
- [ ] Correlation IDs for request tracing
- [ ] Graceful error handling with meaningful messages

### Phase 8: Swagger/OpenAPI Documentation ⏳
- [ ] Swagger UI integration
- [ ] OpenAPI metadata on endpoints
- [ ] Request/Response examples
- [ ] Authorization header documentation
- [ ] Endpoint descriptions and summaries
- [ ] Type documentation with XML comments

### Phase 9: Structured Logging ⏳
- [ ] Serilog integration
- [ ] Request/Response logging middleware
- [ ] File and console sinks
- [ ] Correlation IDs
- [ ] Performance metrics logging
- [ ] Database query logging
- [ ] Structured log aggregation

### Phase 10: Caching & Health Checks ⏳
- [ ] IMemoryCache for in-process caching
- [ ] Redis distributed caching
- [ ] Cache invalidation strategies
- [ ] HTTP response caching headers
- [ ] Health check endpoints (`/health`, `/health/ready`)
- [ ] Database health checks
- [ ] Custom health check policies

### Phase 11: Advanced Features ⏳
- [ ] Pagination with skip/take
- [ ] Filtering with LINQ predicates
- [ ] Sorting by column
- [ ] API versioning (v1, v2 routes)
- [ ] Soft deletes for entities
- [ ] Audit logging (created by, created at, modified by, modified at)
- [ ] Request/Response compression (gzip, brotli)
- [ ] Rate limiting
- [ ] Request timeout handling
- [ ] Batch operations
- [ ] Database query optimization
- [ ] Lazy loading configuration

### Phase 12: Testing & DevOps ⏳
- [ ] Unit tests with xUnit
- [ ] Integration tests with WebApplicationFactory
- [ ] Health check tests
- [ ] Authentication/Authorization tests
- [ ] Docker support
- [ ] Docker Compose for local development
- [ ] CI/CD pipeline (GitHub Actions / Azure DevOps)
- [ ] Code coverage reporting
- [ ] Performance testing

---

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 / VS Code

### Clone & Setup

```bash
git clone <repo-url>
cd src
dotnet build
```

### Setup Database

```bash
# Create migration (first time)
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project MinimalAPI

# Apply migrations (automatic on app startup)
dotnet run --project MinimalAPI
```

### Setup User Secrets (Development)

```bash
cd MinimalAPI

# Initialize user secrets
dotnet user-secrets init

# Set connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=MinimalApiDb;Trusted_Connection=true;"

# Set JWT secret (minimum 32 characters)
dotnet user-secrets set "JwtSettings:Secret" "your-super-secret-key-minimum-32-characters-long"

# View all secrets
dotnet user-secrets list
```

### Run Application

```bash
dotnet run --project MinimalAPI
```

API will be available at: `https://localhost:5001`

### Default Login Credentials (Development)

**Admin User:**
```json
POST /auth/login
{
  "username": "admin",
  "password": "Admin@123"
}
```

**Regular User:**
```json
POST /auth/login
{
  "username": "user",
  "password": "User@123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGc...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "roles": ["Admin"]
  }
}
```

---

## 🏗️ Project Structure

```
src/
├── Infrastructure/
│   ├── Configuration/
│   │   ├── JwtSettings.cs              # JWT configuration
│   │   ├── CorsSettings.cs             # CORS configuration
│   │   └── ...
│   ├── Data/
│   │   ├── ApplicationDbContext.cs     # EF Core DbContext
│   │   ├── Configuration/              # Entity configurations
│   │   │   ├── ProductEntityConfiguration.cs
│   │   │   ├── CustomerEntityConfiguration.cs
│   │   │   ├── OrderEntityConfiguration.cs
│   │   │   ├── UserEntityConfiguration.cs
│   │   │   └── RoleEntityConfiguration.cs
│   │   └── Seeding/
│   │       └── DataSeeder.cs           # Database seed data
│   ├── Entities/
│   │   ├── Product.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── User.cs
│   │   └── Role.cs
│   ├── Repositories/
│   │   ├── IRepository.cs              # Generic repository interface
│   │   └── Repository.cs               # Generic repository implementation
│   ├── Services/
│   │   ├── IJwtTokenService.cs         # JWT token generation
│   │   └── JwtTokenService.cs
│   └── Infrastructure.csproj
│
├── MinimalAPI/
│   ├── Configuration/
│   │   ├── CorsSettings.cs
│   │   └── UserSecretsTemplate.json
│   ├── Endpoints/
│   │   ├── AuthEndpoints.cs            # Authentication endpoints
│   │   ├── ProductsEndpoints.cs        # Product CRUD
│   │   ├── CustomersEndpoints.cs       # Customer CRUD
│   │   └── OrdersEndpoints.cs          # Order CRUD
│   ├── Filters/
│   │   └── ValidationFilter.cs         # Fluent validation filter
│   ├── Extensions/
│   │   └── EndpointExtensions.cs       # .WithValidation<T>() extension
│   ├── Models/
│   │   ├── Requests/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── CreateProductRequest.cs
│   │   │   ├── UpdateProductRequest.cs
│   │   │   └── ...
│   │   └── Responses/
│   │       ├── AuthResponse.cs
│   │       └── UserInfo.cs
│   ├── Validators/
│   │   ├── ProductRequestValidator.cs
│   │   ├── CustomerRequestValidator.cs
│   │   └── OrderRequestValidator.cs
│   ├── Program.cs                      # Application configuration
│   ├── appsettings.json                # Settings (no secrets)
│   ├── appsettings.Development.json    # Dev overrides
│   └── MinimalAPI.csproj
│
└── MinimalAPIPlayground.sln
```

---

## 📦 Dependencies

### Infrastructure
- **Microsoft.EntityFrameworkCore** (9.0.0) - ORM
- **Microsoft.EntityFrameworkCore.SqlServer** (9.0.0) - SQL Server provider
- **BCrypt.Net-Next** (4.0.3) - Password hashing

### MinimalAPI
- **FluentValidation.DependencyInjectionExtensions** (12.0.0) - Request validation
- **Microsoft.AspNetCore.Authentication.JwtBearer** (9.0.0) - JWT authentication
- **System.IdentityModel.Tokens.Jwt** (8.0.1) - JWT token handling
- **Microsoft.IdentityModel.Tokens** (8.0.1) - Token validation

---

## 🔐 Security Notes

### Authentication Flow
1. User sends credentials to `POST /auth/login`
2. Password verified against BCrypt hash
3. JWT token generated with user claims and roles
4. Token returned to client
5. Client includes token in `Authorization: Bearer <token>` header

### Authorization Rules
- **Public endpoints**: GET requests (list/read operations)
- **Authenticated endpoints**: POST/PUT (create/modify operations)
- **Admin-only endpoints**: DELETE (destructive operations)

### Production Deployment
- Use environment variables or Azure Key Vault for secrets
- Update CORS origins to production domains
- Enable HTTPS only
- Use strong JWT secret (minimum 32 characters)
- Implement rate limiting
- Enable request logging
- Set up monitoring and alerting

---

## 🧪 Testing

### Manual Testing with cURL

**Login:**
```bash
curl -X POST https://localhost:5001/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@123"}'
```

**Create Product (Protected):**
```bash
curl -X POST https://localhost:5001/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "name":"New Product",
    "description":"Product description",
    "price":99.99
  }'
```

**Delete Product (Admin Only):**
```bash
curl -X DELETE https://localhost:5001/products/1 \
  -H "Authorization: Bearer <admin-token>"
```

---

## 📊 Architecture Principles

### Design Patterns Used
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Service registration and resolution
- **JWT Authentication** - Token-based auth
- **Role-Based Access Control** - Authorization via roles
- **Fluent Validation** - Declarative request validation
- **Entity Configuration** - Fluent EF Core mapping

### SOLID Principles
- **S**ingle Responsibility - Each class has one reason to change
- **O**pen/Closed - Open for extension, closed for modification
- **L**iskov Substitution - Repository implementations are substitutable
- **I**nterface Segregation - Small, focused interfaces
- **D**ependency Inversion - Depend on abstractions, not concretions

### Best Practices
- Async/await throughout
- Proper error handling
- Secure password hashing
- Environment-based configuration
- No hard-coded secrets
- Comprehensive validation
- Clean code structure

---

## 🔗 Next Steps

1. **Phase 7**: Implement global exception handling middleware
2. **Phase 8**: Add Swagger/OpenAPI documentation
3. **Phase 9**: Integrate Serilog for structured logging
4. **Phase 10**: Implement caching and health checks
5. **Phase 11**: Add advanced features (pagination, filtering, versioning)
6. **Phase 12**: Set up testing and CI/CD

---

## 📚 Resources

- [Microsoft Docs: Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [FluentValidation](https://fluentvalidation.net/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [JWT.io](https://jwt.io/)
- [BCrypt.net](https://github.com/BcryptNet/bcrypt.net)

---

## 📝 License

This project is provided as-is for learning and development purposes.

---

## 👨‍💻 Development

### Useful Commands

```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Create migration
dotnet ef migrations add MigrationName --project Infrastructure --startup-project MinimalAPI

# Update database
dotnet ef database update --project Infrastructure --startup-project MinimalAPI

# View migrations
dotnet ef migrations list --project Infrastructure --startup-project MinimalAPI

# Drop database (caution!)
dotnet ef database drop --project Infrastructure --startup-project MinimalAPI

# Format code
dotnet format
```

---

**Last Updated:** November 2024
**Status:** Phase 6 Complete - JWT + Authentication & Authorization ✓
**Next Phase:** Global Exception Handling
