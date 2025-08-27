using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ForParts.Exceptions.Product;
using ForParts.Models.Enums;
using SupplyAlias = ForParts.Models.Supply.Supply;

namespace ForParts.Models.Product
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public string codeProduct { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty;
        public string productDescription { get; set; } = string.Empty;
        public ProductType productCategory { get; set; }
        public ICollection<SupplyNecessary> ProductoInsumos { get; set; } = new List<SupplyNecessary>();
        
        [Range(0.01, double.MaxValue)]
        public decimal productPrice { get; set; }
        public string imageUrl { get; set; } = string.Empty;

        //Trazabilidad de los productos
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;


        public void AddSupply(SupplyAlias supply, int quantity)
        {
            if (quantity <= 0) throw new ProductException("Cantidad inválida.");

            ProductoInsumos.Add(new SupplyNecessary(supply, quantity));
        }

        public void MarkUpdated(){
            this.IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void changeDescription(string nameProduct, string descriptionProduct)
        {
            this.productName = nameProduct;
            this.productDescription = descriptionProduct;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
