namespace VoyagerTravelBlog.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Title { get; set; }

        public required string Content { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public DateTimeOffset PublishedAt { get; set; }

        // Relationships
        public required User User { get; set; }
        public List<Media>? Medias { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
