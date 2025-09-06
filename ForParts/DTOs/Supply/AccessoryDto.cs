using ForParts.IDto;
using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Supply
{
    public class AccessoryDto : IImageDto
    {
        [Required(ErrorMessage = "Es necesario ingresar el codigo del insumo.")]
        [StringLength(50)]
        public string codeSupply { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar el nombre del insumo.")]
        [StringLength(50)]
        public string nameSupply { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar una descripción del insumo.")]
        [StringLength(100)]
        public string descriptionSupply { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar la imagen.")]
        public IFormFile? Image { get; set; }

        public string? imageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar el nombre del proveedor.")]
        [StringLength(20)]
        public string nameSupplier { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal priceSupply { get; set; }
        //___________________________________________________//

        [Required(ErrorMessage = "Es necesario ingresar una descripción del accesorio.")]
        [StringLength(100)]
        public string descriptionAccessory { get; set; } = string.Empty;

        [EnumDataType(typeof(TypeAccessory), ErrorMessage = "El tipo de accesorio no es válido")]
        public TypeAccessory typeAccessory { get; set; }

        [EnumDataType(typeof(SerieProfile), ErrorMessage = "La serie no es válida")]
        public SerieProfile serie { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar un color del accesorio.")]
        [StringLength(100)]
        public string color { get; set; } = string.Empty;
    }
}
