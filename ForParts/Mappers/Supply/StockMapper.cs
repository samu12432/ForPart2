using ForParts.DTOs.Supply;
using Stock = ForParts.Models.Supply.Stock;
using AutoMapper;

namespace ForParts.Mappers.Supply
{
    public class StockMapper : Profile
    {
        public StockMapper()
        {
            CreateMap<StockDto, Stock>()
            .ForMember(dest => dest.stockCreate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.stockUpdate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Stock, StockDto>();

        }
    }
}
