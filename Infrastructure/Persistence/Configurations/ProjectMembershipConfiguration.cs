using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProjectMembershipConfiguration : IEntityTypeConfiguration<ProjectMembership>
    {
        public void Configure(EntityTypeBuilder<ProjectMembership> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ProjectId).IsRequired();
            builder.Property(r => r.MemberId).IsRequired();

            builder.Property(r => r.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(r => r.JoinedAtUtc).IsRequired();

            builder.HasIndex(r => new { r.ProjectId, r.MemberId })
                .IsUnique();

            builder.HasOne<Member>()
                .WithMany()
                .HasForeignKey(r => r.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Project>()
                .WithMany()
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
