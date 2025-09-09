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

        public async Task<budgets?> FindByIdAsync(int id, CancellationToken ct = default)
        {
            // delega al repo (AsNoTracking + Includes adentro)
            return await RepoPresupuesto.FindByIdForPdfAsync(id, ct);
        }
        //private readonly IBudgetRepository _budgetRepository;
        public async Task<budgets?> CreateBudgetAsync(BudgetCreateDto dto)
        {
            if (dto == null)
                throw new BudgetException("Datos inválidos");

            if (dto.Productos == null || dto.Productos.Count == 0)
                throw new BudgetException("Debe incluir al menos un producto a presupuestar.");

            var productosPresupuestados = new List<BudgetedProduct>();
            decimal precioTotalPresupuesto = 0m;

            foreach (var producto in dto.Productos)
            {
                switch (producto.TypeProduct)
                {
                    case ProductType.Ventana:
                        {
                            var ventana = await CalcularVentana(producto);   // devuelve BudgetedProduct
                            productosPresupuestados.Add(ventana);
                            precioTotalPresupuesto += ventana.TotalPrice;
                            break;
                        }

                    case ProductType.Puerta:
                        {
                            var puerta = await CalcularPuerta(producto);     // devuelve BudgetedProduct
                            productosPresupuestados.Add(puerta);
                            precioTotalPresupuesto += puerta.TotalPrice;
                            break;
                        }

                    default:
                        throw new BudgetException("El tipo de producto no existe.");
                }
            }

            var presupuesto = new budgets
            {
                Customer = new Customer
                {
                    Nombre = dto.Cliente?.Nombre ?? string.Empty,
                    Telefono = dto.Cliente?.Telefono ?? string.Empty,
                    Email = dto.Cliente?.Email ?? string.Empty,
                    Identificador = dto.Cliente?.Identificador ?? string.Empty,
                    TipoDocumento = dto.Cliente?.TipoDocumento ?? "RUT",
                    DireccionFiscal = dto.Cliente?.DireccionFiscal ?? new Direccion()
                },

                State = StateBudget.Borrador,
                Products = productosPresupuestados,   // <<--- usa lista
                TotalPrice = precioTotalPresupuesto
            };

            // Si tu entidad budgets aún tiene la propiedad singular `Product`,
            // podés dejarla en null o, si querés compatibilidad:
            // presupuesto.Product = productosPresupuestados.FirstOrDefault();

            var resultado = await RepoPresupuesto.Add(presupuesto);
            return resultado;
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


            {
                case SerieProfile.Serie_30:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoPuertaMecal30Interior(prodDto);
                    return puerta;
                case SerieProfile.Linea_Probba:
                    puerta = await CalculadoraPresupuesto.CalcularPresupuestoPuertaProbba(prodDto);
                    return puerta;

                default:
                    throw new Exception("Tipo de producto no soportado.");
            }

        }
       
    }
}
