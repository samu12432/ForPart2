using API_REST_PROYECT.DTOs.Budget;
using API_REST_PROYECT.DTOs.Product;
using API_REST_PROYECT.Exceptions.Budget;
using API_REST_PROYECT.IRepository;
using API_REST_PROYECT.IServices;
using API_REST_PROYECT.Models.Client;
using API_REST_PROYECT.Models.Enum;
using API_REST_PROYECT.Models.Products;
using Microsoft.SqlServer.Management.Smo;
using budgets = API_REST_PROYECT.Models.Budget.Budget;

namespace API_REST_PROYECT.Services.Budget
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository RepoPresupuesto;
        private readonly ICalculadoraPresupuesto CalculadoraPresupuesto;


        public BudgetService(IBudgetRepository repoPresupuesto, ICalculadoraPresupuesto calculadoraPresupuesto)
        {
            RepoPresupuesto = repoPresupuesto;
            CalculadoraPresupuesto = calculadoraPresupuesto;

        }


        //private readonly IBudgetRepository _budgetRepository;
        public Task<budgets?> CreateBudgetAsync(BudgetCreateDto dto)
        {
            //Validamos nuevamente que no sea nulo el dto
            if(dto == null)
                throw new BudgetException("Datos invalidos");

            var productosPresupuestados = new List<ProductoPresupuestado>();
            decimal precioTotalPresupuesto = 0;

            foreach (var prodDto in dto.Productos)
            {

                switch (prodDto.Tipo)
                {
                    case ProductCategory.Ventana:

                        var productoVentana = CalcularVentana(prodDto);
                        precioTotalPresupuesto += productoVentana.PrecioTotal;
                        productosPresupuestados.Add(productoVentana);
                        break;

                    case ProductCategory.Puerta:
                        var productoPuerta = CalcularPuerta(prodDto);
                        precioTotalPresupuesto += productoPuerta.PrecioTotal;
                        productosPresupuestados.Add(productoPuerta);
                        break;

                    default:
                        throw new Exception("El tipo de producto no existe.");
                }
            }
            var presupuesto = new budgets
            {
                customer = new Customer
                {
                    ClienteNombre = dto.Cliente.ClienteNombre,
                    ClienteTelefono = dto.Cliente.ClienteTelefono,
                    ClienteDireccion = dto.Cliente.ClienteDireccion
                },
                Estado = StateBudget.Borrador,
                Productos = productosPresupuestados,
                PrecioTotal = precioTotalPresupuesto
            };

            var pr = RepoPresupuesto.Add(presupuesto);
            return pr;
        }

        private ProductoPresupuestado CalcularVentana(ProductBudgetDto prodDto)
        {
            ProductoPresupuestado ventana;


            switch (prodDto.Serie)
            {
                case SerieProfile.Serie_20:
                    ventana = CalculadoraPresupuesto.CalcularPresupuestoVentanaS20(prodDto);
                    return ventana;
                case SerieProfile.Serie_25:
                    ventana = CalculadoraPresupuesto.CalcularPresupuestoVentanaS25(prodDto);
                    return ventana;
                case SerieProfile.Linea_Probba:
                    ventana = CalculadoraPresupuesto.CalcularPresupuestoVentanaProbba(prodDto);
                    return ventana;
                case SerieProfile.Linea_Gala:
                    ventana = CalculadoraPresupuesto.CalcularPresupuestoVentanaGala(prodDto);
                    return ventana;
                case SerieProfile.Linea_Gala_CR:
                    ventana = CalculadoraPresupuesto.CalcularPresupuestoVentanaGalaCR(prodDto);
                    return ventana;
                case SerieProfile.Linea_Summa:
                    ventana = CalculadoraPresupuesto.CalcularPresupuestoVentanaSumma(prodDto);
                    return ventana;
                default:
                    throw new Exception("El tipo de producto no existe.");
            }

        }


        private ProductoPresupuestado CalcularPuerta(ProductBudgetDto prodDto)
        {

            ProductoPresupuestado puerta;


            switch (prodDto.Serie)

            ///HAY QUE CAMBIAR TODO A UN METODO QUE DEVUELVA UNA PUERTA CALCULADA.
            {
                case SerieProfile.Serie_20:
                    puerta = CalculadoraPresupuesto.CalcularPresupuestoVentanaS20(prodDto);
                    return puerta;
                case SerieProfile.Serie_25:
                    puerta = CalculadoraPresupuesto.CalcularPresupuestoVentanaS25(prodDto);
                    return puerta;
                case SerieProfile.Linea_Probba:
                    puerta = CalculadoraPresupuesto.CalcularPresupuestoVentanaProbba(prodDto);
                    return puerta;
                case SerieProfile.Linea_Gala:
                    puerta = CalculadoraPresupuesto.CalcularPresupuestoVentanaGala(prodDto);
                    return puerta;
                case SerieProfile.Linea_Gala_CR:
                    puerta = CalculadoraPresupuesto.CalcularPresupuestoVentanaGalaCR(prodDto);
                    return puerta;
                case SerieProfile.Linea_Summa:
                    puerta = CalculadoraPresupuesto.CalcularPresupuestoVentanaSumma(prodDto);
                    return puerta;
                default:
                    throw new Exception("Tipo de producto no soportado.");
            }

        }
    }
}