namespace VoyagerTravelBlog.Domain.Entities
{
    public class Media
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Url { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        // Relationships
        public required Post Post { get; set; }
    }
}
