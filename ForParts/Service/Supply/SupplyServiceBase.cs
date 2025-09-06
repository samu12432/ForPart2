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

        public async Task<bool> DeleteByCodeAsync(string codeSupply)
        {
            return await DeleteSupplyAsync(codeSupply);
        }

        public async Task<bool> UpdateDescriptionAsync(EditSupplyDto dto)
        {
            if (dto == null) throw new SupplyException("No puede estar vacío.");

            var existing = await _repository.GetSupplyByCode(dto.codeSupply);
            if (existing == null) throw new SupplyException($"No se encontró el suministro con código '{dto.codeSupply}'.");

            existing.descriptionSupply = dto.description;

            return await _repository.UpdateSupply(existing); 
        }

        public async Task<bool> UpdateImageAsync(EditImageSupplyDto dto)
        {
            if (dto == null)
                throw new SupplyException("Los datos no pueden estar vacíos.");

            if (dto.Image == null || dto.Image.Length == 0)
                throw new SupplyException("La imagen es obligatoria.");

            var supply = await _repository.GetSupplyByCode(dto.codeSupply);
            if (supply == null)
                throw new SupplyException($"No se encontró el suministro con código '{dto.codeSupply}'.");

            var imageUrl = await _imageService.SaveImageAsync(dto.Image, "supplies");

            supply.imageUrl = imageUrl;

            return await _repository.UpdateSupply(supply);
        }

        public async Task<bool> UpdatePriceAsync(EditPriceSupplyDto dto)
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

        public async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        protected abstract SupplyException CreateAlreadyExistsException();
    }
}
