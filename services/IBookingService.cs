using car_rental_and_fleet_management.DTOs;

namespace car_rental_and_fleet_management.services;

public interface IBookingService
{
    Task<CustomerBookingReceiptDto> CreateReservation(Guid userId, PlaceBookingDto dto);
}