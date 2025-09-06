using System.ComponentModel.DataAnnotations;
using ForParts.Models.Enums;

namespace ForParts.DTOs.Supply
{
    public class StockMovementDto
    {
        [Required(ErrorMessage = "El código del insumo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El código del insumo no puede superar los 50 caracteres.")]
        public string CodeSupply { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, 1000000, ErrorMessage = "La cantidad debe estar dentro de un rango válido.")]
        public int QuantityChange { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
        public TypeMovement MovementType { get; set; }
        public DateTime? MovementDate { get; set; }

    }
}
