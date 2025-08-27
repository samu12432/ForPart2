using System.ComponentModel.DataAnnotations;
using ForParts.Models.Supply;
using System.ComponentModel.DataAnnotations.Schema;
using SupplyAlias = ForParts.Models.Supply.Supply;

namespace ForParts.Models.Product
{
    public class SupplyNecessary
    {
        [Key]
        public int idSupplyNecessary { get; set; }

        [Required]
        public int supplyId { get; set; }

        [ForeignKey(nameof(supplyId))]
        public SupplyAlias? supply { get; set; } = null;

        [Required]
        public int productId { get; set; }

        [ForeignKey(nameof(productId))]
        public Product? Product { get; set; } = null;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } 

        public SupplyNecessary() { }

        public SupplyNecessary(SupplyAlias supply, int quantity)
        {
            this.supply = supply;
            this.supplyId = supply.idSupply;
            Quantity = quantity;
        }


    }
}
