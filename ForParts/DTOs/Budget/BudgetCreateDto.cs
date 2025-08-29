using ForParts.DTOs.Customer;
using ForParts.DTOs.Product;

namespace ForParts.DTOs.Budget
{
    public class BudgetCreateDto
    {
        public CustomerDto Cliente { get; set; }
        public List<ProductBudgetDto> Productos { get; set; } 
    }
}
