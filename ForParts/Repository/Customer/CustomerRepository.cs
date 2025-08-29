using ForParts.Data;
using ForParts.IRepository.Customer;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Repository.Customer
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly ContextDb _context;

        public CustomerRepository(ContextDb context)
        {
            _context = context;
        }

        public Task<Models.Customers.Customer> GetByIdAsync(int customerId)
        {
            return _context.Customers.FirstOrDefaultAsync(i => i.CustomerId == customerId); ;
        }
    }
}
