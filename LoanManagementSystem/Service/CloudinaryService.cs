using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace LoanManagementSystem.Service
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var account = new Account(
                System.Configuration.ConfigurationManager.AppSettings["CloudinaryCloudName"],
                System.Configuration.ConfigurationManager.AppSettings["CloudinaryApiKey"],
                System.Configuration.ConfigurationManager.AppSettings["CloudinaryApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public ImageUploadResult UploadImage(HttpPostedFileBase file)
        {
            var uploadResult = new ImageUploadResult();

            if (file != null && file.ContentLength > 0)
            {
                using (var stream = file.InputStream)
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Crop("fill").Width(500).Height(500)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            return uploadResult;
        }
    }
}