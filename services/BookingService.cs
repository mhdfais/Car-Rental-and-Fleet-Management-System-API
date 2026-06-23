using car_rental_and_fleet_management.data;
using car_rental_and_fleet_management.DTOs;
using car_rental_and_fleet_management.enums;
using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;

namespace car_rental_and_fleet_management.services;

public class BookingService(AppDbContext context)
{
    public async Task<CustomerBookingReceiptDto> CreateReservation(Guid userId, PlaceBookingDto dto)
    {
        // Timeline Validation
        if (dto.StartDate.Date < DateTime.Now.Date)
            throw new ApplicationException("Reservations cannot be backdated into the past.");

        if (dto.EndDate <= dto.StartDate)
            throw new ApplicationException("The drop-off date must follow the pick-up date.");   

        var vehicle = await context.Vehicles.FindAsync(dto.VehicleId);
        if (vehicle == null) throw new KeyNotFoundException("The requested vehicle asset does not exist.");
        if (vehicle.Status == VehicleStatus.OutOfService) 
            throw new InvalidOperationException("This asset has been permanently retired from the rental lineup.");     

        // Collision Protection
        var isOverlapDetected = await context.Bookings.AnyAsync(b =>
            b.VehicleId == dto.VehicleId &&
            b.Status != BookingStatus.Cancelled &&
            dto.StartDate < b.ScheduledEndDate &&
            dto.EndDate > b.ScheduledStartDate);

        if (isOverlapDetected)
        {
            throw new InvalidOperationException("This vehicle is already booked by another customer across the requested date matrix timeline.");
        }        

        int totalDays = (dto.EndDate.Date - dto.StartDate.Date).Days;
        if (totalDays == 0) totalDays = 1; // 24-hour baseline price floor guarantee
        decimal billSum = totalDays * vehicle.DailyRate;

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VehicleId = dto.VehicleId,
            ScheduledStartDate = dto.StartDate,
            ScheduledEndDate = dto.EndDate,
            TotalPrice = billSum,
            Status = BookingStatus.Confirmed
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        return new CustomerBookingReceiptDto
        {       
            BookingId = booking.Id,
            CarDetails = $"{vehicle.VehicleBrand.Make} {vehicle.VehicleBrand.Model} ({vehicle.Year})", 
            LicensePlate = vehicle.NumberPlate, 
            StartDate = booking.ScheduledStartDate,
            EndDate = booking.ScheduledEndDate,
            ProjectedTotal = booking.TotalPrice,
            Status = booking.Status.ToString()
        };
    }
}