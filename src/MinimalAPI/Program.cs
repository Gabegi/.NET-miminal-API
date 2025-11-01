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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

var app = builder.Build();

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
