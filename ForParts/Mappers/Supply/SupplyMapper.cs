using AutoMapper;
using ForParts.DTOs.Supply;
using ProfileAlias = ForParts.Models.Supply.Profile;
using GlassAlias = ForParts.Models.Supply.Glass;
using AccessoryAlias = ForParts.Models.Supply.Accessory;

namespace ForParts.Mappers.Supply
{
    public class SupplyMapper : Profile
    {
        public SupplyMapper()
        {        // De CreateProfileDto → Profile
            CreateMap<ProfileDto, ProfileAlias>()
                .ForMember(dest => dest.imageUrl, opt => opt.MapFrom(src => src.imageUrl));

            // Si querés también el reverso
            CreateMap<ProfileAlias, ProfileDto>();


            // De CreateGlassDto → Glass
            CreateMap<GlassDto, GlassAlias>()
                .ForMember(dest => dest.imageUrl, opt => opt.MapFrom(src => src.imageUrl));

            // Si querés también el reverso
            CreateMap<GlassAlias, GlassDto>();

            // De CreateAccessoryDto → Accessory
            CreateMap<AccessoryDto, AccessoryAlias>()
                .ForMember(dest => dest.imageUrl, opt => opt.MapFrom(src => src.imageUrl));

            // Si querés también el reverso
            CreateMap<AccessoryAlias, AccessoryDto>();
        }
    }
}
