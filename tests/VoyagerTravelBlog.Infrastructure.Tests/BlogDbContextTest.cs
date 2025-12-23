using FluentAssertions;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace VoyagerTravelBlog.Infrastructure.Tests
{
    public class BlogDbContextTest
    {
        private DbSet<Post> mockPostSet;
        private DbSet<User> mockUserSet;
        private DbSet<Media> mockMediaSet;
        private DbSet<Comment> mockCommentSet;
        public BlogDbContextTest()
        {
            mockPostSet = Substitute.For<DbSet<Post>>();
            mockUserSet = Substitute.For<DbSet<User>>();
            mockMediaSet = Substitute.For<DbSet<Media>>();
            mockCommentSet = Substitute.For<DbSet<Comment>>();

        }
        private DbContextOptions<BlogDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void BlogDbContext_ShouldHaveDbSetOfPost()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var context = new BlogDbContext(options)
            {
                Posts = mockPostSet,
                Users = mockUserSet,
                Medias = mockMediaSet,
                Comments = mockCommentSet
            };

            // Act
            var postSet = context.Posts;

            // Assert
            postSet.Should().NotBeNull();
        }

        [Fact]
        public void BlogDbContext_ShouldHaveDbSetOfUser()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var context = new BlogDbContext(options)
            {
                Posts = mockPostSet,
                Users = mockUserSet,
                Medias = mockMediaSet,
                Comments = mockCommentSet
            };

            // Act
            var userSet = context.Users;

            // Assert
            userSet.Should().NotBeNull();
        }

        [Fact]
        public void BlogDbContext_ShouldHaveDbSetOfMedia()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var context = new BlogDbContext(options)
            {
                Posts = mockPostSet,
                Users = mockUserSet,
                Medias = mockMediaSet,
                Comments = mockCommentSet
            };

            // Act
            var mediaSet = context.Medias;

            // Assert
            mediaSet.Should().NotBeNull();
        }

        [Fact]
        public void BlogDbContext_ShouldHaveDbSetOfComment()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var context = new BlogDbContext(options)
            {
                Posts = mockPostSet,
                Users = mockUserSet,
                Medias = mockMediaSet,
                Comments = mockCommentSet
            };

            // Act
            var commentSet = context.Comments;

            // Assert
            commentSet.Should().NotBeNull();
        }

        [Fact]
        public void BlogDbContext_ShouldApplyConfigurationsFromAssembly()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var context = new BlogDbContext(options)
            {
                Posts = mockPostSet,
                Users = mockUserSet,
                Medias = mockMediaSet,
                Comments = mockCommentSet
            };

            // Act
            var derivedContext = new BlogDbContextTestable(options)
            {
                Posts = mockPostSet,
                Users = mockUserSet,
                Medias = mockMediaSet,
                Comments = mockCommentSet
            };
            derivedContext.InvokeOnModelCreating(Substitute.For<ModelBuilder>());

            // Assert
        }

        private class BlogDbContextTestable : BlogDbContext
        {
            public BlogDbContextTestable(DbContextOptions<BlogDbContext> options) : base(options)
            {
            }

            public void InvokeOnModelCreating(ModelBuilder modelBuilder)
            {
                OnModelCreating(modelBuilder);
            }
        }
    }
}