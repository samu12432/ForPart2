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
            return _context.Customers.FirstOrDefaultAsync(i => i.CustomerId == customerId);
        }

        public async Task<Models.Customers.Customer> AddAsync(Models.Customers.Customer entity)
        {
            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Models.Customers.Customer entity)
        {
            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Models.Customers.Customer entity)
        {
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(IReadOnlyList<Models.Customers.Customer> Items, int Total)> ListAsync(int page, int pageSize, string? q, string sortBy, bool desc)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c => c.Nombre.ToLower().Contains(q.ToLower()) || c.Identificador.ToLower().Contains(q.ToLower()));
            }

            var total = await query.CountAsync();

            query = sortBy.ToLower() switch
            {
                "identificador" => desc ? query.OrderByDescending(c => c.Identificador) : query.OrderBy(c => c.Identificador),
                "email" => desc ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
                "telefono" => desc ? query.OrderByDescending(c => c.Telefono) : query.OrderBy(c => c.Telefono),
                _ => desc ? query.OrderByDescending(c => c.Nombre) : query.OrderBy(c => c.Nombre),
            };

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
