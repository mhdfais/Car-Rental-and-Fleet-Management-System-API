using System.Text;
using System.Text.Json.Serialization;
using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.middleware;
using car_rental_and_fleet_management.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey ?? throw new InvalidOperationException("JWT Key is null"))
        ),
        ClockSkew = TimeSpan.Zero // Crucial so expired tokens drop off exactly on time
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            // 1. Terminate the default schema handling to prevent duplicate responses
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            // 2. Safely check if the failure context contains a known type string
            var isExpired = context.AuthenticateFailure != null && 
                            context.AuthenticateFailure.GetType().Name == "SecurityTokenExpiredException";

            var responseMessage = isExpired ? "Token has expired" : "Token is missing or invalid";

            // 3. Send a clean, unified response structure
            await context.Response.WriteAsJsonAsync(new
            {
                message = responseMessage
            });
        }
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IVehicleManagementService,VehicleManagementService>();
builder.Services.AddScoped<IVehicleBrandService,VehicleBrandService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This converter forces all Enums to output as readable text strings instead of numbers like(1, 2, 3)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ErrorMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();  // Send the request to the matching controller method

app.Run();