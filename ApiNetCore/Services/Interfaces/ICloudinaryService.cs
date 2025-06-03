namespace ApiNetCore.Services.Interfaces;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "products");
    Task<bool> DeleteImageAsync(string publicId);
    Task<string> UpdateImageAsync(IFormFile newFile, string oldPublicId, string folder = "products");
}