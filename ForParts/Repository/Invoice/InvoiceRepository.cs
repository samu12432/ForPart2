using System;
using ForParts.Data;
using ForParts.IRepository.Invoice;
using ForParts.Models.Enums;
using ProductAlias = ForParts.Models.Product.Product;
using Microsoft.EntityFrameworkCore;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;
using ForParts.Exceptions.Invoice;


namespace ForParts.Repository.Invoice
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ContextDb _context;

        public InvoiceRepository(ContextDb context)
        {
            _context = context;
        }

        public async Task AddAsync(InvoiceAlias invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<InvoiceAlias?> GetByIdWithItemsAsync(int invoiceId)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                    .ThenInclude(item => item.Product)
                .Include(i => i.Customer)
                    .ThenInclude(c => c.DireccionFiscal)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
        }

        public async Task<List<InvoiceAlias>> GetAllAsync(DateTime? desde, DateTime? hasta, string? estado)
        {
            var query = _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Customer)
                .AsQueryable();

            if (desde.HasValue)
                query = query.Where(i => i.InvoiceDateCreate >= desde.Value);

            if (hasta.HasValue)
                query = query.Where(i => i.InvoiceDateCreate <= hasta.Value);

            if (!string.IsNullOrWhiteSpace(estado) && Enum.TryParse<InvoiceState>(estado, out var parsed))
                query = query.Where(i => i.InvoiceState == parsed);

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(InvoiceAlias invoice)
        {
            var existing = await _context.Invoices.FindAsync(invoice.InvoiceId);
            if (existing == null)
                throw new InvoiceException("No se encontró la factura para actualizar.");

            _context.Entry(existing).CurrentValues.SetValues(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsProductInto(string codeProduct)
        {
            return await _context.Invoices
                .SelectMany(i => i.Items)
                .AnyAsync(item => item.Product.codeProduct == codeProduct);
        }

        public async Task<bool> ExistInInvoice(string codeSupply)
        {
            return await _context.Invoices
                .SelectMany(i => i.Items)
                .AnyAsync(item => item.Product.ProductoInsumos
                    .Any(insumo => insumo.supply.codeSupply == codeSupply));
        }

        public async Task<IEnumerable<ProductAlias>> GetFacturadosByProductIds(List<int> productsId)
        {
            return await _context.Invoices
                .SelectMany(i => i.Items)
                .Where(item => productsId.Contains(item.Product.productId))
                .Select(item => item.Product)
                .Distinct()
                .ToListAsync();
        }
    }
}
