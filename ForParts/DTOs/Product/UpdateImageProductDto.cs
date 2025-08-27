using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Product
{
    public class UpdateImageProductDto
    {
        [Required]
        public string codeProduct {  get; set; } = string.Empty;
        [Required]
        public IFormFile? Image { get; set; }
        public string? imageUrl { get; set; } = string.Empty;
    }
}
