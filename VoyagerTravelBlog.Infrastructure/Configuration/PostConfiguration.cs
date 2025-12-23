using VoyagerTravelBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoyagerTravelBlog.Infrastructure.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable(nameof(Post));

            builder.HasKey(x => x.Id);
            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(p => p.Title)
                .HasMaxLength(150)
                .IsRequired();

            builder
                .Property(p => p.Content)
                .HasColumnType("nvarchar(Max)")
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .HasOne(p => p.User)
                .WithMany()
                .IsRequired();
        }
    }
}
