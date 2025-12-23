using AutoMapper;
using VoyagerTravelBlog.Application.Dtos.Comment;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Application.Mappings
{
    public class CommentMapping : Profile
    {
        public CommentMapping() 
        {
            _ = CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();

            _ = CreateMap<Comment, CreateCommentDto>().ReverseMap();
        }
    }
}
