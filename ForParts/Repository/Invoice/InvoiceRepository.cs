using System;
using ForParts.Data;
using ForParts.IRepository.Invoice;
using ForParts.Models.Enums;
using Microsoft.EntityFrameworkCore;
using InvoiceAlias = ForParts.Models.Invoice.Invoice;


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
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
    }
}
