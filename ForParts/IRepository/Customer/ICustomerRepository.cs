
using CustomerAlias = ForParts.Models.Customers.Customer;
namespace ForParts.IRepository.Customer
{
    public interface ICustomerRepository
    {
        Task<CustomerAlias> GetByIdAsync(int customerId);
        Task<CustomerAlias> AddAsync(CustomerAlias entity);
        Task UpdateAsync(CustomerAlias entity);
        Task DeleteAsync(CustomerAlias entity);
        Task<(IReadOnlyList<CustomerAlias> Items, int Total)> ListAsync(int page, int pageSize, string? q, string sortBy, bool desc);
    }
}
