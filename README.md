# .NET_MiminalAPI
A project to play with Minimal APIs

# Create projects

```
dotnet new web nameofproject
```

My playground to work with Minimal APIs

## TODO: Improvements & Additions

 - Auth
- mapper vs struct

### Architecture & Structure

- [ ] DI container not configured - register services in Program.cs
- [ ] Products endpoints not mapped in Program.cs:4
- [ ] Missing appsettings.json for configuration

### Missing Functionality

- [ ] No Customer/Order endpoints (entities exist but unused)
- [ ] No repository implementations for Customer/Order

- [ ] No validation (e.g., FluentValidation)
- [ ] No error handling/exception middleware
- [ ] No logging configured
- [ ] No database (currently in-memory only)

### API Improvements

- [ ] No Swagger/OpenAPI documentation
- [ ] No versioning
- [ ] No pagination for GetAll
- [ ] No filtering/sorting
- [ ] No DTOs (exposing entities directly)
- [ ] No authentication/authorization

### Code Quality

- [ ] Namespace mismatch: Order/Customer/OrderItem use `EShop.Core.Entities` vs Product uses `Core.Entities`
- [ ] ProductsEndpoints takes concrete `RandomProductRepository` instead of `IRandomProductRepository`
- [ ] No input validation on endpoints
- [ ] No unit tests

### Data Layer

- [ ] Thread-safety issues in RandomProductRepository (List<T> not thread-safe)
- [ ] No EF Core or real database
- [ ] No migrations

### DevOps

- [ ] No Docker support
- [ ] No CI/CD pipeline
- [ ] No health checks endpoint

### Missing Best Practices for .NET Minimal APIs

- [ ] FluentValidation for request validation
- [ ] MediatR/CQRS pattern for complex business logic
- [ ] Result pattern instead of throwing exceptions
- [ ] Global exception handling middleware
- [ ] Request/Response logging middleware
- [ ] Correlation IDs for request tracking
- [ ] Rate limiting (built-in .NET 7+)
- [ ] API versioning with Asp.Versioning.Http
- [ ] ProblemDetails for standardized error responses
- [ ] Output caching for GET endpoints
- [ ] CORS policy configuration
- [ ] Compression middleware (Brotli/Gzip)
- [ ] Request timeout policies
- [ ] Minimal API filters for cross-cutting concerns
- [ ] Endpoint grouping and organization by feature
- [ ] Strongly-typed configuration with IOptions pattern
- [ ] Environment-specific configuration (Development/Staging/Production)
- [ ] Serilog or similar structured logging
- [ ] OpenTelemetry for distributed tracing
- [ ] Metrics and monitoring endpoints
- [ ] Response DTOs with AutoMapper or Mapster
- [ ] Request DTOs separate from domain entities
- [ ] API documentation with XML comments
- [ ] Endpoint descriptions and metadata
- [ ] Route constraints for type safety
- [ ] Asynchronous endpoints where appropriate
- [ ] Cancellation token support
- [ ] Repository pattern with Unit of Work
- [ ] Database connection resilience (Polly)
- [ ] Migration strategy and seed data
- [ ] Integration tests with WebApplicationFactory
- [ ] API contract tests
- [ ] Load testing configuration
