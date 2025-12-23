
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Tests.Mocks;

public static class MockComment
{
    public static Comment CreateComment()
    {
        return new Comment
        {
            Id = 1,
            UserId = 1,
            User = MockUser.CreateUser(),
            PostId = 1,
            Post = MockPost.CreatePost(),
            Content = "Sample Content",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public static List<Comment> CreateComments()
    {
        return new List<Comment>
        {
            new Comment
                {
                    Id = 1,
                    UserId = 1,
                    User = MockUser.CreateUser(),
                    PostId = 1,
                    Post = MockPost.CreatePost(),
                    Content = "Sample Content",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new Comment
                {
                    Id = 2,
                    UserId = 1,
                    User = MockUser.CreateUser(),
                    PostId = 1,
                    Post = MockPost.CreatePost(),
                    Content = "Sample Content 2",
                    CreatedAt = DateTimeOffset.UtcNow
                }
        };
    }
}