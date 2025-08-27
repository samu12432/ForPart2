using System.ComponentModel.DataAnnotations;
using ForParts.Models.Enums;

namespace ForParts.Models.Supply
{
    public class StockMovement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CodeSupply { get; set; } = string.Empty;

        [Required]
        public int QuantityChange { get; set; } // Positivo o negativo

        [Required]
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public TypeMovement MovementType { get; set; } // Ej: "Alta", "Baja", "Edición"

        public string? UserName { get; set; }

        public StockMovement() { }

        public StockMovement(string codeSupply, int quantityChange, DateTime movementDate, TypeMovement movementType, string? userName)
        {
            CodeSupply = codeSupply;
            QuantityChange = quantityChange;
            MovementDate = movementDate;
            MovementType = movementType;
            UserName = userName;
        }
    }
}
