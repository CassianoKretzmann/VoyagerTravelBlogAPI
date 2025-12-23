namespace VoyagerTravelBlog.Application.Dtos.User
{
    public class CreateUserDto
    {
        public required string Name { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
