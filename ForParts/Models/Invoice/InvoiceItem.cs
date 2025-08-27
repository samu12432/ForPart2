using System.ComponentModel.DataAnnotations;
using ProductAlias = ForParts.Models.Product.Product;

namespace ForParts.Models.Invoice
{
    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
        public string ProductCode { get; set; } = string.Empty;
        public ProductAlias Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }

}
