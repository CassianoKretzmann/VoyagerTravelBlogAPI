using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Auth;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Application.Mappings
{
    public class RefreshTokenMapping : Profile
    {
        public RefreshTokenMapping()
        {
            _ = CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
        }
    }
}
