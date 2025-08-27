using API_REST_PROYECT.Models.Budget;

namespace API_REST_PROYECT.IRepository
{
    public interface IFormulaRepositorio
    {
        Formula GetFormula(string codigoInsumo, string seriePerfil, string tipoProducto, string descripcion);
    }
}
