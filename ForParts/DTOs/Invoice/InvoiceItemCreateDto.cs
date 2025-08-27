namespace ForParts.DTOs.Invoice
{
    public class InvoiceItemCreateDto
    {
        public string ProductCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
