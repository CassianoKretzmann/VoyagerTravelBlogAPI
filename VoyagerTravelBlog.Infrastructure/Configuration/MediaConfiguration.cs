using VoyagerTravelBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoyagerTravelBlog.Infrastructure.Configuration
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable(nameof(Media));

            builder.HasKey(x => x.Id);
            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(p => p.Url)
                .IsRequired();

            builder
                .HasOne(p => p.Post)
                .WithMany(p => p.Medias);
        }
    }
}
