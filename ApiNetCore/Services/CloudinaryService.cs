using ApiNetCore.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ApiNetCore.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var cloudinaryUrl = configuration["CLOUDINARY_URL"];
        if (string.IsNullOrEmpty(cloudinaryUrl))
        {
            throw new ArgumentException("CLOUDINARY_URL environment variable is not set");
        }

        _cloudinary = new Cloudinary(cloudinaryUrl);
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder = "products")
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is required");

        // Validar que sea una imagen
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException("Only image files are allowed");

        // Validar tamaño (máximo 5MB)
        if (file.Length > 5 * 1024 * 1024)
            throw new ArgumentException("File size cannot exceed 5MB");

        try
        {
            await using var stream = file.OpenReadStream();
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
                    .Width(800)
                    .Height(600)
                    .Crop("fill"),
                PublicId = $"{folder}_{Guid.NewGuid()}"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error uploading image: {ex.Message}");
        }
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            return false;

        try
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> UpdateImageAsync(IFormFile newFile, string oldPublicId, string folder = "products")
    {
        // Subir nueva imagen
        var newImageUrl = await UploadImageAsync(newFile, folder);

        // Eliminar imagen anterior si existe
        if (!string.IsNullOrEmpty(oldPublicId))
        {
            await DeleteImageAsync(oldPublicId);
        }

        return newImageUrl;
    }
}