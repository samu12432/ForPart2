

using InvoiceAlias = ForParts.Models.Invoice.Invoice;

namespace ForParts.IRepository.Invoice
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Models.Invoice.Invoice invoice);
        Task<InvoiceAlias> GetByIdWithItemsAsync(int invoiceId);
    }
}
