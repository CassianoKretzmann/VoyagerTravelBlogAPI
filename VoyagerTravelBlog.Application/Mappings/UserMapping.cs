using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.User;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Application.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            _ = CreateMap<User, UserDto>().ReverseMap();

            _ = CreateMap<User, CreateUserDto>().ReverseMap();
        }
    }
}
