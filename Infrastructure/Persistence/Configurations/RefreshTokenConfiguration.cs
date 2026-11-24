using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration
        : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property<uint>("xmin")
                .IsRowVersion();

            builder.Property(rt => rt.TokenHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(rt => rt.TokenHash)
                .IsUnique();

            builder.HasIndex(rt => rt.UserId);

            builder.HasIndex(rt => rt.FamilyId);

            builder.Property(rt => rt.CreatedAtUtc)
                .IsRequired();

            builder.Property(rt => rt.ExpiresAtUtc)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}