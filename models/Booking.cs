using car_rental_and_fleet_management.enums;

namespace car_rental_and_fleet_management.models;

public class Booking
{
    public Guid Id { get; set; }

    // user
    public Guid UserId {get; set;}
    public User? User {get; set;}

    // vehicle
    public Guid VehicleId {get; set;}
    public Vehicle? Vehicle {get; set;}

    // --- Scheduling Timeline ---
    public DateTime ScheduledStartDate { get; set; }
    public DateTime ScheduledEndDate { get; set; }
    
    // Actual times tracked when staff handles counter check-in/out
    public DateTime? ActualCheckOutTime { get; set; }
    public DateTime? ActualCheckInTime { get; set; } 

    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
