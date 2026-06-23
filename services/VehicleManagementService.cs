using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.enums;
using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;

namespace car_rental_and_fleet_management.services;

public class VehicleManagementService(AppDbContext context):IVehicleManagementService
{
    public async Task AddVehicle(AddvehicleDto dto, IPhotoStorageService storageService)
    {
        if(await context.Vehicles.AnyAsync(v=>v.NumberPlate==dto.NumberPlate))
        {
            throw new KeyNotFoundException("vehicle exist with this number plate");
        }

        if(!await context.VehicleBrands.AnyAsync(vb => vb.Id == dto.VehicleBrandId))
        {
            throw new KeyNotFoundException("vehicle with this brand is not found in vehicles brands");
        }

        var newVehicle = new Vehicle
        {
          Id=Guid.NewGuid(),  
          NumberPlate=dto.NumberPlate,
          Year=dto.Year,
          Odometer=dto.Odometer,
          DailyRate=dto.DailyRate,
          VehicleBrandId=dto.VehicleBrandId , 
          Images = new List<VehicleImage>() // Instantiated cleanly to avoid any collection NULL tracking bugs
        };

        if (dto.Images != null && dto.Images.Count > 0)
        {
        bool isFirstImage = true;

            foreach (var file in dto.Images)
            {
              // Upload binary streams directly to Cloudinary
                var secureUrl = await storageService.UploadImage(file);
            
                if (!string.IsNullOrEmpty(secureUrl))
                {
                   newVehicle.Images.Add(new VehicleImage
                   {
                    Id = Guid.NewGuid(),
                    Url = secureUrl,
                    IsPrimary = isFirstImage, // Mark the first uploaded image as primary thumbnail
                    });
                
                isFirstImage = false; // Subsquent files are marked secondary gallery images
                }
            }
        }

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
           ImageUrls=v.Images.Select(img=>img.Url).ToList(),
       }).ToListAsync();
    }

    public async Task<VehicleResponseDto> GetVehicleById(Guid id)
    {
        var vehicle = await context.Vehicles
        .Include(v => v.VehicleBrand) 
        .Include(v=>v.Images)
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
            ImageUrls=vehicle.Images.Select(img=>img.Url).ToList()
        };
    }

    public async Task<IEnumerable<VehicleResponseDto>> GetAvailableVehicles(VehicleSearchQueryDto dto)
    {
        var queryable=context.Vehicles
        .Include(v=>v.Images)
        .Include(v=>v.VehicleBrand)
        .Where(v=>v.Status==VehicleStatus.Available)
        .AsQueryable();

        // Text Matching
        if (!string.IsNullOrWhiteSpace(dto.SearchTerm))
        {
            var cleanTerm=dto.SearchTerm.Trim().ToLower();
            queryable=queryable.Where(v=>v.VehicleBrand.Make.ToLower().Contains(cleanTerm) || v.VehicleBrand.Model.ToLower().Contains(cleanTerm));
        }

        // Budget Cap
        if(dto.MaxDailyRate.HasValue && dto.MaxDailyRate.Value > 0)
        {
            queryable=queryable.Where(v=>v.DailyRate <= dto.MaxDailyRate.Value);
        }

        // Availability Checks
        // if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.EndDate > dto.StartDate)
        // {
        //     var start = dto.StartDate.Value;
        //     var end = dto.EndDate.Value;

        //     var bookedVehicleIds = context.Bookings
        //         .Where(b => b.Status != BookingStatus.Cancelled &&
        //             start < b.ScheduledEndDate &&
        //             end > b.ScheduledStartDate)
        //         .Select(b => b.VehicleId);

        //     queryable = queryable.Where(v => !bookedVehicleIds.Contains(v.Id));
        // }

        var vehicles = await queryable.AsNoTracking().OrderBy(v => v.DailyRate).ToListAsync();
        
        return vehicles.Select(v=>new VehicleResponseDto
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
            ImageUrls=v.Images.Select(img=>img.Url).ToList(),
        });
    }
}