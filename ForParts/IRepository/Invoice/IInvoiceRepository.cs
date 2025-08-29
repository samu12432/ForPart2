
using ProductAlias = ForParts.Models.Product.Product;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;

namespace ForParts.IRepository.Invoice
{
    public interface IInvoiceRepository
    {
        Task AddAsync(InvoiceAlias invoice);
        Task<bool> ExistInInvoice(string codeSupply);
        Task<InvoiceAlias> GetByIdWithItemsAsync(int invoiceId);
        Task<IEnumerable<ProductAlias>> GetFacturadosByProductIds(List<int> productsId);
        Task<bool> IsProductInto(string codeProduct);
    }
}
