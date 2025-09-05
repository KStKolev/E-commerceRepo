using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_commerceApplication.Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace E_commerceApplication.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream())
            };

            var result = await _cloudinary
                .UploadAsync(uploadParams);

            return result
                .SecureUrl
                .ToString();
        }
    }
}