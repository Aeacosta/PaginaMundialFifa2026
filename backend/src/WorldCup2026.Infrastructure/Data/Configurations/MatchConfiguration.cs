using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Match entity
/// </summary>
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.MatchDate)
            .IsRequired();

        builder.Property(m => m.Phase)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(m => m.Round)
            .IsRequired(false);

        builder.Property(m => m.Status)
            .IsRequired()
            .HasConversion<string>();

        // Indexes
        builder.HasIndex(m => m.MatchDate);

        builder.HasIndex(m => m.Phase);

        builder.HasIndex(m => m.Status);

        builder.HasIndex(m => m.GroupId);

        builder.HasIndex(m => new { m.HomeTeamId, m.AwayTeamId });

        // Relationships
        builder.HasOne(m => m.Group)
            .WithMany(g => g.Matches)
            .HasForeignKey(m => m.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(m => m.Result)
            .WithOne(mr => mr.Match)
            .HasForeignKey<MatchResult>(mr => mr.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        // HomeTeam and AwayTeam relationships are configured in TeamConfiguration
        // Stadium relationship is configured in StadiumConfiguration
    }
}

// Made with Bob