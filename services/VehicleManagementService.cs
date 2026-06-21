using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;

namespace car_rental_and_fleet_management.services;

public class VehicleManagementService(AppDbContext context):IVehicleManagementService
{
    public async Task AddVehicle(AddvehicleDto dto)
    {
        if(await context.Vehicles.AnyAsync(v=>v.NumberPlate==dto.NumberPlate))
        {
            throw new KeyNotFoundException("vehicle exist with this number plate");
        }

        var newVehicle = new Vehicle
        {
          NumberPlate=dto.NumberPlate,
          Year=dto.Year,
          Odometer=dto.Odometer,
          DailyRate=dto.DailyRate,
          VehicleBrandId=dto.VehicleBrandId  
        };

        context.Vehicles.Add(newVehicle);
        await context.SaveChangesAsync();
    }

    public async Task UpdateVehicle(Guid id, UpdateVehicleDto dto)
    {
        var vehicle=await context.Vehicles.FindAsync(id) 
        ?? throw new KeyNotFoundException("vehicle not found");

        vehicle.NumberPlate=dto.NumberPlate;
        vehicle.Year=dto.Year;
        vehicle.Odometer=dto.Odometer;
        vehicle.DailyRate=dto.DailyRate;
        vehicle.VehicleBrandId=dto.VehicleBrandId;

        await context.SaveChangesAsync();
    }

    public async Task<List<VehicleResponseDto>> GetAllVehicles()
    {
       return await context.Vehicles.Select(v=>new VehicleResponseDto
       {
           Id=v.Id,
           NumberPlate=v.NumberPlate,
           Year=v.Year,
           Odometer=v.Odometer,
           DailyRate=v.DailyRate,
           Status=v.Status,
           CreatedAt=v.CreatedAt,
           Make=v.VehicleBrand.Make,
           Model=v.VehicleBrand.Model,
           Type=v.VehicleBrand.Type,
       }).ToListAsync();
    }

    public async Task<VehicleResponseDto> GetVehicleById(Guid id)
    {
        var vehicle = await context.Vehicles
        .Include(v => v.VehicleBrand) 
        .FirstOrDefaultAsync(v => v.Id == id) 
        ?? throw new KeyNotFoundException("vehicle not found");

        return new VehicleResponseDto
        {
            Id=vehicle.Id,
            NumberPlate=vehicle.NumberPlate,
            Year=vehicle.Year,
            Odometer=vehicle.Odometer,
            DailyRate=vehicle.DailyRate,
            Status=vehicle.Status,
            CreatedAt=vehicle.CreatedAt,
            Make=vehicle.VehicleBrand.Make,
            Model=vehicle.VehicleBrand.Model,
            Type=vehicle.VehicleBrand.Type,
        };
    }
}