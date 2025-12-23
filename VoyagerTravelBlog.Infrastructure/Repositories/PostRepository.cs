using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Repositories
{
    public class PostRepository(BlogDbContext context) : BaseRepository<Post>(context), IPostRepository
    {
    }
}
