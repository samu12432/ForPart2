
using ForParts.Models.Supply;

namespace ForParts.IRepository.Supply
{
    public interface IStockRepository
    {
        Task<bool> AddAsync(Stock newStock);
        Task<IEnumerable<Stock>> GetAllStock();
        Task<Stock?> GetStockByCode(string codeSupply);
        Task<bool> GetStockBySku(string codeSupply);
        Task<bool> UpdateStockAsync(Stock exist);

        // Movimientos de stock
        Task<bool> AddStockMovementAsync(StockMovement newMovement);
        Task<IEnumerable<StockMovement>> GetAllStockMovements();
        List<Profile> GetAvailableProfiles(string codeSupply, string color, decimal largoNecesario);
        int GetAvailableQuantity(int idSupply);
    }
}
