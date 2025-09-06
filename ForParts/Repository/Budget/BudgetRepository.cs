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
    }
}
