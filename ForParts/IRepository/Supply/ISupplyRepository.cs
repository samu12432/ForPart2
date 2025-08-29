using ForParts.Models.Enums;
using ForParts.Models.Supply;
using SUPPLY = ForParts.Models.Supply.Supply;
namespace ForParts.IRepository.Supply
{
    public interface ISupplyRepository<T> : ISupplyExisting
        where T : SUPPLY
    {
        Task<bool> ExistSupplyByCode(string codeSupply);

        Task<bool> AddAsync(T newSupply);

        Task<bool> DeleteAsync(string codeSupply);

        Task<T?> GetSupplyByCode(string codeSupply);

        Task<bool> UpdateSupply<TEntity>(TEntity existing) where TEntity : SUPPLY;
        Task<Profile> GetByCodeAsync(string codigo);
        Glass GetGlassByType(GlassType tipoVidrio, string espesor);
    }
}
