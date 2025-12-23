namespace VoyagerTravelBlog.Application.Dtos.Media
{
    public class MediaDto
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public required string Url { get; set; }
    }
}
