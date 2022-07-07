using AutoMapper;
using Task4Back.Entities;
using Task4Back.Models.Users;

namespace Task4Back.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            _ = CreateMap<User, AuthenticationResponse>();

            _ = CreateMap<User, UserModel>();

            _ = CreateMap<RegisterRequest, User>();
        }
    }
}