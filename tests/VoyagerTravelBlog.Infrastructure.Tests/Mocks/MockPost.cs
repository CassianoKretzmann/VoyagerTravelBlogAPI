
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Tests.Mocks;

public static class MockPost
{
    public static Post CreatePost()
    {
        return new Post
        {
            Id = 1,
            UserId = 1,
            User = MockUser.CreateUser(),
            Title = "Test Post",
            Content = "Test Content",
            CreatedAt = DateTimeOffset.UtcNow,
            PublishedAt = DateTimeOffset.UtcNow
        };
    }

    public static List<Post> CreatePosts()
    {
        return new List<Post>
        {
            new Post
                {
                    Id = 1,
                    UserId = 1,
                    User = MockUser.CreateUser(),
                    Title = "Test Post 1",
                    Content = "Test Content 1",
                    CreatedAt = DateTimeOffset.UtcNow,
                    PublishedAt = DateTimeOffset.UtcNow
                },
                new Post
                {
                    Id = 2,
                    UserId = 1,
                    User = MockUser.CreateUser(),
                    Title = "Test Post 2",
                    Content = "Test Content 2",
                    CreatedAt = DateTimeOffset.UtcNow,
                    PublishedAt = DateTimeOffset.UtcNow
                },
                new Post
                {
                    Id = 3,
                    UserId = 1,
                    User = MockUser.CreateUser(),
                    Title = "Test Post 3",
                    Content = "Test Content 3",
                    CreatedAt = DateTimeOffset.UtcNow,
                    PublishedAt = DateTimeOffset.UtcNow
                }
        };
    }
}