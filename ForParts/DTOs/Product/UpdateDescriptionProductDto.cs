using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Product
{
    public class UpdateDescriptionProductDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el codigo del producto.")]
        public string codeProduct { get; set; } = string.Empty;
        public string nameProduct { get; set; } = string.Empty;
        public string descriptionProduct { get; set; } = string.Empty;
    }
}
