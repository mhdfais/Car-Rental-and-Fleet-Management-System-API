using System.ComponentModel.DataAnnotations;

namespace car_rental_and_fleet_management.DTOs;

public class VehicleBrandDto
{
    [Required]
    public string Make { get; set;}=string.Empty;

    [Required]
    public string Model { get; set;}=string.Empty;

    [Required]
    public string Type { get; set;}=string.Empty;
}    

public class VehicleBrandResponseDto
{
    public Guid Id;
    public string Make { get; set;}=string.Empty;
    public string Model { get; set;}=string.Empty;
    public string Type { get; set;}=string.Empty;
}    

