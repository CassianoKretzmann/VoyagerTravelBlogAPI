using FluentAssertions;
using VoyagerTravelBlog.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace VoyagerTravelBlog.Infrastructure.Tests
{
    public class DependencyInjectionTest
    {
        [Fact]
        public void AddPersistence_ShouldRegisterDbContext()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDatabase;Trusted_Connection=True;";

            // Act
            services.AddPersistence(connectionString);
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<BlogDbContext>();

            // Assert
            dbContext.Should().NotBeNull();
        }

        [Fact]
        public void AddPersistence_ShouldRegisterRepositories()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDatabase;Trusted_Connection=True;";

            // Act
            services.AddPersistence(connectionString);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<IMediaRepository>().Should().NotBeNull();
            serviceProvider.GetService<IPostRepository>().Should().NotBeNull();
            serviceProvider.GetService<IUserRepository>().Should().NotBeNull();
            serviceProvider.GetService<ICommentRepository>().Should().NotBeNull();
            serviceProvider.GetService<IRefreshTokenRepository>().Should().NotBeNull();
        }

        [Fact]
        public void AddPersistence_ShouldUseSqlServer()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDatabase;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();

            // Act
            services.AddPersistence(connectionString);
            var serviceProvider = services.BuildServiceProvider();
            var dbContextOptions = serviceProvider.GetService<DbContextOptions<BlogDbContext>>();

            // Assert
            dbContextOptions.Should().NotBeNull();
            dbContextOptions.GetExtension<SqlServerOptionsExtension>().Should().NotBeNull();
            dbContextOptions.GetExtension<SqlServerOptionsExtension>().ConnectionString.Should().Be(connectionString);
        }
    }
}