using VoyagerTravelBlog.Application.Dtos.Media;

namespace VoyagerTravelBlog.Application.Dtos.Post
{
    public class UpdatePostDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public List<MediaDto>? Medias { get; set; }
    }
}
