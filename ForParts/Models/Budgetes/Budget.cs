using ForParts.Models;
using ForParts.Models.Enums;
using ForParts.Models.Product;
using ForParts.Models.Customers;
//using CustomerEntity = ForParts.Models.Customers.Customer;
using ForParts.Models.Customers;
//ACA DEJE
namespace ForParts.Models.Budgetes
{
    public class Budget
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public Customer Customer { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public StateBudget State { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BudgetedProduct> Products { get; set; }
    }
}
