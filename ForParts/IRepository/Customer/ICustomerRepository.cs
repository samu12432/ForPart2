
using CustomerAlias = ForParts.Models.Customers.Customer;
namespace ForParts.IRepository.Customer
{
    public interface ICustomerRepository
    {
        Task<CustomerAlias> GetByIdAsync(int customerId);
    }
}
