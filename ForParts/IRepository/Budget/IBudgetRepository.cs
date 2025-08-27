using API_REST_PROYECT.Models.Budget;
using API_REST_PROYECT.Models.Products;

namespace API_REST_PROYECT.IRepository
{
    public interface IBudgetRepository
    {
        Task<Budget?> Add(Budget presupuesto);
    }
}