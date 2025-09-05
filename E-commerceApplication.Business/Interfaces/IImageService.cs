using Microsoft.AspNetCore.Http;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IImageService
    {
       Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
