using ForParts.Data;
using ForParts.Models.Budgetes;

namespace ForParts.Repository
{
    public class FormulaRepositorio
    {
        private readonly ContextDb Contexto;

        public FormulaRepositorio(ContextDb contexto)
        {
            Contexto = contexto;
        }
        public Formula GetFormula(string codigoInsumo, string seriePerfil, string tipoProducto, string descripcion)
        {
            Formula formula = Contexto.Formulas
                .FirstOrDefault(f =>
                    f.CodigoInsumo == codigoInsumo &&
                    f.SeriePerfil.ToString() == seriePerfil &&
                    f.TipoProducto.ToString() == tipoProducto &&
                    f.Descripcion == descripcion);

            if (formula == null)
                throw new Exception($"No se encontró fórmula para insumo {codigoInsumo}, serie {seriePerfil}, tipo {tipoProducto}.");

            return formula;
        }

    }
}
