using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.IServices.Supply;
using SupplyAlias = ForParts.Models.Supply.Supply;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ForParts.DTOs.Supply;
using ForParts.IServices.Image;
using ForParts.IDto;
using ForParts.IRepository.Invoice;
using ForParts.Exceptions.Invoice;

namespace ForParts.Services.Supply
{
    public abstract class SupplyServiceBase<TDto, TEntity> : ISupplyService<TDto>
    where TDto : class, IImageDto
    where TEntity : SupplyAlias
    {
        protected readonly ISupplyRepository<TEntity> _repository;
        protected readonly IStockRepository _stockRepository;
        protected readonly IInvoiceRepository _invoiceRepository;
        protected readonly IImageService _imageService;
        protected readonly IMapper _mapper;

        protected SupplyServiceBase(ISupplyRepository<TEntity> repository, IStockRepository stockRepository,
            IInvoiceRepository invoiceRepository ,IImageService imageService ,IMapper mapper)
        {
            _repository = repository;
            _stockRepository = stockRepository;
            _invoiceRepository = invoiceRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<bool> AddSupplyAsync(TDto dto)
        {
            if (dto == null)
                throw new SupplyException("Los datos están vacíos.");

            if (dto.Image != null)
            {
                dto.imageUrl = await _imageService.SaveImageAsync(dto.Image, "supplies");
            }

            var entity = _mapper.Map<TEntity>(dto);

            var exists = await _repository.ExistSupplyByCode(entity.codeSupply);
            if (exists)
                throw new SupplyException($"Ya existe un suministro con el código '{entity.codeSupply}'.");

            return await _repository.AddAsync(entity);
        }

        public async Task<bool> DeleteSupplyAsync(string codeSupply)
        {
            if(codeSupply.IsNullOrEmpty()) throw new SupplyException("El código del suministro no puede estar vacío.");

            // Verificamos si existe
            var exists = await _repository.GetSupplyByCode(codeSupply);
            if (exists == null)
                throw new SupplyException($"No existe un insumo con codigo {codeSupply}");
             
            //No podemos eliminar un insumo si tiene stock registrado.
            var conStock = await _stockRepository.GetStockBySku(codeSupply);
            if (conStock)
                throw new StockException($"No se puede eliminar el insumo con el SKU {codeSupply}, porque tiene stock registrado.");

            //Tiene vinculo con facturacion
            var enFactura = await _invoiceRepository.ExistInInvoice(codeSupply);
            if(enFactura)
                throw new InvoiceException($"No se puede eliminar el insumo con el SKU {codeSupply}, porque tiene registro en una factura.");

            exists.isEnabledSupply = false;

            var deleted = await _repository.UpdateSupply(exists);

            return deleted; 
        }

        public async Task<bool> updateImageSupply(EditImageSupplyDto dto)
        {
            if (dto == null)
                throw new SupplyException("Los datos no pueden estar vacíos.");

            if (dto.Image == null || dto.Image.Length == 0)
                throw new SupplyException("La imagen es obligatoria.");

            var supply = await _repository.GetSupplyByCode(dto.codeSupply);
            if (supply == null)
                throw new SupplyException($"No se encontró el suministro con código '{dto.codeSupply}'.");

            // Guardar nueva imagen
            var imageUrl = await _imageService.SaveImageAsync(dto.Image, "supplies");

            // Actualizar URL en el modelo
            supply.imageUrl = imageUrl;

            return await _repository.UpdateSupply(supply);
        }

        public async Task<bool> updatePriceSupply(EditPriceSupplyDto dto)
        {
            if (dto == null)
                throw new SupplyException("Los datos no pueden estar vacíos.");

            if (dto.newPriceSupply <= 0)
                throw new SupplyException("El precio debe ser mayor que cero.");

            var supply = await _repository.GetSupplyByCode(dto.codeSupply);
            if (supply == null)
                throw new SupplyException($"No se encontró el suministro con código '{dto.codeSupply}'.");

            supply.priceSupply = dto.newPriceSupply;

            return await _repository.UpdateSupply(supply);
        }

        public async Task<bool> updateSupply(EditSupplyDto dto)
        {
            if (dto == null) throw new SupplyException("No puede estar vacío.");

            // Aquí se implementaría la lógica para editar el suministro por su código.
            var existing = await _repository.GetSupplyByCode(dto.codeSupply);
            if (existing == null) throw CreateAlreadyExistsException();

            existing.descriptionSupply = dto.description;

            return await _repository.UpdateSupply(existing); 
        }

        protected abstract SupplyException CreateAlreadyExistsException();
    }
}
