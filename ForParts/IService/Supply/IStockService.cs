using ForParts.DTOs.Supply;
using ForParts.Models.Supply;

namespace ForParts.IServices.Supply
{
    public interface IStockService
    {
        Task<bool> AddStock(StockDto dto);
        Task<IEnumerable<StockDto>> GetAllStock();
        Task<Stock> GetStockBySku(string sku);
        Task<bool> UpdateStock(StockDto dto);

        Task<IEnumerable<StockMovementDto>> GetAllStockMovements();
        Task<bool> DeleteStock(string codeSupply);
    }
}
