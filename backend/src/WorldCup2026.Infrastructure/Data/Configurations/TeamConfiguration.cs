using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Team entity
/// </summary>
public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(t => t.FlagUrl)
            .HasMaxLength(500);

        builder.Property(t => t.Confederation)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.FifaRanking)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(t => t.Code)
            .IsUnique();

        builder.HasIndex(t => t.Name);

        builder.HasIndex(t => t.GroupId);

        // Relationships
        builder.HasOne(t => t.Group)
            .WithMany(g => g.Teams)
            .HasForeignKey(t => t.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.HomeMatches)
            .WithOne(m => m.HomeTeam)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.AwayMatches)
            .WithOne(m => m.AwayTeam)
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Standing)
            .WithOne(s => s.Team)
            .HasForeignKey<Standing>(s => s.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.WonMatches)
            .WithOne(mr => mr.WinnerTeam)
            .HasForeignKey(mr => mr.WinnerTeamId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

// Made with Bob