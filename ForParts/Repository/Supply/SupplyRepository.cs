using ForParts.Data;
using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.Models.Enums;
using ForParts.Models.Supply;
using Microsoft.EntityFrameworkCore;
using SUPPLY = ForParts.Models.Supply.Supply;

namespace ForParts.Repositorys.Supply
{
    public class SupplyRepository<TEntity> : ISupplyRepository<TEntity> where TEntity : SUPPLY
    {
        private readonly ContextDb _context;
        private readonly DbSet<TEntity> _dbSet;

        public SupplyRepository(ContextDb context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<bool> ExistSupplyByCode(string codeSupply)
        {
            return await _dbSet.AnyAsync(x => x.codeSupply == codeSupply);
        }
        public async Task<bool> ExistSupplyById(int supplyid)
        {
            return await _dbSet.AnyAsync(x => x.idSupply == supplyid);
        }


        public async Task<bool> AddAsync(TEntity newSupply)
        {
            if (newSupply is Glass glass)
                _context.Glasses.Add(glass);
            else if (newSupply is Profile profile)
                _context.Profiles.Add(profile);
            else if (newSupply is Accessory accessory)
                _context.Accessories.Add(accessory);
            else
                _context.Supplies.Add(newSupply); // Supply base

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> DeleteAsync(string codeSupply)
        {
            var exist = await GetSupplyByCode(codeSupply);
            if (exist != null)
            _context.Supplies.Remove(exist);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public Task<TEntity?> GetSupplyByCode(string codeSupply) => 
            _dbSet.FirstOrDefaultAsync(x => x.codeSupply == codeSupply);

        public async Task<bool> UpdateSupply<TEntity1>(TEntity1 existing) where TEntity1 : SUPPLY
        {
            var existingEntity = _dbSet.FirstOrDefault(x => x.codeSupply == existing.codeSupply);

            if (existingEntity == null) return false;

            _context.Entry(existingEntity).CurrentValues.SetValues(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> GetExistingCodesAsync(List<string> codes)
        {
            return await _context.Supplies
                .Where(s => codes.Contains(s.codeSupply))
                .Select(s => s.codeSupply)
                .ToListAsync();
        }
        public async Task<Profile?> GetByCodeAsync(string codigo)
        {
            return await _context.Supplies
                .OfType<Profile>()                   // solo los derivadas Profile
                .FirstOrDefaultAsync(p => p.codeSupply == codigo);
            // Devuelve un Profile (subtipo), tipado como Supply
        }


        public Glass GetGlassByType(GlassType tipoVidrio, int idInsumo)
        {
            Glass vidrio = _context.Supplies
                .OfType<Glass>()
                .FirstOrDefault(v =>
                    v.glassType == tipoVidrio &&
                    v.idSupply == idInsumo);

            if (vidrio == null)
                throw new GlassException($"No se encontró un vidrio del tipo '{tipoVidrio}'.");

            return vidrio;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.isEnabledSupply == true)
                .ToListAsync();
        }
    }
}
