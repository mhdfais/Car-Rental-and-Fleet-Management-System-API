using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace car_rental_and_fleet_management.services;

public class CloudinaryStorageService : IPhotoStorageService
{
    private readonly Cloudinary cloudinary;
    public CloudinaryStorageService(IConfiguration config)
    {
        var cloudName = config.GetSection("CloudinarySettings:CloudName").Value;
        var apiKey = config.GetSection("CloudinarySettings:ApiKey").Value;
        var apiSecret = config.GetSection("CloudinarySettings:ApiSecret").Value;

        var account = new Account(cloudName, apiKey, apiSecret);
        cloudinary = new Cloudinary(account);
    }

    public async Task<string?> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0) return null;

        // Open a stream to read the uploaded binary file safely without holding it all in server memory
        await using var stream = file.OpenReadStream();
        
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            // Crop and resize images automatically on Cloudinary's servers to prevent unoptimized uploads
            Transformation = new Transformation().Width(800).Height(600).Crop("fill"),
            Folder = "vehicles" // Cloudinary folder name
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return uploadResult.SecureUrl.ToString(); // Returns the safe https:// resourcelink
        }

        return null;
    }
}