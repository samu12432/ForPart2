using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Supply
{
    public class EditImageSupplyDto
    {
        [Required]
        public string codeSupply { get; set; } = string.Empty;
        [Required]
        public IFormFile? Image { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        [Required]
        public TypeSupply type { get; set; }
    }
}


