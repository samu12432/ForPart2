using ForParts.DTOs.Invoice;

namespace ForParts.Utils
{
    public interface IZureoApi
    {
        Task<string> SendInvoiceAsync(InvoiceDto zureoDto);
    }
}


