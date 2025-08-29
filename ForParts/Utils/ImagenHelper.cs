using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace ForParts.Utils
{
    public static class ImagenHelper
    {
        public static string ObtenerUrl(string codigo, HttpRequest request, IWebHostEnvironment env)
        {
            var extensiones = new[] { ".jpg", ".png", ".jpeg", ".webp" };

            string? ruta = extensiones
                .Select(ext => Path.Combine(env.WebRootPath, "imagenes", $"{codigo}{ext}"))
                .FirstOrDefault(File.Exists);

            string archivo = ruta != null
                ? Path.GetFileName(ruta)
                : "default.jpg"; // Asegurate de tener esta imagen en wwwroot/imagenes

            return $"{request.Scheme}://{request.Host}/imagenes/{archivo}";
        }
    }
}
