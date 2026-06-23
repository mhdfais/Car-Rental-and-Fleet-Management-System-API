using car_rental_and_fleet_management.common;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_and_fleet_management.controllers;

[ApiController]
[Authorize]
[Route("api/vehicles")]
public class VehicleController(IVehicleManagementService vehicleService, IVehicleBrandService vehicleBrandService) : ControllerBase
{
    [HttpPost]
    [Route("vehicleBrands")]
    public async Task<IActionResult> CreateVehicleBrand([FromBody] VehicleBrandDto dto)
    {
        await vehicleBrandService.AddVehicleBrand(dto);
        return Created("",new ApiResponse<object>
        {
            Message="vehicle brand created successfully",
        });
    }

    [HttpPut]
    [Route("vehicleBrands/{id}")]
    public async Task<IActionResult> UpdateVehicleBrand(Guid id, [FromBody ]VehicleBrandDto dto)
    {
        await vehicleBrandService.UpdateVehicleBrand(id,dto);
        return Ok(new ApiResponse<object>
        {
            Message="updated vehicle brand successfully",
        });
    }

    [HttpGet]
    [Route("vehicleBrands")]
    public async Task<IActionResult> GetVehicleBrands()
    {
        var brands= await vehicleBrandService.GetAllVehicleBrands();
        return Ok(new ApiResponse<object>
        {
            Message="vehicle brands retrieved successfully",
            Data=brands
        });
    }

    [HttpPost]
    [Route("vehicles")]
    public async Task<IActionResult> AddVehicle([FromForm] AddvehicleDto dto, IPhotoStorageService storageService)
    {
        await vehicleService.AddVehicle(dto, storageService);
        return Created("",new ApiResponse<object>
        {
            Message="vehicle created successfully",
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] UpdateVehicleDto dto)
    {
        await vehicleService.UpdateVehicle(id,dto);
         return Ok(new ApiResponse<object>
        {
            Message="vehicle updated successfully",
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetVehicles()
    {
        var Vehicles=await vehicleService.GetAllVehicles();
        return Ok(new ApiResponse<object>
        {
            Message="vehicles retrieved successfully",
            Data=Vehicles
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetVehicleById(Guid id)
    {
        var vehicle=await vehicleService.GetVehicleById(id);
        return Ok(new ApiResponse<object>
        {
            Message="vehicle retrieved successfully",
            Data=vehicle
        });
    }

    [HttpGet]
    [Route("available")]
    public async Task<IActionResult> GetAvailableVehicles([FromQuery] VehicleSearchQueryDto dto)
    {
        var vehicles=await vehicleService.GetAvailableVehicles(dto);
        return Ok(new ApiResponse<object>
        {
            Message="Vehicles retrieved successfully",
            Data=vehicles
        });
    }

}