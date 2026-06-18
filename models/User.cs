using car_rental_and_fleet_management.enums;

namespace car_rental_and_fleet_management.models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public string Email { get; set; }=string.Empty;
    public string Password { get; set; }=string.Empty;
    public UserRole Role {  get; set; }=UserRole.Customer;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }=DateTime.Now;

}