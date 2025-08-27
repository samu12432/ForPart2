using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ForParts.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Models.Supply
{
    [Table("Supply")]
    [Index(nameof(codeSupply), IsUnique = true)]
    public class Supply
    {
        [Key]
        public int idSupply { get; set; }
        [Required]
        public string codeSupply { get; set; } = string.Empty;
        [Required]
        public string nameSupply { get; set; } = string.Empty;
        public string descriptionSupply { get; set; } = string.Empty;

        public string imageUrl { get; set; } = string.Empty;
        [Required]
        public string nameSupplier { get; set; } = string.Empty;
        [Required]
        public decimal priceSupply { get; set; }

        public bool isEnabledSupply { get; set; } = true;

        public Stock Stock { get; set; } = null!;

        public ICollection<SupplyNecessary> ProductoInsumos { get; set; } = new List<SupplyNecessary>();
        public Supply() { }

        public Supply(int idSupply, string codeSupply, string nameSupply, string descriptionSupply, string imageUrl, string nameSupplier, decimal priceSupply)
        {
            this.idSupply = idSupply;
            this.codeSupply = codeSupply;
            this.nameSupply = nameSupply;
            this.descriptionSupply = descriptionSupply;
            this.imageUrl = imageUrl;
            this.nameSupplier = nameSupplier;
            this.priceSupply = priceSupply;
        }
    }
}
