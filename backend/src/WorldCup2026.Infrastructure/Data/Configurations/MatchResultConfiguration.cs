using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for MatchResult entity
/// </summary>
public class MatchResultConfiguration : IEntityTypeConfiguration<MatchResult>
{
    public void Configure(EntityTypeBuilder<MatchResult> builder)
    {
        builder.ToTable("MatchResults");

        builder.HasKey(mr => mr.Id);

        builder.Property(mr => mr.HomeTeamScore)
            .IsRequired();

        builder.Property(mr => mr.AwayTeamScore)
            .IsRequired();

        builder.Property(mr => mr.HomeTeamPenalties)
            .IsRequired(false);

        builder.Property(mr => mr.AwayTeamPenalties)
            .IsRequired(false);

        builder.Property(mr => mr.WinnerTeamId)
            .IsRequired(false);

        builder.Property(mr => mr.Highlights)
            .IsRequired(false)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(mr => mr.MatchId)
            .IsUnique();

        builder.HasIndex(mr => mr.WinnerTeamId);

        // Relationships
        // Match relationship is configured in MatchConfiguration
        // WinnerTeam relationship is configured in TeamConfiguration
    }
}

// Made with Bob