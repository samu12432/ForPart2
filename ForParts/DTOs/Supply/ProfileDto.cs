using System.ComponentModel.DataAnnotations;
using ForParts.IDto;
using ForParts.Models.Enums;

namespace ForParts.DTOs.Supply
{
    public class ProfileDto : IImageDto
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

        public IFormFile? Image { get; set; }

        public string? imageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Es necesario ingresar el nombre del proveedor.")]
        [StringLength(20)]
        public string nameSupplier { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal priceSupply { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El peso debe ser mayor que cero.")]
        public decimal profileWeigth { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El largo debe ser mayor que cero.")]
        public decimal profileHeigth { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El peso por metro debe ser mayor que cero.")]
        public decimal weigthMetro { get; set; }

        [Required(ErrorMessage = "El color es obligatorio.")]
        [StringLength(20)]
        public string profileColor { get; set; } = string.Empty;

        [EnumDataType(typeof(SerieProfile), ErrorMessage = "La serie no es válida.")]
        public SerieProfile serieProfile { get; set; } 
    }
}
