
namespace Shared.Helper;
public static class MimeTypeHelper
{
    public static string GetContentType(string filePathExtension)
    {
        string extension = Path.GetExtension(filePathExtension).ToLower();
        return extension switch
        {
            ".svg"=>"image/svg+xml",
            ".png"=>"image/png",
            ".jpg"or".jpeg"=>"image/jpeg",
            ".webp"=>"image/webp",
            _=>"application/octet-stream"
        };
    }
}