using car_rental_and_fleet_management.common;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_and_fleet_management.controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        await service.RegisterUser(dto);

        return Created("",new ApiResponse<object>{
                Message="user created successfully", 
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var result=await service.LoginUser(dto);
        return Created("", new ApiResponse<object>
        {
                Message="login successfull",
                Data=result
        });
    }
    
    
}