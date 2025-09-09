using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Product
{
    public class ProductWebDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el nombre del producto.")]
        [MaxLength(100)]
        public string productName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar una descripcion del producto.")]
        [MaxLength(100)]
        public string productDescription { get; set; } = string.Empty;

        [EnumDataType(typeof(ProductType), ErrorMessage = "Tipo de producto no valido.")]
        public ProductType productCategory { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal productPrice { get; set; }

        public IFormFile? Image { get; set; }

        public string? imageUrl { get; set; } = string.Empty;
    }
}
