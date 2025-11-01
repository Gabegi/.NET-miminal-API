using BCrypt.Net;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MinimalAPI.Models.Requests;
using MinimalAPI.Models.Responses;
using Infrastructure.Entities;

namespace MinimalAPI.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth").WithTags("Authentication");

        group.MapPost("/login", Login)
            .WithName("Login")
            .WithDescription("Authenticate user with username and password to obtain a JWT token")
            .Produces<AuthResponse>(StatusCodes.Status200OK, contentType: "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        IRepository<User> userRepository,
        IJwtTokenService tokenService)
    {
        // Find user by username
        var users = await userRepository.FindAsync(u => u.Username == request.Username);
        var user = users.FirstOrDefault();

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        if (!user.IsActive)
        {
            return Results.BadRequest(new AuthResponse
            {
                Success = false,
                Message = "User account is inactive"
            });
        }

        // Generate token
        var token = tokenService.GenerateToken(user);

        // Update last login
        user.LastLogin = DateTime.UtcNow;
        await userRepository.UpdateAsync(user);
        await userRepository.SaveChangesAsync();

        return Results.Ok(new AuthResponse
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            User = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.Name).ToList()
            }
        });
    }
}
