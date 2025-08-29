using ForParts.Data;
using ForParts.IRepository;
using ForParts.IRepository.Budget;
using ForParts.Models.Budgetes;
using budgets = ForParts.Models.Budgetes.Budget;

namespace ForParts.Repository
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
             await _context.SaveChangesAsync();

            return presupuesto;
        } 
    }
}
