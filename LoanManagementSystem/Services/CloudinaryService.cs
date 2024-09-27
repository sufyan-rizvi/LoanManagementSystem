using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Web;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        // Initialize Cloudinary account settings
        Account account = new Account(
            "your_cloud_name",
            "your_api_key",
            "your_api_secret"
        );

        _cloudinary = new Cloudinary(account);
    }

    public RawUploadResult UploadDocument(HttpPostedFileBase file)
    {
        if (file == null || file.ContentLength == 0)
        {
            return null;
        }

        var stream = file.InputStream;

        // Set up the raw upload parameters for document upload
        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            ResourceType = "raw" // Correct usage for document upload
        };

        // Perform the upload
        return _cloudinary.Upload(uploadParams);
    }
}
