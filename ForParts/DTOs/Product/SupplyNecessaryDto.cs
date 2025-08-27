using System.ComponentModel.DataAnnotations;

namespace ForParts.DTO.Product
{
    public class SupplyNecessaryDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el codigo del insumo.")]
        public string codeSupply { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0.")]
        public int quantity { get; set; }
    }
}
