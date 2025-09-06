using ForParts.DTOs.Product;
using ForParts.Models.Product;
namespace ForParts.IRepository.Budget
{
    public interface IBudgetCalculator
    {
    
        //VENTANAS
        Task<BudgetedProduct> CalcularPresupuestoVentanaS20(ProductBudgetDto dto);
        Task<BudgetedProduct> CalcularPresupuestoVentanaS25(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoVentanaProbba(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoVentanaGalaCR(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoVentanaGala(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoVentanaSumma(ProductBudgetDto prodDto);

        //PUERTAS
        Task<BudgetedProduct> CalcularPresupuestoPuertaS30(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoPuertaMecal30Interior(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoPuertaMecal30Exterior(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoPuertaProbba(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoPuertaGala(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoPuertaSumma(ProductBudgetDto prodDto);

        //Batiente
        Task<BudgetedProduct> CalcularPresupuestoBatienteS30(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoBatienteMecal30(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoBatienteProbba(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoBatienteGala(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoBatienteSumma(ProductBudgetDto prodDto);

        //Tabaqueras
        Task<BudgetedProduct> CalcularPresupuestoTabaqueraS30(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoTabaqueraMecal30(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoTabaqueraProbba(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoTabaqueraGala(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoTabaqueraSumma(ProductBudgetDto prodDto);

        //Proyectante
        Task<BudgetedProduct> CalcularPresupuestoProyectanteS30(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoProyectanteMecal30(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoProyectanteProbba(ProductBudgetDto prodDto);

        Task<BudgetedProduct> CalcularPresupuestoProyectanteGala(ProductBudgetDto prodDto);
        Task<BudgetedProduct> CalcularPresupuestoProyectanteSumma(ProductBudgetDto prodDto);

    }
}




