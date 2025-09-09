using ForParts.DTOs.Customer;
using ForParts.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace ForParts.DTOs.Budget
{
    public class BudgetCreateDto
    {

        public ProductBudgetDto Producto { get; set; } = new ProductBudgetDto();
        [Required(ErrorMessage = "El cliente es requerido")]
        public CustomerDto Cliente { get; set; } = new CustomerDto();

        [Required(ErrorMessage = "Los productos son requeridos")]
        public List<ProductBudgetDto> Productos { get; set; } = new List<ProductBudgetDto>();
    }
}
