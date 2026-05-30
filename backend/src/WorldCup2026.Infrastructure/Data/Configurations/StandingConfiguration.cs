using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Standing entity
/// </summary>
public class StandingConfiguration : IEntityTypeConfiguration<Standing>
{
    public void Configure(EntityTypeBuilder<Standing> builder)
    {
        builder.ToTable("Standings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Played)
            .IsRequired();

        builder.Property(s => s.Won)
            .IsRequired();

        builder.Property(s => s.Drawn)
            .IsRequired();

        builder.Property(s => s.Lost)
            .IsRequired();

        builder.Property(s => s.GoalsFor)
            .IsRequired();

        builder.Property(s => s.GoalsAgainst)
            .IsRequired();

        builder.Property(s => s.GoalDifference)
            .IsRequired();

        builder.Property(s => s.Points)
            .IsRequired();

        builder.Property(s => s.Position)
            .IsRequired();

        // Indexes
        builder.HasIndex(s => s.TeamId)
            .IsUnique();

        builder.HasIndex(s => s.GroupId);

        builder.HasIndex(s => new { s.GroupId, s.Position });

        builder.HasIndex(s => new { s.GroupId, s.Points });

        // Relationships
        builder.HasOne(s => s.Group)
            .WithMany(g => g.Standings)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Team relationship is configured in TeamConfiguration
    }
}

// Made with Bob