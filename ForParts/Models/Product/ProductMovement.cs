using System.ComponentModel.DataAnnotations;
using ForParts.Models.Enums;

namespace ForParts.Models.Product
{
    public class ProductMovement
    {
        [Key]
        public int MovementId { get; set; }

        [Required]
        public string CodeProduct { get; set; } = string.Empty;

        [Required]
        public TypeProductMovement MovementType { get; set; }

        public int Quantity { get; set; } = 0; // útil para facturacion

        public string UserName { get; set; } = "Sistema";

        public DateTime MovementDate { get; set; } = DateTime.UtcNow;

        public ProductMovement() { }

        public ProductMovement(string codeProduct, TypeProductMovement movementType,
            int quantity, string userName, DateTime movementDate)
        {
            CodeProduct = codeProduct;
            MovementType = movementType;
            Quantity = quantity;
            UserName = userName;
        }
    }
}
