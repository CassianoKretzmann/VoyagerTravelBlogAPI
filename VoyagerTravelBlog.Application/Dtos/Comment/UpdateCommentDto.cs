namespace VoyagerTravelBlog.Application.Dtos.Comment
{
    public class UpdateCommentDto
    {
        public required int Id { get; set; }

        public required string Content { get; set; }
    }
}
