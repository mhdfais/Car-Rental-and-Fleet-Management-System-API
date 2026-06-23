namespace car_rental_and_fleet_management.data;

using car_rental_and_fleet_management.models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options){}

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleBrand> VehicleBrands { get; set; }
    public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().Property(u=>u.Role).HasConversion<string>();

        modelBuilder.Entity<VehicleImage>()
            .HasOne(img => img.Vehicle)             // An image has One Vehicle
            .WithMany(v => v.Images)                // A Vehicle has Many Images
            .HasForeignKey(img => img.VehicleId)    // The Foreign key column name
            .OnDelete(DeleteBehavior.Cascade);      // If Vehicle is deleted, delete all its images
    }
}