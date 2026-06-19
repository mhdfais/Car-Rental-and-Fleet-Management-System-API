using car_rental_and_fleet_management.DTOs;

namespace car_rental_and_fleet_management.services;

public interface IVehicleBrandService
{
    Task AddVehicleBrand(VehicleBrandDto dto);
    Task UpdateVehicleBrand(Guid id, VehicleBrandDto dto);
    Task<List<VehicleBrandResponseDto>> GetAllVehicleBrands();
}