namespace car_rental_and_fleet_management.models;

public class VehicleBrand
{
    public Guid Id { get; set;}
    public string Make { get; set;}=string.Empty;
    public string Model { get; set;}=string.Empty;
    public string Type { get; set;}=string.Empty;
}