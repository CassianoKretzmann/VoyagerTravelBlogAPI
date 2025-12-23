using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Repositories
{
    public class MediaRepository(BlogDbContext context) : BaseRepository<Media>(context), IMediaRepository
    {
    }
}
