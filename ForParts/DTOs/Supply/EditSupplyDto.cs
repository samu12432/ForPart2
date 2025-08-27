using System.ComponentModel.DataAnnotations;
using ForParts.Models.Enums;

namespace ForParts.DTOs.Supply
{
    public class EditSupplyDto
    {
        [Required]
        public string codeSupply { get; set; } = string.Empty;
        [Required]
        public string description { get; set; } = string.Empty;
        [Required]
        public TypeSupply type { get; set; }    
    }
}
