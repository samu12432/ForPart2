using System.ComponentModel.DataAnnotations;
using ForParts.Models.Enums;

namespace ForParts.DTOs.Supply
{
    public class DeleteSupplyDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el codigo del insumo que deseas eliminar.")]
        public string codeSupply { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar el tipo de insumo que deseas eliminar.")]
        public TypeSupply type { get; set; }
    }
}
