using ForParts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForParts.Models.Supply
{
    public class BudgetedSupply
    {
        [Key]
        public int Id { get; set; }
        public int BudgetedProductId { get; set; }

        public string SupplyCode { get; set; }
        public TypeSupply TypeSupply { get; set; }
        public string UnitMeasure { get; set; }
        public int amount { get; set; }

        public decimal UnityPrice { get; set; }
        public decimal Subtotal { get; set; }

        public bool OutOfStock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
