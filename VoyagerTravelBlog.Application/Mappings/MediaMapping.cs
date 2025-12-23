using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Media;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Application.Mappings
{
    public class MediaMapping : Profile
    {
        public MediaMapping()
        {
            _ = CreateMap<Media, MediaDto>().ReverseMap();

            _ = CreateMap<Media, CreateMediaDto>().ReverseMap();
        }
    }
}
