using System.ComponentModel.DataAnnotations;

namespace VoyagerTravelBlog.Application.Dtos.Auth
{
    public class LoginDto
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}