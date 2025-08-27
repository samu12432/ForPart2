using AutoMapper;
using ForParts.DTOs.Supply;
using StockMovement = ForParts.Models.Supply.StockMovement;

namespace ForParts.Mappers.Supply
{
    public class StockMovementMapper : Profile
    {
        public StockMovementMapper()
        {
            CreateMap<StockMovementDto, StockMovement>()
               .ForMember(dest => dest.MovementDate, opt => opt.Ignore())
               .ForMember(dest => dest.UserName, opt => opt.Ignore());
        }
    }
}
