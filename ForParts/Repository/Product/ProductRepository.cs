using ForParts.Data;
using ForParts.IRepository.Product;
using ForParts.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Repository.Product
{
    public class ProductRepository : IProductRepository
    {

        private readonly ContextDb _context;

        public ProductRepository(ContextDb context)
        {
            _context = context;
        }


        public async Task<bool> AddAsync(Models.Product.Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddStockMovementAsync(ProductMovement nuevoMovimientoStock)
        {
            var producto = await _context.Products
                .FirstOrDefaultAsync(p => p.codeProduct == nuevoMovimientoStock.CodeProduct);

            if (producto == null)
                throw new InvalidOperationException("Producto no encontrado.");

            producto.StockActual += nuevoMovimientoStock.Quantity;

            _context.ProductMovements.Add(nuevoMovimientoStock);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistProductAsync(string codeProduct)
        {
            return await _context.Products.AnyAsync(p => p.codeProduct == codeProduct);
        }

        public async Task<IEnumerable<Models.Product.Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.ProductoInsumos)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductMovement>> GetAllStockMovements()
        {
            return await _context.ProductMovements
                .Include(m => m.CodeProduct)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<Models.Product.Product?> GetProductByCodeAsync(string codeProduct)
        {
            return await _context.Products
                .Include(p => p.ProductoInsumos)
                .FirstOrDefaultAsync(p => p.codeProduct == codeProduct);
        }

        public async Task<IEnumerable<Models.Product.Product>> GetProductsUsingSupply(string codeSupply)
        {
            return await _context.Products
                .Where(p => p.ProductoInsumos.Any(i => i.supply.codeSupply == codeSupply))
                .ToListAsync();
        }

        public async Task<Models.Product.Product?> GetProductWithSupplies(string codeProduct)
        {
            return await _context.Products
                .Include(p => p.ProductoInsumos)
                .FirstOrDefaultAsync(p => p.codeProduct == codeProduct);
        }

        public async Task<int> GetStockByCode(string codeProduct)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.codeProduct == codeProduct);

            return product?.StockActual ?? 0;
        }

        public async Task<bool> UpdateAsync(Models.Product.Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
