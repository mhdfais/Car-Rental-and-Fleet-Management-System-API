namespace car_rental_and_fleet_management.enums;

public enum VehicleStatus
{
    Available,         // Ready for a customer to drive away
    Rented,            // Currently out with a customer on an active trip
    Reserved,          // Booked for a future slot (blocked from being rented right now)
    InMaintenance,     // In the shop for routine work (oil change, tires)
    Damaged,           // Involved in an accident; awaiting insurance or repair inspection
    Cleaning,          // Returned by a customer, being vacuumed/washed before next rental
    OutOfService       // Permanently retired from the active fleet lineup
}