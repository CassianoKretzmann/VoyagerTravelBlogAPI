using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Repositories
{
    public class RefreshTokenRepository(BlogDbContext context) : BaseRepository<RefreshToken>(context), IRefreshTokenRepository
    {
    }
}
