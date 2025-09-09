
using ForParts.Models.Budgetes;
using budgets = ForParts.Models.Budgetes.Budget;

namespace ForParts.IRepository.Budget
{
    public interface IBudgetRepository
    {
        Task<budgets?> Add(budgets presupuesto);
        Task<budgets?> FindByIdForPdfAsync(int id, CancellationToken ct = default);
    }
}