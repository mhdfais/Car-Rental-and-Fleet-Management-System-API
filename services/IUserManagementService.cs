using car_rental_and_fleet_management.DTOs;

namespace car_rental_and_fleet_management.services;

public interface IUserManagementService
{
    Task CreateStaff(CreateUserDto dto);
    Task CreateFleetManager(CreateUserDto dto);
    Task<bool> UpdateStaff(Guid id, UpdateUserDto dto);
    Task<bool> UpdateFleetManager(Guid id, UpdateUserDto dto);
    Task<List<UserResponseDto>> GetAllStaffs();
    Task<List<UserResponseDto>> GetAllFleetManagers();
    Task<List<UserResponseDto>> GetAllCustomers();
    Task<bool> UpdateUserStatus(Guid id);
    Task<UserResponseDto> GetUserById(Guid id);
}