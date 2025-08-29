using ForParts.Models.Budgetes;

namespace ForParts.IRepository.Budget
{
    public interface IFormulaRepository
    {
        Formula GetFormula(string codigoInsumo, string seriePerfil, string tipoProducto, string descripcion);
    }
}
