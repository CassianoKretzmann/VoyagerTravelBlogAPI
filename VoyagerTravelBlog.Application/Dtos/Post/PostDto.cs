using VoyagerTravelBlog.Application.Dtos.Media;

namespace VoyagerTravelBlog.Application.Dtos.Post
{
    public class PostDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset PublishedAt { get; set; }

        public required string UserName { get; set; }

        public List<MediaDto>? Medias { get; set; }
    }
}
