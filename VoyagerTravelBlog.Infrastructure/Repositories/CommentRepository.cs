using VoyagerTravelBlog.Application.Interfaces.Repositories;
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Repositories
{
    public class CommentRepository(BlogDbContext context) : BaseRepository<Comment>(context), ICommentRepository
    {
    }
}
