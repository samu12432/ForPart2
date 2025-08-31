using ForParts.DTOs.Customer;
using ForParts.DTOs.Product;

namespace ForParts.DTOs.Budget
{
    public class BudgetCreateDto
    {
        public CustomerDto Cliente { get; set; }
        public ProductBudgetDto Producto { get; set; } 
    }
}
