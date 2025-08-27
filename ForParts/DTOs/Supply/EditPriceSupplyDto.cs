using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Supply
{
    public class EditPriceSupplyDto
    {
        [Required]
        public string codeSupply { get; set; } = string.Empty;
        [Required]
        public decimal newPriceSupply { get; set; }
        [Required]
        public TypeSupply type { get; set; }
    }
}
