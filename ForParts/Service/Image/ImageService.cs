using ForParts.IServices.Image;
using Microsoft.EntityFrameworkCore.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace ForParts.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveImageAsync(IFormFile image, string folder)
        {
                // Directorio donde se guardarán las imágenes
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, "products");
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                // Nombre único para la imagen
                var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(imagesFolder, fileName);

                // Guardar la imagen en disco
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Construir la URL accesible externamente

                var request = _httpContextAccessor.HttpContext?.Request;
                var imageUrl = $"{request?.Scheme}://{request?.Host}/{folder}/{fileName}";

                return imageUrl;
        }
    }
}

