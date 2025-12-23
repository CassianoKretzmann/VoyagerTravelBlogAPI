namespace VoyagerTravelBlog.Application.Dtos.Auth
{
    public class ValidateUserResponseDto
    {
        public bool Success { get; set; }

        public string Errors { get; set; }

        public string AccessToken { get; set; }

        public int AccessTokenExpirationMinutes { get; set; }

        public string RefreshToken { get; set; }

        public DateTimeOffset RefreshTokenExpirationDate { get; set; }
    }
}
