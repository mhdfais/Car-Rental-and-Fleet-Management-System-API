using System.ComponentModel.DataAnnotations;
using car_rental_and_fleet_management.enums;

namespace car_rental_and_fleet_management.DTOs;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set;}=string.Empty;
    public string Email { get; set; }=string.Empty;
    public UserRole Role {get; set; }
    public bool IsActive { get; set; }
}

public class CreateUserDto
{
    [Required]
    public string Name { get; set;}=string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set;}=string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set;}=string.Empty;

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set;}=string.Empty;
}

public class UpdateUserDto
{
    [Required(ErrorMessage = "name cannot be empty")]
    public string Name { get; set;}="";
}