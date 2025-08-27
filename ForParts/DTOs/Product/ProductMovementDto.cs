using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Product
{
    public class ProductMovementDto
    {
        [Required(ErrorMessage = "El código del producto es obligatorio.")]
        public string CodeProduct { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
        public TypeProductMovement MovementType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser cero o mayor.")]
        public int QuantityProduced { get; set; } = 0;

        public DateTime? MovementDate { get; set; } // opcional, puede ser seteado automáticamente
    }
}
