using ForParts.DTOs.Supply;
using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.Models.Supply;
using AutoMapper;
using ForParts.IServices.Image;
using ForParts.IRepository.Invoice;

namespace ForParts.Services.Supply
{
    public class AccessoryService : SupplyServiceBase<AccessoryDto, Accessory>
    {
        public AccessoryService(
            ISupplyRepository<Accessory> repository,
            IStockRepository stockRepository,
            IInvoiceRepository invoiceRepository,
            IImageService imageService,
            IMapper mapper)
        : base(repository, stockRepository, invoiceRepository, imageService, mapper) { }

        protected override SupplyException CreateAlreadyExistsException()
            => new AccessoryException("Ya existe un accesorio con este código.");
    }

}
