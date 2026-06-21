namespace car_rental_and_fleet_management.common;

public class ApiResponse<T>
{
    public string Message { get; set; }=string.Empty;
    public bool Success { get; set; } = true;
    public T? Data { get; set; }

    // Helper method for quick generation of success payloads
    public static ApiResponse<T> SuccessPayload(T? data, bool success = true, string message="")
    {
        return new ApiResponse<T>
        {
            Success = success,
            Message = message,
            Data = data
        };
    }
}