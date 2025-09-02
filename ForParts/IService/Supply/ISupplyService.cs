using ForParts.DTOs.Supply;

namespace ForParts.IServices.Supply
{
    public interface ISupplyService<T>
    {
        Task<bool> AddSupplyAsync(T newSupplie);

        Task<bool> DeleteSupplyAsync(string codeSupply);

        Task<bool> DeleteByCodeAsync(string codeSupply);

        Task<bool> UpdateDescriptionAsync(EditSupplyDto dto);

        Task<bool> UpdateImageAsync(EditImageSupplyDto dto);

        Task<bool> UpdatePriceAsync(EditPriceSupplyDto dto);

        Task<List<T>> GetAllAsync();
    }
}
