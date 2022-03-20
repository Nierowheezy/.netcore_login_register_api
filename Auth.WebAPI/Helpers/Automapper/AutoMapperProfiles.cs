using Auth.WebAPI.Core.DTOS;
using Auth.WebAPI.DB;
using AutoMapper;

namespace Auth.WebAPI.Helpers.Automapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<SignInUserDTO, User>().ReverseMap();
            CreateMap<SignUpUserDTO, User>().ReverseMap();
        }
    }
}
