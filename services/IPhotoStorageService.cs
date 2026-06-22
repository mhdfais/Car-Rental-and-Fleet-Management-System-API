namespace car_rental_and_fleet_management.services;

public interface IPhotoStorageService
{
    Task<string?> UploadImage(IFormFile file);
}