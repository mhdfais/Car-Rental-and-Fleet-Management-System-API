using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_and_fleet_management.controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles ="Admin")]
public class UserController(IUserManagementService userService) : ControllerBase
{
    [HttpPost("staff")]
    public async Task<IActionResult> CreateStaff([FromBody] CreateUserDto dto)
    {
        await userService.CreateStaff(dto);
        return Ok(new {message="staff created successfully"});
    }

    [HttpPost("fleetManager")]
     public async Task<IActionResult> CreateFleetManager([FromBody] CreateUserDto dto)
    {
        await userService.CreateFleetManager(dto);
        return Ok(new {message="fleet manager created successfully"});
    }

    [HttpPut("staff/{id}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateUserDto dto)
    {
        await userService.UpdateStaff(id, dto);
        return Ok(new {message="Staff updated"});
    }

    [HttpPut("fleetManager/{id}")]
    public async Task<IActionResult> UpdateFleetManager(Guid id, [FromBody] UpdateUserDto dto)
    {
        await userService.UpdateFleetManager(id, dto);
        return Ok(new {message="fleet manager updated"});
    }

    [HttpGet("customer")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await userService.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("staff")]
    public async Task<IActionResult> GetStaffs()
    {
        var staffs = await userService.GetAllStaffs();
        return Ok(staffs);
    }

    [HttpGet("fleetManager")]
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

