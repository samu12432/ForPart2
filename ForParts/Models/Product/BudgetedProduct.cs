using ForParts.Models.Enums;
using ForParts.Models.Supply;

namespace ForParts.Models.Product
{
    public class BudgetedProduct
    {
        public int Id { get; set; }
        public int PresupuestoId { get; set; }

        public string Name { get; set; }
        public decimal Width { get; set; }
        public decimal Heigth { get; set; }
        public string Color { get; set; }
        public int Amount { get; set; }

        public ProductType ProductType { get; set; }
        public SerieProfile SerieProfile { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public List<BudgetedSupply> SuppliesUsed { get; set; }
    }

}
