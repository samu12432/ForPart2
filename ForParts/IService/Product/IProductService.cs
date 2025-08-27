
using ForParts.DTO.Product;
using ProductAlias = ForParts.Models.Product.Product;
using ForParts.DTOs.Product;
using ForParts.Models.Product;

namespace ForParts.IService.Product
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(ProductDto dto);
        Task<bool> DeleteProductAsync(string codeProduct);
        Task<IEnumerable<ProductMovementDto>> GetAllProductMovements();
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductAlias> GetProductByCode(string codeProduct);
        Task<List<SupplyNecessaryDto>> GetSuppliesForProducts(string codeProduct);
        Task<bool> UpdateDescriptionProductAsync(UpdateDescriptionProductDto dto);
        Task<bool> UpdateImageProductAsync(UpdateImageProductDto dto);
    }
}
