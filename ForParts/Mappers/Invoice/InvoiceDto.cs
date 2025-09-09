using AutoMapper;
using ForParts.DTOs.FEU;
using ForParts.Models.Invoice;

namespace ForParts.Mappers.Invoice
{
    public class InvoiceDto : Profile
    {
        public InvoiceDto()
        {
            CreateMap<Models.Invoice.Invoice, FeuInvoiceDto>()
            .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => DateTime.Now.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.RutReceptor, opt => opt.MapFrom(src => src.Customer.Identificador))
            .ForMember(dest => dest.NombreReceptor, opt => opt.MapFrom(src => src.Customer.Nombre))
            .ForMember(dest => dest.DomicilioReceptor, opt => opt.MapFrom(src => src.Customer.DireccionFiscal))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Moneda, opt => opt.MapFrom(_ => "UYU"))
            .ForMember(dest => dest.FormaPago, opt => opt.MapFrom(_ => "Contado"));

            CreateMap<InvoiceItem, FeuItemDto>()
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Product.productDescription))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.Product.productPrice));
        }

    }
}
