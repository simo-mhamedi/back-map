using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using back_map.Entity;
using back_map.Context;

namespace back_map.Business.Cloud
{
    public class CloudService:ICloudService
    {
        private readonly Cloudinary _cloudinary;
        private readonly AppDbContext _dbContext;

        public CloudService(AppDbContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            // Set up Cloudinary account configuration
            Account cloudinaryAccount = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            // Create a Cloudinary instance
            _cloudinary = new Cloudinary(cloudinaryAccount);
        }

        public MediaFile UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            var publicId = RandomTokenString(10);
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId, // Optional: Specify a public ID for the uploaded image
                };

                // Perform the upload
                var uploadResult = _cloudinary.Upload(uploadParams);
                var media = new MediaFile();
                media.PulbicId = publicId;
                media.MediaUrl = uploadResult.SecureUrl.ToString();
                _dbContext.MediaFiles.Add(media);
                _dbContext.SaveChanges();
                // You can handle the upload result as needed
                return media;
            }
        }
        public static string RandomTokenString(int length, bool digitsOnly = false)
        {

            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            var token = "";
            if (digitsOnly)
            {
                var number = BitConverter.ToUInt16(bytes, 0);
                token = (number % 1000000).ToString("D6");
            }
            else
            {
                token = BitConverter.ToString(bytes).Replace("-", "");
            }
            return token;
        }
        public string GetImageUrl(string publicUrl)
        {
            // Parse the public URL to extract the public ID
            var publicId = GetPublicIdFromUrl(publicUrl);

            if (publicId == null)
            {
                // Handle case where the public ID cannot be extracted
                return null;
            }

            // Construct the URL for the image
            return _cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(300).Height(300).Crop("fill")).BuildUrl(publicId);
        }
        private string GetPublicIdFromUrl(string publicUrl)
        {
            // Extract the public ID from the URL
            var uri = new Uri(publicUrl);
            var segments = uri.Segments;

            if (segments.Length > 0)
            {
                // Assuming the public ID is the last segment without the file extension
                var publicId = segments[segments.Length - 1].Split('.')[0];
                return publicId;
            }

            return null;
        }
        public void DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
                // You can add more options as needed
            };

            var result = _cloudinary.Destroy(deletionParams);

            // Check result for success or handle accordingly
            if (result.Result == "ok")
            {
                // Successfully deleted
            }
            else
            {
                // Handle deletion failure
                // You might want to log or throw an exception based on your application's needs
            }
        }
    }
}
