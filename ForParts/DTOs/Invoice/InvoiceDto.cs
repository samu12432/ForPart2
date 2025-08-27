using ForParts.Models.Enums;

namespace ForParts.DTOs.Invoice
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDateCreate { get; set; }
        public string InvoiceCompany { get; set; }
        public int InvoiceBranchOfCompanyId { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public decimal TipoCambio { get; set; }
        public string InvoiceDescription { get; set; }
        public DateTime InvoiceExpirationDate { get; set; }
        public InvoiceState InvoiceState { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }

}
