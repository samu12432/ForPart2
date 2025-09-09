using ForParts.DTO.Product;
using AutoMapper;
using ForParts.Models.Product;
using ForParts.DTOs.Product;

namespace ForParts.Mappers.Product
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Models.Product.Product, ProductDto>().ReverseMap();
            CreateMap<SupplyNecessary, SupplyNecessaryDto>().ReverseMap();

            CreateMap<Models.Product.Product, ProductWebDto>()
                .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.productName))
                .ForMember(dest => dest.productDescription, opt => opt.MapFrom(src => src.productDescription))
                .ForMember(dest => dest.productCategory, opt => opt.MapFrom(src => src.productCategory))
                .ForMember(dest => dest.productPrice, opt => opt.MapFrom(src => src.productPrice))
                .ForMember(dest => dest.imageUrl, opt => opt.MapFrom(src => src.imageUrl))
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}
