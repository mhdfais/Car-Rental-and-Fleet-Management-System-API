using car_rental_and_fleet_management.DTOs;

namespace car_rental_and_fleet_management.services;

public interface IAuthService
{
    Task RegisterUser(RegisterRequestDto dto);
    Task<AuthResponseDto> LoginUser(LoginRequestDto dto);
    Task RegisterStaff(RegisterRequestDto dto);
}