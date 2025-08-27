using API_REST_PROYECT.DTOs.Product;
using API_REST_PROYECT.Models.Products;

namespace API_REST_PROYECT.IRepository
{
    public interface ICalculadoraPresupuesto
    {
        //VENTANAS
        ProductoPresupuestado CalcularPresupuestoVentanaS20(ProductBudgetDto dto);
        ProductoPresupuestado CalcularPresupuestoVentanaS25(ProductBudgetDto prodDto);
        Task<ProductoPresupuestado> CalcularPresupuestoVentanaProbba(ProductBudgetDto prodDto);
        Task<ProductoPresupuestado> CalcularPresupuestoVentanaGalaCR(ProductBudgetDto prodDto);
        Task<ProductoPresupuestado> CalcularPresupuestoVentanaGala(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoVentanaSumma(ProductBudgetDto prodDto);

        //PUERTAS
        ProductoPresupuestado CalcularPresupuestoPuertaS30(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoPuertaMecal30(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoPuertaProbba(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoPuertaGala(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoPuertaSumma(ProductBudgetDto prodDto);

        //Batiente
        ProductoPresupuestado CalcularPresupuestoBatienteS30(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoBatienteMecal30(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoBatienteProbba(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoBatienteGala(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoBatienteSumma(ProductBudgetDto prodDto);

        //Tabaqueras
        ProductoPresupuestado CalcularPresupuestoTabaqueraS30(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoTabaqueraMecal30(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoTabaqueraProbba(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoTabaqueraGala(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoTabaqueraSumma(ProductBudgetDto prodDto);

        //Proyectante
        ProductoPresupuestado CalcularPresupuestoProyectanteS30(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoProyectanteMecal30(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoProyectanteProbba(ProductBudgetDto prodDto);

        ProductoPresupuestado CalcularPresupuestoProyectanteGala(ProductBudgetDto prodDto);
        ProductoPresupuestado CalcularPresupuestoProyectanteSumma(ProductBudgetDto prodDto);

    }
}


