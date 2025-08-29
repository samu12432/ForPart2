
using ForParts.Models.Budgetes;
//using ForParts.ForParts
using budgets = ForParts.Models.Budgetes.Budget;
namespace ForParts.IRepository
{
    public interface IBudgetRepository
    {
        Task<budgets?> Add(budgets presupuesto);
    }
}