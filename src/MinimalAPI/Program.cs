using FluentValidation;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Configuration;
using MinimalAPI.Endpoints;
using MinimalAPI.Extensions;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for structured logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithMachineName()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/app-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

builder.Host.UseSerilog();

// Add User Secrets in development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Configure settings from configuration
var corsSettings = new CorsSettings();
builder.Configuration.GetSection(CorsSettings.SectionName).Bind(corsSettings);
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection(CorsSettings.SectionName));

// Configure JWT settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

// Configure Hybrid Caching (L1 in-memory + L2 Redis)
var cacheSettings = new CacheSettings();
builder.Configuration.GetSection(CacheSettings.SectionName).Bind(cacheSettings);
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.SectionName));

// Always register HybridCache so DI resolves in endpoints regardless of config
builder.Services.AddHybridCache(options =>
{
    if (cacheSettings.Enabled)
    {
        options.MaximumPayloadBytes = cacheSettings.MaxPayloadBytes;
        options.MaximumKeyLength = cacheSettings.MaxKeyLength;

        options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(10),
            LocalCacheExpiration = TimeSpan.FromMinutes(10 * cacheSettings.L1ToL2Ratio)
        };
    }
    else
    {
        // Disabled: zero TTL acts as pass-through (no actual caching)
        options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.Zero,
            LocalCacheExpiration = TimeSpan.Zero
        };
    }
});

// Use Redis for L2 cache if enabled and connection string is configured
if (cacheSettings.Enabled && !string.IsNullOrEmpty(cacheSettings.RedisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = cacheSettings.RedisConnectionString;
    });
}

Log.Information("Hybrid caching: {Status} | Redis: {RedisEnabled} | L1/L2 Ratio: {Ratio}",
    cacheSettings.Enabled ? "Enabled" : "Disabled (pass-through)",
    cacheSettings.Enabled && !string.IsNullOrEmpty(cacheSettings.RedisConnectionString) ? "Yes" : "No (L1 only)",
    cacheSettings.L1ToL2Ratio);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register health checks
var healthChecks = builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database", tags: new[] { "ready" });

if (cacheSettings.Enabled && !string.IsNullOrEmpty(cacheSettings.RedisConnectionString))
{
    healthChecks.AddRedis(cacheSettings.RedisConnectionString, name: "redis", tags: new[] { "ready" });
}

// Register generic repositories
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IRepository<Customer>, Repository<Customer>>();
builder.Services.AddScoped<IRepository<Order>, Repository<Order>>();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Register JWT token service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Register User repository for authentication
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Role>, Repository<Role>>();

// Configure JWT Authentication
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);

var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Register Authorization with policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserOrAdmin", policy =>
        policy.RequireRole("Admin", "User"));
});

// Register CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsSettings.PolicyName, policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins)
            .WithMethods(corsSettings.AllowedMethods)
            .WithHeaders(corsSettings.AllowedHeaders)
            .WithExposedHeaders(corsSettings.ExposedHeaders);

        if (corsSettings.AllowCredentials)
        {
            policy.AllowCredentials();
        }

        policy.SetIsOriginAllowed(origin => corsSettings.AllowedOrigins.Contains(origin));
    });
});

// Configure Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Minimal API",
        Version = "v1",
        Description = "A comprehensive enterprise-grade .NET 9.0 Minimal API template with JWT authentication, validation, exception handling, and more."
    });

    // Configure JWT Bearer authentication in Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Global exception handling (must be first)
app.UseGlobalExceptionHandling();

// Configure Swagger/OpenAPI UI (development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at root
        options.DocumentTitle = "Minimal API - OpenAPI Documentation";
    });
}

// Request/response logging middleware
app.UseRequestResponseLogging();

// Use CORS middleware
app.UseCors(corsSettings.PolicyName);

// Use Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Map endpoints
app.MapAuthEndpoints();
app.MapProducts();
app.MapCustomers();
app.MapOrders();

app.Run();
