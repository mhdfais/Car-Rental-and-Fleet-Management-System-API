using car_rental_and_fleet_management.common;
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
        return Created("",new ApiResponse<object>
        {
            Message="staff created successfully"
        });
    }

    [HttpPost("fleetManager")]
     public async Task<IActionResult> CreateFleetManager([FromBody] CreateUserDto dto)
    {
        await userService.CreateFleetManager(dto);
       return Created("",new ApiResponse<object>
        {
            Message="fleetmanager created successfully"
        });
    }

    [HttpPut("staff/{id}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateUserDto dto)
    {
        await userService.UpdateStaff(id, dto);
        return Ok(new ApiResponse<object>
        {
            Message="staff updated successfully"
        });
    }

    [HttpPut("fleetManager/{id}")]
    public async Task<IActionResult> UpdateFleetManager(Guid id, [FromBody] UpdateUserDto dto)
    {
        await userService.UpdateFleetManager(id, dto);
        return Ok(new ApiResponse<object>
        {
            Message="fleet manager updated successfully"
        });
    }

    [HttpGet("customer")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await userService.GetAllCustomers();
        return Ok(new ApiResponse<object>
        {
            Message="customers retrieved successfully",
            Data=customers
        });
    }

    [HttpGet("staff")]
    public async Task<IActionResult> GetStaffs()
    {
        var staffs = await userService.GetAllStaffs();
        return Ok(new ApiResponse<object>
        {
            Message="staffs retrieved successfully",
            Data=staffs
        });
    }

    [HttpGet("fleetManager")]
    public async Task<IActionResult> GetFleetManagers()
    {
        var fleetManagers = await userService.GetAllFleetManagers();
        return Ok(new ApiResponse<object>
        {
            Message="fleet managers retrieved successfully",
            Data=fleetManagers
        });
    }

    [HttpPatch("updateUserStatus/{id}")]
    public async Task<IActionResult> UpdateUserStatus(Guid id)
    {
        await userService.UpdateUserStatus(id);
        return Ok(new ApiResponse<object>
        {
            Message="status updated successfully",
        });
    }
}

