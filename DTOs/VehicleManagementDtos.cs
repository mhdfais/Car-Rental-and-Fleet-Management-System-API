using System.ComponentModel.DataAnnotations;
using car_rental_and_fleet_management.enums;

namespace car_rental_and_fleet_management.DTOs;

public class AddvehicleDto
{
    [Required]
    public string NumberPlate { get; set;}=string.Empty;

    [Required]
    [Range(2000,2050)]
    public int Year { get; set;}

    [Required]
    [Range(0,100000)]
    public int Odometer { get; set;}

    [Required]
    [Range(100,50000)]
    public decimal DailyRate { get; set;}

    [Required]
    public Guid VehicleBrandId { get; set;}
}

public class UpdateVehicleDto
{
    [Required]
    public string NumberPlate { get; set;}=string.Empty;

    [Required]
    [Range(2000,2050)]
    public int Year { get; set;}

    [Required]
    [Range(0,100000)]
    public int Odometer { get; set;}

    [Required]
    [Range(100,50000)]
    public decimal DailyRate { get; set;}

    [Required]
    public Guid VehicleBrandId { get; set;}
}

public class VehicleResponseDto
{
    public Guid Id { get; set;}
    public string NumberPlate {get; set;}=string.Empty;
    public int Year {get; set;}
    public int Odometer{get; set;}
    public decimal DailyRate{get; set;}
    public VehicleStatus Status{ get; set;}
    public DateTime CreatedAt { get; set;}

    public string Make { get; set;}=string.Empty;
    public string Model { get; set;}=string.Empty;
    public string Type { get; set;}=string.Empty;
}