using ForParts.Data;
using ForParts.IRepository.Budget;
using ForParts.Models.Budgetes;
using Microsoft.EntityFrameworkCore;
using budgets = ForParts.Models.Budgetes.Budget;

namespace ForParts.Repository.Budget
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ContextDb _context;

        public BudgetRepository(ContextDb context)
        {
            _context = context;
        }

        public async Task<budgets?> Add(budgets presupuesto)
        { 

             await _context.Budgets.AddAsync(presupuesto);
             //await _context.SaveChangesAsync();

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message); // suele decir "Cannot insert the value NULL into column 'X'..."
                throw;
            }
            

            return presupuesto;
        }
        public Task<budgets?> FindByIdForPdfAsync(int id, CancellationToken ct = default)
        {
            // Si Customer.DireccionFiscal es entidad separada y NO owned, dejá el ThenInclude;
            // si es owned (OwnsOne), podés quitarlo.
            // en tu método del repo para PDF
            return _context.Budgets
                .AsNoTracking()
                .Include(b => b.Customer)
                .ThenInclude(c => c.DireccionFiscal) // quitá si es owned
                .Include(b => b.Products)
                .ThenInclude(p => p.SuppliesUsed)    // <<--- importante
                .FirstOrDefaultAsync(b => b.Id == id, ct);


        }
    }
}
