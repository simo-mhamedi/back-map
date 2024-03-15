using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using back_map.Entity;

namespace back_map.Business.Cloud
{
    public interface ICloudService
    {
        MediaFile UploadImage(IFormFile file);
        void DeleteImage(string publicId);

    }
}
