using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;

namespace car_rental_and_fleet_management.services;

public class VehicleBrandService(AppDbContext context): IVehicleBrandService
{
    public async Task AddVehicleBrand(VehicleBrandDto dto)
    {
        if(await context.VehicleBrands.AnyAsync(vb => vb.Model == dto.Model))
        {
            throw new KeyNotFoundException("this model already exists");
        }

        var newVehicleBrand=new VehicleBrand
        {
            Make=dto.Make,
            Model=dto.Model,
            Type=dto.Type,
        };
        context.VehicleBrands.Add(newVehicleBrand);
        await context.SaveChangesAsync();
    }

    public async Task UpdateVehicleBrand(Guid id, VehicleBrandDto dto)
    {
        if(await context.VehicleBrands.AnyAsync(vb => vb.Model == dto.Model))
        {
            throw new KeyNotFoundException("this model already exists");
        }

        var vehicleBrand=await context.VehicleBrands.FindAsync(id)
        ?? throw new KeyNotFoundException("vehicle brand not found");

        vehicleBrand.Make=dto.Make;
        vehicleBrand.Model=dto.Model;
        vehicleBrand.Type=dto.Type;
       
        await context.SaveChangesAsync();
    }

    public async Task<List<VehicleBrandResponseDto>> GetAllVehicleBrands()
    {
        return await context.VehicleBrands.Select(v=> new VehicleBrandResponseDto
        {
            Id=v.Id,
            Make=v.Make,
            Type=v.Type,
            Model=v.Model,
        }).ToListAsync();
    }
}