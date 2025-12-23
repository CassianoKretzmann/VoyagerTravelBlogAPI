using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Post;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Application.Mappings
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            _ = CreateMap<Post, PostDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();

            _ = CreateMap<Post, CreatePostDto>().ReverseMap();
        }
    }
}
