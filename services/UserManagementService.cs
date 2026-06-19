using System.Data.SqlTypes;
using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.enums;
using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;

namespace car_rental_and_fleet_management.services;

public class UserManagementService(AppDbContext context) : IUserManagementService
{
    public async Task CreateStaff(CreateUserDto dto)
    {
        if(await context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            throw new InvalidOperationException("user with this email already exists");
        }

        string passwordHash=BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var newUser=new User
        {
            Email=dto.Email,
            Name=dto.Name,
            Password=passwordHash,
            Role=UserRole.Staff
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        
    }

    public async Task CreateFleetManager(CreateUserDto dto)
    {
        if(await context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            throw new InvalidOperationException("user with this email already exists");
        }

        string passwordHash=BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var newUser=new User
        {
            Email=dto.Email,
            Name=dto.Name,
            Password=passwordHash,
            Role=UserRole.FleetManager
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        
    }

    public async Task<bool> UpdateStaff(Guid id, UpdateUserDto dto)
    {
        var user =await context.Users.FindAsync(id)
         ?? throw new KeyNotFoundException("provided user id is invalid or null");

         user.Name=dto.Name;

         await context.SaveChangesAsync();
         return true;
    }

     public async Task<bool> UpdateFleetManager(Guid id, UpdateUserDto dto)
    {
        var user =await context.Users.FindAsync(id)
         ?? throw new KeyNotFoundException("provided user id is invalid or null");

         user.Name=dto.Name;

         await context.SaveChangesAsync();
         return true;
    }

    public async Task<List<UserResponseDto>> GetAllStaffs()
    {
       var staffs=await GetUsersByRole(UserRole.Staff);
       return staffs;
    }

    public async Task<List<UserResponseDto>> GetAllCustomers()
    {
        var customers=await GetUsersByRole(UserRole.Customer);
        return customers;
    }

    public async Task<List<UserResponseDto>> GetAllFleetManagers()
    {
        var fleetManagers=await GetUsersByRole(UserRole.FleetManager);
        return fleetManagers;
    }

    public async Task<UserResponseDto> GetUserById(Guid id)
    {
        var user=await context.Users.FindAsync(id)
        ?? throw new ArgumentException("User not found");

        return new UserResponseDto{
            Id=user.Id, 
            Name=user.Name, 
            Email=user.Email,
            Role=user.Role,
            IsActive=user.IsActive,
        };
    }

    public async Task<bool> UpdateUserStatus(Guid id)
    {
        var user=await context.Users.FindAsync(id) 
        ?? throw new ArgumentException("user not found");

        user.IsActive=!user.IsActive;
        await context.SaveChangesAsync();

        return true; 
    }

    private async Task<List<UserResponseDto>> GetUsersByRole(UserRole role)
    {
        return await context.Users.Where(u=>u.Role==role).Select(u=>new UserResponseDto
        {
             Id=u.Id,
            Name=u.Name,
            Email=u.Email,
            Role=u.Role,
            IsActive=u.IsActive,
        }).ToListAsync();

    }

}