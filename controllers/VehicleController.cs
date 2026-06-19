using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_and_fleet_management.controllers;

[ApiController]
[Authorize]
[Route("api/admin")]
public class VehicleController(IVehicleManagementService vehicleService, IVehicleBrandService vehicleBrandService) : ControllerBase
{
    [HttpPost]
    [Route("vehicleBrand")]
    public async Task<IActionResult> CreateVehicleBrand([FromBody] VehicleBrandDto dto)
    {
        await vehicleBrandService.AddVehicleBrand(dto);
        return Ok(new {message="vehicle brand created"});
    }

    [HttpPut]
    [Route("vehicleBrand/{id}")]
    public async Task<IActionResult> UpdateVehicleBrand(Guid id, [FromBody ]VehicleBrandDto dto)
    {
        await vehicleBrandService.UpdateVehicleBrand(id,dto);
        return Ok(new {message="vehicle brand updated"});
    }

    [HttpGet]
    [Route("vehicleBrand")]
    public async Task<IActionResult> GetVehicleBrands()
    {
        var brands= await vehicleBrandService.GetAllVehicleBrands();
        return Ok(brands);
    }

    [HttpPost]
    [Route("vehicle")]
    public async Task<IActionResult> AddVehicle([FromBody] AddvehicleDto dto)
    {
        await vehicleService.AddVehicle(dto);
        return Ok(new { message="vehicle created"});
    }

    [HttpPut]
    [Route("vehicle/{id}")]
    public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] UpdateVehicleDto dto)
    {
        await vehicleService.UpdateVehicle(id,dto);
        return Ok(new {message="vehicle updated"});
    }

    [HttpGet]
    [Route("vehicle")]
    public async Task<IActionResult> GetVehicles()
    {
        var Vehicles=await vehicleService.GetAllVehicles();
        return Ok(Vehicles);
    }

    [HttpGet]
    [Route("vehicle/{id}")]
    public async Task<IActionResult> GetVehicleById(Guid id)
    {
        var vehicle=await vehicleService.GetVehicleById(id);
        return Ok(vehicle);
    }

}