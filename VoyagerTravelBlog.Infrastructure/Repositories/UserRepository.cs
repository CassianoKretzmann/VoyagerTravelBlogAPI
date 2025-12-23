using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Repositories
{
    public class UserRepository(BlogDbContext context) : BaseRepository<User>(context), IUserRepository
    {
    }
}
