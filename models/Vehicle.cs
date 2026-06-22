using car_rental_and_fleet_management.enums;

namespace car_rental_and_fleet_management.models;

public class Vehicle
{
    public Guid Id { get; set;}
    public string NumberPlate { get; set;}=string.Empty;
    public int Year { get; set;}=0;
    public int Odometer { get; set;}=0;
    public decimal DailyRate { get; set;}=0;
    public Guid VehicleBrandId { get; set;}
    public VehicleBrand VehicleBrand { get; set;}=null!;
    public VehicleStatus Status { get; set;}=VehicleStatus.Available;
    public List<VehicleImage> Images { get; set; } = [];
    public bool IsActive { get; set;}=true;
    public DateTime CreatedAt { get; set;} = DateTime.Now;
}