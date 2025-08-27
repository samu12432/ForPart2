using API_REST_PROYECT.Data;
using API_REST_PROYECT.IRepository;
using API_REST_PROYECT.Models.Budget;

namespace API_REST_PROYECT.Repository
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ContextDb _context;

        public BudgetRepository(ContextDb context)
        {
            _context = context;
        }

        public async Task<Budget?> Add(Budget presupuesto) => await _context.Budgets.Add(presupuesto);
    }
}
