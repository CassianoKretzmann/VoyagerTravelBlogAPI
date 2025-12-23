namespace VoyagerTravelBlog.Application.Dtos.Auth
{
    public class RefreshTokenDto
    {
        public int UserId { get; set; }

        public required string Token { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }
    }
}
