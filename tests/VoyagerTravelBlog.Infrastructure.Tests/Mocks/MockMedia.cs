using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Tests.Mocks;

public static class MockMedia
{
    public static Media CreateMedia()
    {
        return new Media
        {
            Id = 1,
            PostId = 1,
            Url = "http://example.com",
            CreatedAt = DateTimeOffset.Now,
            Post = MockPost.CreatePost()
        };
    }
}