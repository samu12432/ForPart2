using ForParts.Data;
using ForParts.IRepository.Budget;
using ForParts.Models.Budgetes;
using ForParts.Models.Enums;

namespace ForParts.Repository.Budget
{
    public class FormulaRepositorio : IFormulaRepository
    {
        private readonly ContextDb Contexto;

        public FormulaRepositorio(ContextDb contexto)
        {
            Contexto = contexto;
        }
        public Formula GetFormula(string codigoInsumo, int seriePerfil, int tipoProducto, string descripcion)
        {

            Formula formula = Contexto.Formulas
                .FirstOrDefault(f =>
                    f.CodigoInsumo == codigoInsumo &&
                    ((int)f.SeriePerfil) == seriePerfil && //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
                    ((int)f.TipoProducto) == tipoProducto && //Para la formula, llega el enum, pero para acceder a la bd. necesitamos el valor INT
                    f.Descripcion == descripcion);

            if (formula == null)
                formula = Contexto.Formulas.First();
            //throw new Exception($"No se encontró fórmula para insumo {codigoInsumo}, serie {seriePerfil}, tipo {tipoProducto}.");

            return formula;
        }

    }
}
