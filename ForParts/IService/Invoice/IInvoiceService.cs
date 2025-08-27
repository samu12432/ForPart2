using ForParts.DTOs.Invoice;

namespace ForParts.IService.Invoice
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CrearFacturaAsync(InvoiceCreateDto dto);
        Task<InvoiceDto?> ObtenerFacturaPorIdAsync(int invoiceId);
    }
}
