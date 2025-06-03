namespace ApiNetCore.Utils;

public static class CloudinaryHelper
{
    public static string? ExtractPublicIdFromUrl(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return null;

        try
        {
            var uri = new Uri(imageUrl);
            var path = uri.AbsolutePath;

            
            var segments = path.Split('/');
            var uploadIndex = Array.IndexOf(segments, "upload");
            
            if (uploadIndex >= 0 && uploadIndex + 2 < segments.Length)
            {
                // Saltar "upload" y version number
                var relevantSegments = segments.Skip(uploadIndex + 2);
                var publicIdWithExtension = string.Join("/", relevantSegments);
                
                // Remover extensión
                var lastDotIndex = publicIdWithExtension.LastIndexOf('.');
                return lastDotIndex > 0 ? publicIdWithExtension.Substring(0, lastDotIndex) : publicIdWithExtension;
            }
        }
        catch
        {
            // Si no se puede parsear, retornar null
        }

        return null;
    }
}