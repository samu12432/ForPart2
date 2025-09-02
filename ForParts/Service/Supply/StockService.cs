using ForParts.DTOs.Supply;
using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.IServices.Supply;
using SupplyAlias = ForParts.Models.Supply.Supply;
using ForParts.Models.Supply;
using AutoMapper;
using ForParts.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using ForParts.IRepository.Product;
using ForParts.Exceptions.Product;
using ForParts.IRepository.Invoice;
using ForParts.Models.Product;

namespace ForParts.Services.Supply
{
    public class StockService : IStockService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; //Nos ayuda a poder registrar quien realiza el movimiento de stock
        private readonly IStockRepository _stockRepository;
        private readonly ISupplyRepository<SupplyAlias> _supplyRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public StockService(IHttpContextAccessor httpContextAccessor, IStockRepository stockRepository,
            ISupplyRepository<SupplyAlias> supplyRepository, 
            IProductRepository productRepository, IInvoiceRepository invoiceRepository,
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _stockRepository = stockRepository;
            _supplyRepository = supplyRepository;
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        //Add
        public async Task<bool> AddStock(StockDto dto)
        {
            //Nuevamente validamos que el dto pasado por parametro no sea nulo (no se pierda info en el hilo)
            if (dto == null) throw new StockException("Datos incorrectos.");

            //Validamos que el insumo ingresado exista
            var supply = await _supplyRepository.ExistSupplyByCode(dto.codeSupply);

            if (!supply)
                throw new SupplyException("No se encontró algun insumo con el código " + dto.codeSupply + ".");

            //Validamos que no exista un stock referente al codigo del articulo ingresado
            var exist = await _stockRepository.GetStockBySku(dto.codeSupply);
            if (exist)
                throw new StockException("El articulo " + dto.codeSupply + " cuenta con stock registrado.");

            //Creamos el objeto Stock
            Stock newStock = _mapper.Map<Stock>(dto);

            //Guardamos en la base de datos
            //Retornamos exito en la tarea
            var succes =  await _stockRepository.AddAsync(newStock);

            //Luego de agregar al sistema el stock correspondiente, registramos el movimiento del mismo para futuras o posibles auditorias
            if (succes)
            {
                await mappeoDeMovimiento(dto.codeSupply, dto.stockQuantity, TypeMovement.Alta);
            }

            return succes;
        }

        //Update
        public async Task<bool> UpdateStock(StockDto dto)
        {
            //Nuevamente validamos que el dto pasado por parametro no sea nulo (no se pierda info en el hilo)
            if (dto == null) throw new StockException("Datos incorrectos.");

            //Verificamos que exista stock referente al codigo del insumo ingresado
            Stock? exist = await _stockRepository.GetStockByCode(dto.codeSupply);
            if (exist == null)
                throw new SupplyException("No existe registro de stock referente al codigo " + dto.codeSupply + ".");

            exist.UpdateQuantity(dto.stockQuantity);

            var succes = await _stockRepository.UpdateStockAsync(exist);

            //Luego de actualizar al sistema el stock correspondiente, registramos el movimiento del mismo para futuras o posibles auditorias
            if (succes)
            {
                await mappeoDeMovimiento(dto.codeSupply, dto.stockQuantity, TypeMovement.Edicion);
            }

            return succes;
        }


        //Baja
        public async Task<bool> DeleteStock(string codeSupply)
        {
            //Verificamos que el codigo no sea nulo o empty
            if (string.IsNullOrWhiteSpace(codeSupply)) throw new StockException("Es necesario ingresar el codigo del insumo");

            //Verificamos que el insumo tenga stock creado
            var stock = await _stockRepository.GetStockByCode(codeSupply);
            if(stock == null) throw new StockException($"El codigo {codeSupply} no cuenta con stock registrado. No es posible eliminar");

            //Verificamos que el insumo no sea requerido para algun producto
            var productos = await _productRepository.GetProductsUsingSupply(codeSupply);
            if (productos.Any())
            {
                var productsId = productos.Select(x => x.productId).ToList();
                var facturados = await _invoiceRepository.GetFacturadosByProductIds(productsId);

                if (facturados.Any())
                    throw new StockException($"El insumo {codeSupply} está en productos facturados y no puede ser desactivado.");

                // Bloquear si está en productos no facturados
                throw new StockException($"El insumo {codeSupply} está en uso por productos activos y no puede ser desactivado.");
            }

            var success = await desactivarStock(stock);

            await mappeoDeMovimiento(stock.codeSupply, stock.stockQuantity, TypeMovement.Baja);
             
            return success;
        }

        private async Task<bool> desactivarStock(Stock stock)
        {
            stock.changeState();
            var success = await _stockRepository.UpdateStockAsync(stock);
            return success;
        }

        //Cantidad
        public async Task<IEnumerable<StockDto>> GetAllStock()
        {
            //Obtenemos y verificamos si existen registros
            var stocks = await _stockRepository.GetAllStock();
            if (stocks == null)
                throw new StockException("No hay registro de stock");

            //Antes de devolver un listado, setiamos la salida a informacion del Dto
            return _mapper.Map<IEnumerable<StockDto>>(stocks);
        }

        public async Task<Stock> GetStockBySku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) throw new StockException("Es necesario el codigo del insumo");
            //Buscamos y validamos que exista registro de stock con el Sku del insumo ingresado
            var stock = await _stockRepository.GetStockByCode(sku);
            if (stock == null)
                throw new StockException("No existe registro de stock relacionado al codigo " + sku + ".");

            return stock;
        }

        //Movimiento
        public async Task registerMovement(StockMovementDto dto, string userName)
        {
            if (dto == null) throw new StockException("Datos incorrectos.");
            if (string.IsNullOrWhiteSpace(userName)) throw new StockException("No se pudo identificar el usuario que realiza el movimiento.");


            StockMovement nuevoMovimientoStock = _mapper.Map<StockMovement>(dto);
            nuevoMovimientoStock.MovementDate = DateTime.UtcNow;
            nuevoMovimientoStock.UserName = userName;

            // Guardar movimiento
            await _stockRepository.AddStockMovementAsync(nuevoMovimientoStock);
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllStockMovements()
        {
            var movements = await _stockRepository.GetAllStockMovements();
            return _mapper.Map<IEnumerable<StockMovementDto>>(movements);
        }

        public async Task mappeoDeMovimiento(string codeSupply, int quantity, TypeMovement tipo) {
            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema";

            var movimentDto = new StockMovementDto{ 
                CodeSupply = codeSupply,
                QuantityChange = quantity,
                MovementType = tipo,
            };

            await registerMovement(movimentDto, user);
        }
    }
}
