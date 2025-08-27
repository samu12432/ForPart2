using ForParts.DTOs.Supply;
using ForParts.Exceptions.Supply;
using ForParts.IRepository.Supply;
using ForParts.Models.Supply;
using AutoMapper;
using ForParts.IServices.Image;
using ForParts.IRepository.Invoice;
using ForParts.Repositorys.Supply;

namespace ForParts.Services.Supply
{
    public class GlassService : SupplyServiceBase<GlassDto, Glass>
    {
        public GlassService(
            ISupplyRepository<Glass> repository,
            IStockRepository stockRepository,
            IInvoiceRepository invoiceRepository,
            IImageService imageService,
            IMapper mapper)
        : base(repository, stockRepository, invoiceRepository, imageService, mapper) { }

        protected override SupplyException CreateAlreadyExistsException()
            => new GlassException("Ya existe un vidrio con este código.");
    }
}
