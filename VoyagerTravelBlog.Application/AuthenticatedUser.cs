using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace VoyagerTravelBlog.Application
{
    public class AuthenticatedUser(IHttpContextAccessor accessor)
    {
        private readonly IHttpContextAccessor _accessor = accessor;

        public int Id => int.TryParse(GetClaimsIdentity().FirstOrDefault(a => a.Type == "Myapp_User_Id")?.Value, out var id) ? id : 0;

        public string Email => GetClaimsIdentity().FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value;

        public string UserName => GetClaimsIdentity().FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }
    }
}
