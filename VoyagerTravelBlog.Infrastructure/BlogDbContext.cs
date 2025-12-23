using VoyagerTravelBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VoyagerTravelBlog.Infrastructure;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Post> Posts { get; set; }
    public required DbSet<Media> Medias { get; set; }
    public required DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
    }
}