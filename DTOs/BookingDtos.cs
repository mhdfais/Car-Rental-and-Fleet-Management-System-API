using System.ComponentModel.DataAnnotations;

namespace car_rental_and_fleet_management.DTOs;

public class PlaceBookingDto
{
    [Required]
    public Guid VehicleId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate{ get; set; }
}

public class CustomerBookingReceiptDto{
    public Guid BookingId { get; set; }
    public string CarDetails { get; set; }=string.Empty;
    public string LicensePlate { get; set; }=string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal ProjectedTotal { get; set; }
    public string Status{ get; set; }=string.Empty;
};