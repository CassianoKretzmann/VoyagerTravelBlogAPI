using Microsoft.EntityFrameworkCore;
using VoyagerTravelBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VoyagerTravelBlog.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasKey(p => p.Id);
            builder
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.Password)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
