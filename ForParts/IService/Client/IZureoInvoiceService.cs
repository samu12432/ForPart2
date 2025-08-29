using ForParts.DTOs.Client;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;
namespace ForParts.IService.Client
{
    public interface IZureoInvoiceService
    {
        Task<ZureoResponseDto> EmitirAsync(InvoiceAlias invoice);
    }
}
