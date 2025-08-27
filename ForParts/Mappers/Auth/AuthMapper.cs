using ForParts.DTOs.Auth;
using ForParts.Models.Auth;
using AutoMapper;

namespace ForParts.Mappers.Auth
{
    public class AuthMapper : Profile
    {
        public AuthMapper() 
        {
            CreateMap<User, RegisterDto>().ReverseMap(); //Pasa de Dto a User y al reves
        }
    }
}
