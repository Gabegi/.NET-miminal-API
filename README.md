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

### Phase 7: Global Exception Handling ✓
- [x] Custom exception middleware
- [x] Custom exception types (EntityNotFoundException, ValidationException, UnauthorizedAccessException)
- [x] ProblemDetails-style error responses
- [x] Correlation ID tracking and propagation
- [x] Graceful error handling with meaningful messages
- [x] Exception logging with Serilog integration

### Phase 8: Swagger/OpenAPI Documentation ✓
- [x] Swagger UI integration (served at root)
- [x] OpenAPI metadata on all endpoints
- [x] Endpoint descriptions and summaries
- [x] JWT Bearer authentication documentation
- [x] Response type documentation with status codes
- [x] Endpoint grouping by tags (Products, Customers, Orders, Authentication)
- [x] Production-safe (Swagger disabled in non-dev environments)

### Phase 8a: Structured Logging ✓
- [x] Serilog integration with v9.0.0
- [x] Request/Response logging middleware with performance metrics
- [x] File and console sinks with rolling intervals
- [x] Structured context enrichment (EnvironmentUserName, MachineName)
- [x] Correlation ID tracking through request pipeline
- [x] Performance metrics logging (request duration)
- [x] Proper exception logging with stack traces

### Phase 9: Production-Grade Hybrid Caching ⏳
- [ ] IHybridCache setup (L1 in-memory + L2 distributed)
- [ ] Cache key builder utility class
- [ ] Differentiated TTL configuration (Products: 60min, Customers: 60min, Orders: 10min, Lists: 5min)
- [ ] Smart cache invalidation strategy:
  - [ ] CREATE: Invalidate `*:all` and `*:page:*` keys
  - [ ] UPDATE: Invalidate specific item + related collections
  - [ ] DELETE: Same as UPDATE
- [ ] Pagination caching (cache by page: `product:page:1`)
- [ ] Cache stampede protection via GetOrCreateAsync lock
- [ ] Cache versioning (bump on DTO changes)
- [ ] CacheInvalidationService for consistent invalidation
- [ ] CacheWarmupService for startup data loading
- [ ] Redis health check endpoint (`/health/cache`)
- [ ] Cache metrics: hit/miss rates, L1 memory usage
- [ ] MessagePack serialization option (performance)
- [ ] Redis fallback: L1-only when Redis unavailable
- [ ] Integration tests for cache invalidation scenarios
- [ ] Documentation of cached endpoints
- [ ] Production monitoring dashboard

### Phase 10: Advanced Features ⏳
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

### Phase 11: Testing & DevOps ⏳
- [ ] Unit tests with xUnit
- [ ] Integration tests with WebApplicationFactory
- [ ] Cache invalidation tests
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
- **Swashbuckle.AspNetCore** (9.0.6) - Swagger/OpenAPI documentation
- **Serilog.AspNetCore** (9.0.0) - Structured logging integration
- **Serilog.Sinks.Console** (6.0.0) - Console logging output
- **Serilog.Sinks.File** (7.0.0) - File logging with rolling intervals
- **Serilog.Enrichers.Environment** (3.0.1) - Contextual enrichment
- **Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions** (9.0.10) - Health check support

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

## ⚡ Hybrid Caching Strategy (Phase 9)

### Overview
Phase 9 implements **IHybridCache** - Microsoft's unified caching abstraction for .NET 9 that combines:
- **L1 Cache**: In-memory (fast local access)
- **L2 Cache**: Distributed Redis (scalable, shared across instances)

### Cache Key Naming Convention
```
Cache Version: v1 (bump when DTOs change)
Products:     v1:product:all, v1:product:{id}, v1:product:page:{n}
Customers:    v1:customer:all, v1:customer:{id}, v1:customer:page:{n}
Orders:       v1:order:all, v1:order:{id}, v1:order:customer:{customerId}
```

### Differentiated TTL Configuration
```json
{
  "CacheSettings": {
    "Enabled": true,
    "Products": { "ttlMinutes": 60 },      // Stable data
    "Customers": { "ttlMinutes": 60 },     // Stable data
    "Orders": { "ttlMinutes": 10 },        // Frequently changing
    "Lists": { "ttlMinutes": 5 }           // High cardinality
  }
}
```

### Smart Invalidation Strategy
**CREATE Operation:**
- Invalidate `*:all` list cache
- Invalidate all `*:page:*` keys

**UPDATE Operation:**
- Invalidate specific item: `{type}:{id}`
- Invalidate all lists: `{type}:all`
- Invalidate related collections (e.g., `order:customer:{customerId}`)

**DELETE Operation:**
- Same as UPDATE

### Cache Stampede Protection
- `GetOrCreateAsync()` uses built-in locking mechanism
- Concurrent cache misses wait for first result, preventing DB overload

### Configuration Details
```csharp
services.AddHybridCache(options =>
{
    options.MaximumPayloadBytes = 1024 * 1024;     // 1MB per entry
    options.MaximumKeyLength = 256;
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };
});
```

### Cached Endpoints
```
✅ GET /products           → Cache all products
✅ GET /products/{id}      → Cache by ID
✅ GET /products?page=n    → Cache by page
✅ GET /customers          → Cache all customers
✅ GET /customers/{id}     → Cache by ID
✅ GET /orders             → Cache all orders
✅ GET /orders/{id}        → Cache by ID
✅ GET /orders/customer/{customerId} → Cache by customer
❌ POST, PUT, DELETE       → Invalidate cache, don't cache
```

### Resilience & Fallback
- **Redis Down?** → Automatic fallback to L1 cache only
- **Connection Error?** → Logged with Serilog, graceful degradation
- **Health Check**: `/health/cache` endpoint monitors Redis connectivity

### Monitoring & Metrics
- Cache hit/miss rates logged structurally
- L1 memory usage monitoring
- Redis connection health checks
- Alert on hit rate < threshold (configurable)

### What to Cache vs. Don't Cache
```
✅ Cache:
   - GET endpoints with expensive queries
   - Frequently accessed data
   - Read-heavy operations

❌ Don't Cache:
   - Real-time data (prices changing per request)
   - User-specific data with high cardinality
   - Very large result sets (>10MB)
   - Data requiring sub-second freshness
```

### Implementation Components
1. **CacheKeyBuilder** - Consistent key naming utility
2. **CacheInvalidationService** - Centralized invalidation logic
3. **CacheWarmupService** - App startup data preloading
4. **CacheSettings** - Configuration class
5. **Redis Health Check** - Distributed cache monitoring
6. **Serilog Enrichment** - Cache metrics in structured logs

### Observability & Instrumentation

The only missing piece is **observability**—instrument cache operations to measure:

#### **Cache Hit Rate by Endpoint**
```csharp
// In RequestResponseLoggingMiddleware or CacheService
_logger.LogInformation(
    "Cache operation: {CacheKey} | Hit: {IsHit} | Duration: {DurationMs}ms | Endpoint: {Endpoint}",
    cacheKey,
    wasHit,
    stopwatch.ElapsedMilliseconds,
    context.Request.Path);

// Structured properties for aggregation:
// - CacheKey (product:all, product:123, etc.)
// - IsHit (true/false)
// - DurationMs
// - Endpoint (/products, /customers, etc.)
// - ResponseTime
// - StatusCode
```

#### **Average Response Time (Cached vs Uncached)**
```csharp
// Log cache status with response times
var stopwatch = Stopwatch.StartNew();
var cachedValue = await _cache.GetOrCreateAsync(key, factory);
stopwatch.Stop();

_logger.LogInformation(
    "Response metrics | Endpoint: {Endpoint} | CacheStatus: {Status} | ResponseTime: {ResponseTimeMs}ms | Size: {SizeBytes}",
    endpoint,
    wasFromCache ? "HIT" : "MISS",
    stopwatch.ElapsedMilliseconds,
    cachedValue.Length);

// Calculate aggregated metrics:
// - Avg response time for cached requests
// - Avg response time for cache misses (DB queries)
// - Ratio comparison (cached typically 10-100x faster)
```

#### **Redis Connection Failures**
```csharp
// In CacheService or health check
try
{
    await _cache.GetAsync<T>(key);
}
catch (RedisConnectionException ex)
{
    _logger.LogError(ex,
        "Redis connection failed | Fallback to L1 cache | Key: {CacheKey} | Error: {ErrorMessage}",
        key,
        ex.Message);

    // Log severity for alerting
    // Properties for filtering:
    // - RedisConnectionFailed: true
    // - FallbackMode: L1Only
    // - Timestamp
}
```

#### **Recommended Metrics to Track**
```
1. Cache Hit Rate by Endpoint
   - products:all → 85% hit rate
   - products:{id} → 72% hit rate
   - orders:all → 45% hit rate (more invalidations)
   - orders:customer:{id} → 60% hit rate

2. Response Time Comparison
   - /products cached: 2ms
   - /products uncached (DB): 150ms
   - Improvement: 75x faster

3. Redis Connectivity
   - Connection status (up/down)
   - Latency to Redis (p50, p95, p99)
   - Failed connection attempts
   - Fallback to L1 frequency

4. Cache Efficiency
   - Items in L1 cache (memory usage)
   - Items in L2 cache (Redis size)
   - Eviction count (LRU)
   - Serialization overhead (JSON vs MessagePack)

5. Invalidation Events
   - Cache clears per minute
   - Invalidation latency
   - Cascade invalidations (1 update → N invalidations)
```

#### **Implementation Pattern**
```csharp
public class CacheInstrumentationMiddleware
{
    private readonly ILogger<CacheInstrumentationMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var cacheKey = GenerateCacheKey(context.Request);
        var stopwatch = Stopwatch.StartNew();

        var result = await _cache.GetOrCreateAsync(cacheKey, async () =>
        {
            _logger.LogInformation(
                "Cache miss | Key: {Key} | Endpoint: {Endpoint}",
                cacheKey,
                context.Request.Path);

            return await FetchFromDatabase();
        });

        stopwatch.Stop();

        // Structured logging with all metrics
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            { "CacheKey", cacheKey },
            { "Endpoint", context.Request.Path },
            { "ResponseTimeMs", stopwatch.ElapsedMilliseconds },
            { "DataSizeBytes", SerializeSize(result) },
            { "Timestamp", DateTime.UtcNow }
        }))
        {
            _logger.LogInformation(
                "Cache operation completed | Status: {Status} | Duration: {DurationMs}ms",
                wasFromCache ? "HIT" : "MISS",
                stopwatch.ElapsedMilliseconds);
        }
    }
}
```

#### **Monitoring Dashboard (Using Serilog + ELK/Grafana)**
```
Real-time metrics to display:
├── Cache Hit Rate Gauge (target: >80%)
├── Response Time Distribution (cached vs uncached)
├── Redis Connection Status (green/red)
├── Cache Memory Usage (L1 in-memory size)
├── Invalidation Rate (operations/min)
├── Cache Efficiency Score (hit rate * speed improvement)
└── Alerts:
    ├── Hit rate < 70% → Investigate TTL settings
    ├── Redis down → Check connection logs
    ├── Response time spike → Check DB query performance
    └── High invalidation rate → May indicate cache thrashing
```

#### **Alerting Rules**
```yaml
Alerts to configure:
1. RedisConnectionFailure
   - Trigger: Redis connection error count > 5 in 5min
   - Action: Page on-call, check Redis health

2. LowCacheHitRate
   - Trigger: Cache hit rate < 70% for 10min
   - Action: Log alert, review TTL configuration

3. HighResponseTime
   - Trigger: P95 response time > 500ms
   - Action: Check if cache is stale/invalidated frequently

4. CacheMemoryPressure
   - Trigger: L1 cache memory > 90% limit
   - Action: Review cache size configuration, consider TTL reduction
```

#### **Querying Metrics with Serilog**
```csharp
// Example: Get cache hit rate for last hour
// Using structured logging properties
var hitRate = await _logProvider.QueryAsync(
    filter: e => e.Properties["Endpoint"] == "/products" &&
                  e.Timestamp > DateTime.UtcNow.AddHours(-1),
    aggregation: e => new {
        TotalRequests = e.Count(),
        CacheHits = e.Count(x => x.Properties["CacheStatus"] == "HIT"),
        HitRate = e.Count(x => x.Properties["CacheStatus"] == "HIT") / e.Count() * 100
    }
);
```

### Production Considerations
- Start with JSON serialization (easier debugging)
- Upgrade to MessagePack if metrics show serialization bottleneck (5-10x faster)
- Monitor for 1-2 weeks, adjust TTLs based on actual hit rates
- Document cache behavior changes in release notes
- Plan cache versioning strategy for deployments
- **Set up observability dashboard immediately** (don't wait for production issues)
- **Export metrics to monitoring system** (Grafana, DataDog, New Relic, etc.)
- **Create runbook for cache troubleshooting** based on metrics

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

1. **Phase 9**: Implement production-grade hybrid caching with IHybridCache
   - Cache key builder utility
   - Smart invalidation strategies
   - Redis integration for distributed caching
   - Cache metrics and monitoring
2. **Phase 10**: Add advanced features (pagination, filtering, API versioning)
3. **Phase 11**: Set up comprehensive testing and CI/CD pipelines

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
**Status:** Phase 8a Complete - Structured Logging with Serilog ✓
**Completed Phases:** Database, Repository, CORS, Secrets, Validation, JWT Auth, Exception Handling, OpenAPI/Swagger, Structured Logging
**Next Phase:** Phase 9 - Production-Grade Hybrid Caching
