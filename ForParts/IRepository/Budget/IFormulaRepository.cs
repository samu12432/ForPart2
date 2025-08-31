using ForParts.Models.Budgetes;
using ForParts.Models.Enums;

namespace ForParts.IRepository.Budget
{
    public interface IFormulaRepository
    {
        //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
        Formula GetFormula(string codigoInsumo, int seriePerfil, int tipoProducto, string descripcion);
    }
}
