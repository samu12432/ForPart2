using ForParts.DTOs.Supply;

namespace ForParts.IServices.Supply
{
    public interface ISupplyService<T>
    {
        Task<bool> AddSupplyAsync(T newSupplie);

        Task<bool> DeleteSupplyAsync(string codeSupply);

        Task<bool> updateSupply(EditSupplyDto codeSupply);

        Task<bool> updateImageSupply(EditImageSupplyDto codeSupply);

        Task<bool> updatePriceSupply(EditPriceSupplyDto codeSupply);
    }
}
