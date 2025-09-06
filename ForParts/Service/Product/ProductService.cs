using ForParts.Exceptions.Product;
using ForParts.IRepository.Product;
using Products = ForParts.Models.Product.Product;
using AutoMapper;
using ForParts.IService.Product;
using ForParts.DTO.Product;
using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.IServices.Image;
using ForParts.IRepository.Invoice;
using ForParts.DTOs.Supply;
using ForParts.Models.Enums;
using ForParts.Models.Supply;
using ForParts.Repositorys.Supply;
using ForParts.DTOs.Product;
using ForParts.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace ForParts.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; //Nos ayuda a poder registrar quien realiza el movimiento de stock
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IImageService _imageService;
        private readonly ISupplyExisting _supplyRepository;

        public ProductService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IProductRepository productRepository, IInvoiceRepository invoiceRepository,
        IImageService imageService, ISupplyExisting supplyRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
            _imageService = imageService;
            _supplyRepository = supplyRepository;
        }

        //Add
        public async Task<bool> CreateProductAsync(ProductDto dto)
        {

            if (dto == null) throw new ProductException("Datos incorrectos.");

            if (dto.Image != null)
            {
                dto.imageUrl = await _imageService.SaveImageAsync(dto.Image, "products");
            }

            //Verificamos que no exista previamente este producto
            var exist = await _productRepository.ExistProductAsync(dto.codeProduct);
            if (exist)
                throw new ProductException("Ya existe un producto creado con este codigo.");

            //Verificamos que existan todos lo insumos, pero no se valida cantidad ya que no influye en su creacion dentro del sistema
            //Se debera verificar la cantidad disponible de stock al momento de facturar
            await ExistSuppliesNecessaries(dto.supplies);

            var product = _mapper.Map<Products>(dto);

            var success = await _productRepository.AddAsync(product);
            return success;
        }

        //Eliminar
        public async Task<bool> DeleteProductAsync(string codeProduct)
        {
            //Validamos nuevamente que no sea null o vacio
            if (string.IsNullOrWhiteSpace(codeProduct))
                throw new ProductException("El codigo del producto no puede ser nulo o vacio.");

            //Verificamos que exista el producto
            var exist = await _productRepository.GetProductByCodeAsync(codeProduct);
            if (exist == null)
                throw new ProductException("No existe un producto con este codigo.");

            if (!exist.IsActive)
                throw new ProductException("El producto ya se encuentra desactivado");

            //HAY QUE VERIFICAR QUE NO ESTE TOMADO PARA UNA FACTURA!!!!!!!!!
            var facturado = await _invoiceRepository.IsProductInto(codeProduct);
            if (facturado)
                throw new ProductException("El producto ya fue facturado, no es posible eliminarlo.");

            exist.MarkUpdated();

            //Si existe, lo eliminamos
            var d = await _productRepository.UpdateAsync(exist);

            if (d)
            {
                await mappeoDeMovimiento(codeProduct, 0, TypeProductMovement.Desactivacion);
            }
            return d;
        }

        //Editar
        public async Task<bool> UpdateImageProductAsync(UpdateImageProductDto dto)
        {
            if (dto == null)
                throw new ProductException("Los datos no pueden estar vacíos.");

            if (dto.Image == null || dto.Image.Length == 0)
                throw new ProductException("La imagen es obligatoria.");

            var product = await _productRepository.GetProductByCodeAsync(dto.codeProduct);
            if (product == null)
                throw new ProductException($"No se encontró el producto con código '{dto.codeProduct}'.");

            // Guardar nueva imagen
            var imageUrl = await _imageService.SaveImageAsync(dto.Image, "products");

            // Actualizar URL en el modelo
            product.imageUrl = imageUrl;

            var success = await _productRepository.UpdateAsync(product);
            if (success)
                await mappeoDeMovimiento(dto.codeProduct, 0, TypeProductMovement.Edicion);

            return success;
        }

        public async Task<bool> UpdateDescriptionProductAsync(UpdateDescriptionProductDto dto)
        {
            //Verificamos que no sea null
            if (dto == null) throw new ProductException("Datos incorrectos.");

            //Verificamos que exista el producto
            Products? exist = await _productRepository.GetProductByCodeAsync(dto.codeProduct);
            if (exist == null)
                throw new ProductException("No existe un producto con este codigo.");

            //Si existe, lo editamos
            exist.changeDescription(dto.nameProduct, dto.descriptionProduct);

            //Guardamos los cambios
            var updated = await _productRepository.UpdateAsync(exist);
            if (updated)
                await mappeoDeMovimiento(dto.codeProduct, 0, TypeProductMovement.Edicion);

            return updated;
        }


        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            IEnumerable<Products> products = await _productRepository.GetAllAsync();
            if (products == null || !products.Any())
                return new List<ProductDto>().AsEnumerable();
            //Mapeamos los productos a ProductDto
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            if (productDtos == null || !productDtos.Any())
                throw new ProductException("Error al mapear los productos.");

            //Retornamos la lista de productos
            return productDtos;
        }

        public Task<Products> GetProductByCode(string codeProduct)
        {
            if (string.IsNullOrWhiteSpace(codeProduct))
                throw new ProductException("Es necesario ingresar el codigo del producto.");

            var success = _productRepository.GetProductByCodeAsync(codeProduct);
            if (success == null)
                throw new ProductException($"No existe un producto con el codigo {codeProduct}");

            return success;
        }

        //Insumos que componen al producto
        public async Task<List<SupplyNecessaryDto>> GetSuppliesForProducts(string codeProduct)
        {
            if (string.IsNullOrWhiteSpace(codeProduct))
                throw new ProductException("Es necesario ingresar el codigo del producto.");

            //Verificamos la existencia del producto
            var product = await _productRepository.GetProductWithSupplies(codeProduct);
            if (product == null)
                throw new ProductException($"No existe un producto con el codigo {codeProduct}");

            return _mapper.Map<List<SupplyNecessaryDto>>(product.ProductoInsumos);
        }

        //Movimiento
        public async Task<IEnumerable<ProductMovementDto>> GetAllProductMovements()
        {
            var movements = await _productRepository.GetAllStockMovements();
            return _mapper.Map<IEnumerable<ProductMovementDto>>(movements);
        }









        //AUX
        private async Task ExistSuppliesNecessaries(List<SupplyNecessaryDto> supplies)
        {
            var codes = supplies.Select(s => s.codeSupply).ToList();
            var existentes = await _supplyRepository.GetExistingCodesAsync(codes);

            var faltantes = codes.Except(existentes).ToList();
            if (faltantes.Any())
                throw new SupplyException($"Los siguientes insumos no existen: {string.Join(", ", faltantes)}");
        }

        private async Task registerMovement(ProductMovementDto dto, string userName)
        {
            if (dto == null) throw new ProductException("Datos incorrectos.");
            if (string.IsNullOrWhiteSpace(userName)) throw new ProductException("No se pudo identificar el usuario que realiza el movimiento.");


            ProductMovement nuevoMovimientoStock = new ProductMovement
            {
                CodeProduct = dto.CodeProduct,
                MovementType = dto.MovementType,
                Quantity = dto.QuantityProduced,
                UserName = userName

            };

            // Guardar movimiento
            await _productRepository.AddStockMovementAsync(nuevoMovimientoStock);
        }



        public async Task mappeoDeMovimiento(string codeProduct, int quantity, TypeProductMovement tipo)
        {
            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema";

            var movimentDto = new ProductMovementDto
            {
                CodeProduct = codeProduct,
                MovementType = tipo,
                QuantityProduced = quantity,
                MovementDate = DateTime.Now
            };

            await registerMovement(movimentDto, user);
        }

        
    }
}
