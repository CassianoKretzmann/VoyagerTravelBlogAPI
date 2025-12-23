namespace VoyagerTravelBlog.Application.Options
{
    public class JWTSettingsOptions
    {
        public const string JwtSettings = "JwtSettings";

        public int AccessTokenExpirationMinutes { get; set; }
        public string Audience { get; set; } = String.Empty;
        public string Issuer { get; set; } = String.Empty;
        public int RefreshTokenExpirationDays { get; set; }
        public string SecretKey { get; set; } = String.Empty;
    }
}
