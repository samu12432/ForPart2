using ForParts.Data;
using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.Models.Supply;
using Microsoft.EntityFrameworkCore;

namespace ForParts.Repositorys.Supply
{
    public class StockRepository : IStockRepository
    {
        private readonly ContextDb _contextDb;
        public StockRepository(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<bool> AddAsync(Stock newStock)
        {
            _contextDb.Stocks.Add(newStock);
            return await _contextDb.SaveChangesAsync() > 0; 
        }

        public async Task<bool> UpdateStockAsync(Stock exist)
        {
            var existingEntity = await GetStockByCode(exist.codeSupply);
            if (existingEntity == null)
                throw new StockException($"No se encontró stock para el código {exist.codeSupply}.");
           
            _contextDb.Entry(existingEntity).CurrentValues.SetValues(exist);
            return await _contextDb.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Stock>> GetAllStock() =>
            await _contextDb.Stocks.Include(s => s.Supply).ToListAsync();

        public Task<Stock?> GetStockByCode(string codeSupply) =>
            _contextDb.Stocks.FirstOrDefaultAsync(m => m.codeSupply == codeSupply);

        public async Task<bool> GetStockBySku(string codeSupply)
        {
            var stock = await _contextDb.Stocks.FirstOrDefaultAsync(m => m.codeSupply == codeSupply);
            return stock != null;
        }

        public async Task<bool> AddStockMovementAsync(StockMovement newMovement)
        {
            _contextDb.Set<StockMovement>().Add(newMovement);
            return await _contextDb.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<StockMovement>> GetAllStockMovements()
        {
            return await _contextDb.Set<StockMovement>().ToListAsync();
        }
        public List<Profile> GetAvailableProfiles(string codeSupply, string color, decimal largoNecesario)
        {
            return _contextDb.Stocks
                .Where(s => s.codeSupply == codeSupply && s.Supply != null)
                  .Select(s => s.Supply)            
                  .OfType<Profile>()              
                  .Where(p =>
                      p.profileColor.ToLower() == color &&
                      p.profileHeigth >= largoNecesario       
      )
      .AsNoTracking()
      .ToList();
        }

        public int GetAvailableQuantity(int idSupply)
        {
            return _contextDb.Stocks
                .Where(s => s.Supply.idSupply == idSupply)
                .Sum(s => s.stockQuantity);


        }
    }
}
