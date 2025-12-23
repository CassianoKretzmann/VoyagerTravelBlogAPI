namespace VoyagerTravelBlog.Application.Dtos.Media
{
    public class CreateMediaDto
    {
        public int PostId { get; set; }

        public required string Url { get; set; }
    }
}
