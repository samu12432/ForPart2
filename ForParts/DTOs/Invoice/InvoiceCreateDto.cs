namespace ForParts.DTOs.Invoice
{
    public class InvoiceCreateDto
    {
        public DateTime InvoiceDateCreate { get; set; } = DateTime.Today;
        public string InvoiceCompany { get; set; } = "ED Aluminios";
        public int InvoiceBranchOfCompanyId { get; set; } = 1;
        public string InvoiceCurrencyCode { get; set; } = "USD";
        public decimal TipoCambio { get; set; } = 1;
        public string InvoiceDescription { get; set; } = "Gracias por su compra.";
        public DateTime InvoiceExpirationDate { get; set; } = DateTime.Today.AddDays(30);
        public int CustomerId { get; set; }
        public List<InvoiceItemCreateDto> Items { get; set; } = new();
    }

}
