namespace car_rental_and_fleet_management.models;

public class VehicleImage
{
    public Guid Id { get; set; }
    
    public string Url { get; set; } = string.Empty;
    
    public bool IsPrimary { get; set; } = false; // True if this is the main thumbnail displayed in the catalog
    
    public DateTime UploadedAt { get; set; } = DateTime.Now;

    // --- Relational Foreign Key Configuration ---
    // This tells MS SQL exactly which vehicle this photo belongs to
    public Guid VehicleId { get; set; }
    
    // Navigation property (allows you to navigate from an image back to its parent vehicle object in C#)
    public Vehicle? Vehicle { get; set; }
}