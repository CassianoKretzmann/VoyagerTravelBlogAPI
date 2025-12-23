using FluentAssertions;
using VoyagerTravelBlog.Domain.Entities;
using VoyagerTravelBlog.Infrastructure.Repositories;
using VoyagerTravelBlog.Infrastructure.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace VoyagerTravelBlog.Infrastructure.Tests.Repositories;

public class BaseRepositoryTests
{
    private readonly DbContextOptions<BlogDbContext> options;
    private BlogDbContext mockContext;
    private DbSet<Post> mockPostSet;
    private DbSet<User> mockUserSet;
    private DbSet<Media> mockMediaSet;
    private DbSet<Comment> mockCommentSet;

    public BaseRepositoryTests()
    {

        mockPostSet = Substitute.For<DbSet<Post>>();
        mockUserSet = Substitute.For<DbSet<User>>();
        mockMediaSet = Substitute.For<DbSet<Media>>();
        mockCommentSet = Substitute.For<DbSet<Comment>>();

        options = new DbContextOptionsBuilder<BlogDbContext>()
             .UseInMemoryDatabase(databaseName: "TestDb")
             .Options;

        mockContext = Substitute.For<BlogDbContext>(options);
        mockContext.Posts = mockPostSet;
        mockContext.Users = mockUserSet;
        mockContext.Medias = mockMediaSet;
        mockContext.Comments = mockCommentSet;
    }

    [Fact]
    public async Task CreateAsync_WhenInvalidRecord_ShouldNotAddRecordAsync()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var post = MockPost.CreatePost();

        mockContext.Posts.Add(post);
        mockContext.SaveChangesAsync(default).Returns(Task.FromException<int>(new Exception()));

        // Act
        Func<Task> action = async () => await repository.CreateAsync(post);

        // Assert
        await action.Should().ThrowAsync<Exception>();
        await mockContext.Received(1).SaveChangesAsync(default);
    }


    [Fact]
    public async Task CreateAsync_WhenValidRecord_ShouldAddRecord()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var post = MockPost.CreatePost();

        mockContext.Posts.Add(post);
        mockContext.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        // Act
        var result = await repository.CreateAsync(post);

        // Assert
        await mockContext.Received(1).SaveChangesAsync(default);
        result.Should().Be(post);
    }

    [Fact]
    public async Task CreateListAsync_WhenInvalidRecords_ShouldNotAddRecordsAsync()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var posts = MockPost.CreatePosts();

        mockContext.Posts.AddRange(posts);
        mockContext.SaveChangesAsync(default).Returns(Task.FromException<int>(new Exception()));

        // Act
        Func<Task> action = async () => await repository.CreateListAsync(posts);

        // Assert
        await action.Should().ThrowAsync<Exception>();
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task CreateListAsync_WhenValidRecords_ShouldAddRecords()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var posts = MockPost.CreatePosts();

        mockContext.Posts.AddRange(posts);
        mockContext.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        // Act
        var result = await repository.CreateListAsync(posts);

        // Assert
        await mockContext.Received(1).SaveChangesAsync(default);
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async Task UpdateAsync_WhenInvalidRecord_ShouldNotUpdateRecordAsync()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var post = MockPost.CreatePost();

        mockContext.Posts.Update(post);
        mockContext.SaveChangesAsync(default).Returns(Task.FromException<int>(new Exception()));

        // Act
        Func<Task> action = async () => await repository.UpdateAsync(post);

        // Assert
        await action.Should().ThrowAsync<Exception>();
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task GetAsync_WhenInvalidRecord_ShouldNotGetRecord()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetAsync(x => x.Id == 4);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WhenValidRecord_ShouldGetRecord()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetAsync(x => x.Id == 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Post>();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Post 1");
        result.Content.Should().Be("Test Content 1");
    }

    [Fact]
    public async Task GetAsync_WhenValidRecordWithIncludeProperties_ShouldGetRecord()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetAsync(x => x.Id == 1, includeProperties: "User");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Post>();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Post 1");
        result.Content.Should().Be("Test Content 1");
        result.User.Should().NotBeNull();
        result.User.Should().BeOfType<User>();
        result.User.Id.Should().Be(1);
        result.User.Name.Should().Be("Sample User");
    }

    [Fact]
    public async Task GetListAsync_WhenInvalidRecords_ShouldNotGetRecords()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetListAsync(x => x.Id == 4);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetListAsync_WhenValidRecords_ShouldGetRecords()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetListAsync(x => x.UserId == 1);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        result.Should().BeOfType<List<Post>>();
    }

    [Fact]
    public async Task GetListAsync_WhenValidRecordsWithIncludeProperties_ShouldGetRecords()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetListAsync(x => x.UserId == 1, includeProperties: "User");

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        result.Should().BeOfType<List<Post>>();
    }

    [Fact]
    public async Task GetListAsync_WhenValidRecordsWithOrderBy_ShouldGetRecords()
    {
        // Arrange
        var posts = MockPost.CreatePosts().AsQueryable().BuildMockDbSet();
        mockContext.Set<Post>().Returns(posts);

        var repository = new BaseRepository<Post>(mockContext);

        // Act
        var result = await repository.GetListAsync(x => x.UserId == 1, orderBy: x => x.OrderByDescending(x => x.Id));

        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        result.Should().BeOfType<List<Post>>();
    }

    [Fact]
    public async Task UpdateAsync_WhenValidRecord_ShouldUpdateRecord()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var post = MockPost.CreatePost();

        mockContext.Posts.Update(post);
        mockContext.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        // Act
        await repository.UpdateAsync(post);

        // Assert
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task RemoveAsync_WhenInvalidRecord_ShouldNotRemoveRecordAsync()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var post = MockPost.CreatePost();

        mockContext.Posts.Remove(post);
        mockContext.SaveChangesAsync(default).Returns(Task.FromException<int>(new Exception()));

        // Act
        Func<Task> action = async () => await repository.RemoveAsync(post);

        // Assert
        await action.Should().ThrowAsync<Exception>();
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task RemoveAsync_WhenValidRecord_ShouldRemoveRecord()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var post = MockPost.CreatePost();

        mockContext.Posts.Remove(post);
        mockContext.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        // Act
        await repository.RemoveAsync(post);

        // Assert
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task RemoveListAsync_WhenInvalidRecords_ShouldNotRemoveRecordsAsync()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var posts = MockPost.CreatePosts();

        mockContext.Posts.RemoveRange(posts);
        mockContext.SaveChangesAsync(default).Returns(Task.FromException<int>(new Exception()));

        // Act
        Func<Task> action = async () => await repository.RemoveListAsync(posts);

        // Assert
        await action.Should().ThrowAsync<Exception>();
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task RemoveListAsync_WhenValidRecords_ShouldRemoveRecords()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);
        var posts = MockPost.CreatePosts();

        mockContext.Posts.RemoveRange(posts);
        mockContext.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        // Act
        await repository.RemoveListAsync(posts);

        // Assert
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task SaveAsync_WhenInvalidRecord_ShouldNotSaveRecordAsync()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);

        mockContext.SaveChangesAsync(default).Returns(Task.FromException<int>(new Exception()));

        // Act
        Func<Task> action = async () => await repository.SaveAsync();

        // Assert
        await action.Should().ThrowAsync<Exception>();
        await mockContext.Received(1).SaveChangesAsync(default);
    }

    [Fact]
    public async Task SaveAsync_WhenValidRecord_ShouldSaveRecord()
    {
        // Arrange
        var repository = new BaseRepository<Post>(mockContext);

        mockContext.SaveChangesAsync(default).ReturnsForAnyArgs(1);

        // Act
        await repository.SaveAsync();

        // Assert
        await mockContext.Received(1).SaveChangesAsync(default);
    }
}