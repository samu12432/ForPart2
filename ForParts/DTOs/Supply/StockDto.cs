using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Supply
{
    public class StockDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el sku del articulo.")]
        [StringLength(50)]
        public string codeSupply { get; set; } = string.Empty;

        public int SupplyId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0.")]
        public int stockQuantity { get; set; }
    }
}
