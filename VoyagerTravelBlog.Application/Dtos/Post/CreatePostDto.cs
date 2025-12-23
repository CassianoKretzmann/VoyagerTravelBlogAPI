using VoyagerTravelBlog.Application.Dtos.Media;

namespace VoyagerTravelBlog.Application.Dtos.Post
{
    public class CreatePostDto
    {
        public int UserId { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public List<MediaDto>? Medias { get; set; }
    }
}
