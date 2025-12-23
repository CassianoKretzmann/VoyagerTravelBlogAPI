namespace VoyagerTravelBlog.Application.Dtos.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }

        public required int PostId { get; set; }

        public required int UserId { get; set; }

        public required string UserName { get; set; }

        public required string Content { get; set; }

        public required DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
