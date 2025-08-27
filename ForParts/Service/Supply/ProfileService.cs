using ForParts.DTOs.Supply;
using ForParts.IServices.Supply;
using ForParts.Models.Supply;
using ProfileAlias = ForParts.Models.Supply.Profile;
using AutoMapper;
using ForParts.IRepository.Supply;
using ForParts.Exceptions.Supply;
using ForParts.IServices.Image;
using ForParts.IRepository.Invoice;

namespace ForParts.Services.Supply
{
    public class ProfileService : SupplyServiceBase<ProfileDto, ProfileAlias>
    {
        public ProfileService(
            ISupplyRepository<ProfileAlias> repository,
            IStockRepository stockRepository,
            IInvoiceRepository invoiceRepository,
            IImageService imageService,
            IMapper mapper)
        : base(repository, stockRepository, invoiceRepository, imageService, mapper) { }

        protected override SupplyException CreateAlreadyExistsException()
            => new ProfileException("Ya existe un perfil con este código.");
    }
}
