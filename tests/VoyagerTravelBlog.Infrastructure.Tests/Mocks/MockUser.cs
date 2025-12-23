
using VoyagerTravelBlog.Domain.Entities;

namespace VoyagerTravelBlog.Infrastructure.Tests.Mocks;

public static class MockUser
{
    public static User CreateUser()
    {
        return new User
        {
            Id = 1,
            Name = "Sample User",
            Username = "sampleuser",
            Email = "email@test.com",
            Password = "password123"
        };
    }

    public static List<User> CreateUsers()
    {
        return new List<User>
        {
            new User
                {
                    Id = 1,
                    Name = "Sample User",
                    Username = "sampleuser",
                    Email = "email@test.com",
                    Password = "password123"
                },
                new User
                {
                    Id = 2,
                    Name = "Sample User 2",
                    Username = "sampleuser2",
                    Email = "email2@test.com",
                    Password = "password123"
                }
        };
    }
}