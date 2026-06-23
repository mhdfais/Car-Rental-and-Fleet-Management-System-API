using car_rental_and_fleet_management.DTOs;

namespace car_rental_and_fleet_management.services;

public interface IVehicleManagementService
{
    Task AddVehicle(AddvehicleDto dto, IPhotoStorageService storageService);
    Task UpdateVehicle(Guid id, UpdateVehicleDto dto);
    Task<List<VehicleResponseDto>> GetAllVehicles();
    Task<VehicleResponseDto> GetVehicleById(Guid id);
    Task<IEnumerable<VehicleResponseDto>> GetAvailableVehicles(VehicleSearchQueryDto dto);
}