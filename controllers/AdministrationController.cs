using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_and_fleet_management.controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles ="Admin")]
public class AdministrationController(IUserManagementService userService) : ControllerBase
{
    [HttpPost("createStaff")]
    public async Task<IActionResult> CreateStaff([FromBody] CreateUserDto dto)
    {
        await userService.CreateStaff(dto);
        return Ok(new {message="staff created successfully"});
    }

    [HttpPost("createFleetManager")]
     public async Task<IActionResult> CreateFleetManager([FromBody] CreateUserDto dto)
    {
        await userService.CreateFleetManager(dto);
        return Ok(new {message="fleet manager created successfully"});
    }

    [HttpPut("updateStaff/{id}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateUserDto dto)
    {
        await userService.UpdateStaff(id, dto);
        return Ok(new {message="Staff updated"});
    }

    [HttpPut("updateFleetManager/{id}")]
    public async Task<IActionResult> UpdateFleetManager(Guid id, [FromBody] UpdateUserDto dto)
    {
        await userService.UpdateFleetManager(id, dto);
        return Ok(new {message="fleet manager updated"});
    }

    [HttpGet("getCustomers")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await userService.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("getStaffs")]
    public async Task<IActionResult> GetStaffs()
    {
        var staffs = await userService.GetAllStaffs();
        return Ok(staffs);
    }

    [HttpGet("getFleetManagers")]
    public async Task<IActionResult> GetFleetManagers()
    {
        var fleetManagers = await userService.GetAllFleetManagers();
        return Ok(fleetManagers);
    }

    [HttpPatch("updateUserStatus/{id}")]
    public async Task<IActionResult> UpdateUserStatus(Guid id)
    {
        await userService.UpdateUserStatus(id);
        return Ok(new {message="status updated"});
    }
}

