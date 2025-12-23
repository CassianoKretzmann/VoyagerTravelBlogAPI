namespace VoyagerTravelBlog.Application.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
