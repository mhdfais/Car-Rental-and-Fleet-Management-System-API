using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.enums;
using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace car_rental_and_fleet_management.services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService 
{
    public async Task RegisterUser(RegisterRequestDto dto)
    {
        if(await context.Users.AnyAsync(u=>u.Email == dto.Email))
        {
            throw new InvalidOperationException("user with this email already exists...");
        }

        string passwordHash=BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newUser=new User
        {
            Id=Guid.NewGuid(),
            Name=dto.Name,
            Email=dto.Email,
            Password=passwordHash,
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
    }

    public async Task<AuthResponseDto> LoginUser(LoginRequestDto dto)
    {
        var user=await context.Users.FirstOrDefaultAsync(u=>u.Email==dto.Email);
        if (user==null) throw new UnauthorizedAccessException("invalid email");

        if(!BCrypt.Net.BCrypt.Verify(dto.Password,user.Password)) throw new UnauthorizedAccessException("invalid password");

        string token=GenerateToken(user);

        return new AuthResponseDto{
            Name=user.Name,
            Email=user.Email, 
            Role=user.Role.ToString(),
            Token= token
            };
    }

    public async Task RegisterStaff(RegisterRequestDto dto)
    {
         if(await context.Users.AnyAsync(u=>u.Email == dto.Email))
        {
            throw new InvalidOperationException("user with this email already exists...");
        }

        string passwordHash=BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newStaff=new User
        {
            Name=dto.Name,
            Password=passwordHash,
            Email=dto.Email,
            Role=UserRole.Staff
        };

        await context.Users.AddAsync(newStaff);
        await context.SaveChangesAsync();
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Modern standard claim type for IDs
            new Claim(ClaimTypes.Role,user.Role.ToString()),
        };

        // Fallback validation if configuration keys are missing
        var secretKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Secret Key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), //  Swapped to UtcNow for global compatibility
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}