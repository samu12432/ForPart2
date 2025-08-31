using ForParts.DTOs.Budget;
using ForParts.DTOs.Product;
using ForParts.Exceptions.Budget;
using ForParts.IRepository;
using ForParts.IRepository.Budget;
using ForParts.IService.Buget;
using ForParts.IServices;
using ForParts.Models.Customers;
using ForParts.Models.Enums;
using ForParts.Models.Product;
using Microsoft.SqlServer.Management.Smo;
using budgets = ForParts.Models.Budgetes.Budget;

namespace ForParts.Services.Budget
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository RepoPresupuesto;
        private readonly IBudgetCalculator CalculadoraPresupuesto;


        public BudgetService(IBudgetRepository repoPresupuesto, IBudgetCalculator calculadoraPresupuesto)
        {
            RepoPresupuesto = repoPresupuesto;
            CalculadoraPresupuesto = calculadoraPresupuesto;

        }


        //private readonly IBudgetRepository _budgetRepository;
        public async Task<budgets?> CreateBudgetAsync(BudgetCreateDto dto)
        {
            //Validamos nuevamente que no sea nulo el dto
            if (dto == null)
                throw new BudgetException("Datos invalidos");

            //var productosPresupuestados = new List<BudgetedProduct>();
            decimal precioTotalPresupuesto = 0;

            ProductBudgetDto productoAPresupuestar = dto.Producto;

            BudgetedProduct productoPresupuestado = new BudgetedProduct();
         
                switch (productoAPresupuestar.TypeProduct)
                {
                    case ProductType.Ventana:

                        var productoVentana = await CalcularVentana(productoAPresupuestar);
                        precioTotalPresupuesto += productoVentana.TotalPrice;
                        productoPresupuestado = productoVentana;
                        break;

                    case ProductType.Puerta:
                        var productoPuerta = await CalcularPuerta(productoAPresupuestar);
                        precioTotalPresupuesto += productoPuerta.TotalPrice;
                        productoPresupuestado = productoPuerta;
                        break;

                    default:
                        throw new Exception("El tipo de producto no existe.");
                }

            var presupuesto = new budgets
            {
                Customer = new Customer
                {
                    Nombre = dto.Cliente.Nombre,
                    Telefono = dto.Cliente.Telefono,
                    DireccionFiscal = dto.Cliente.DireccionFiscal,
                    Identificador = dto.Cliente.Identificador,
                    TipoDocumento = dto.Cliente.TipoDocumento,
                    Email = dto.Cliente.Email
                   

                    },

                State = StateBudget.Borrador,
                //Products = productosPresupuestados,
                Product = productoPresupuestado,
                TotalPrice = precioTotalPresupuesto
            };

            var para_ver_presupuesto = presupuesto;
            var pr = await RepoPresupuesto.Add(presupuesto);
            var para_ver = pr;
            return pr;
        }

        private async Task<BudgetedProduct> CalcularVentana(ProductBudgetDto prodDto)
        {
            BudgetedProduct ventana;
           

            switch (prodDto.Serie)
            {
                case SerieProfile.Serie_20:
                    ventana = await CalculadoraPresupuesto.CalcularPresupuestoVentanaS20(prodDto);
                    return ventana;
                case SerieProfile.Serie_25:
                    ventana = await CalculadoraPresupuesto.CalcularPresupuestoVentanaS25(prodDto);
                    return ventana;
                case SerieProfile.Linea_Probba:
                    ventana = await CalculadoraPresupuesto.CalcularPresupuestoVentanaProbba(prodDto);
                    return ventana;
                case SerieProfile.Linea_Gala:
                    ventana = await CalculadoraPresupuesto.CalcularPresupuestoVentanaGala(prodDto);
                    return ventana;
                case SerieProfile.Linea_Gala_CR:
                    ventana = await CalculadoraPresupuesto.CalcularPresupuestoVentanaGalaCR(prodDto);
                    return ventana;
                case SerieProfile.Linea_Summa:
                    ventana = await CalculadoraPresupuesto.CalcularPresupuestoVentanaSumma(prodDto);
                    return ventana;
                default:
                    throw new Exception("El tipo de producto no existe.");
            }

        }


        private async Task<BudgetedProduct> CalcularPuerta(ProductBudgetDto prodDto)
        {

            BudgetedProduct puerta;


            switch (prodDto.Serie)

            ///HAY QUE CAMBIAR TODO A UN METODO QUE DEVUELVA UNA PUERTA CALCULADA.
            {
                case SerieProfile.Serie_20:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoVentanaS20(prodDto);
                    return puerta;
                case SerieProfile.Serie_25:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoVentanaS25(prodDto);
                    return puerta;
                case SerieProfile.Linea_Probba:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoVentanaProbba(prodDto);
                    return puerta;
                case SerieProfile.Linea_Gala:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoVentanaGala(prodDto);
                    return puerta;
                case SerieProfile.Linea_Gala_CR:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoVentanaGalaCR(prodDto);
                    return puerta;
                case SerieProfile.Linea_Summa:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoVentanaSumma(prodDto);
                    return puerta;
                default:
                    throw new Exception("Tipo de producto no soportado.");
            }
            
        }
    }
}
