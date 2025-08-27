using ForParts.DTO.Product;
using AutoMapper;
using ForParts.Models.Product;

namespace ForParts.Mappers.Product
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Models.Product.Product, ProductDto>().ReverseMap();
            CreateMap<SupplyNecessary, SupplyNecessaryDto>().ReverseMap();
        }
    }
}
