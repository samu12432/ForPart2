using API_REST_PROYECT.DTOs.Custumer;
using API_REST_PROYECT.DTOs.Product;

namespace API_REST_PROYECT.DTOs.Budget
{
    public class BudgetCreateDto
    {
        public CustumerDto Cliente { get; set; }
        public List<ProductBudgetDto> Productos { get; set; } 
    }
}
