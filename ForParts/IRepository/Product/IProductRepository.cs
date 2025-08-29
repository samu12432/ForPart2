
using ForParts.Models.Product;
using ProductAlias = ForParts.Models.Product.Product;

namespace ForParts.IRepository.Product
{
    public interface IProductRepository
    {
        Task<bool> AddAsync(Models.Product.Product product);
        Task AddStockMovementAsync(ProductMovement nuevoMovimientoStock);
        Task<bool> ExistProductAsync(string codeProduct);
        Task<IEnumerable<ProductAlias>> GetAllAsync();
        Task<IEnumerable<ProductMovement>> GetAllStockMovements();
        Task<ProductAlias> GetProductByCodeAsync(string codeProduct);
        Task<IEnumerable<ProductAlias>> GetProductsUsingSupply(string codeSupply);
        Task<ProductAlias> GetProductWithSupplies(string codeProduct);
        Task<int> GetStockByCode(string codeProduct);
        Task<bool> UpdateAsync(ProductAlias exist);
    }
}
