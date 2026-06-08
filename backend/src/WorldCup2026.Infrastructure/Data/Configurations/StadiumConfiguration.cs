using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldCup2026.Domain.Entities;

namespace WorldCup2026.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Stadium entity
/// </summary>
public class StadiumConfiguration : IEntityTypeConfiguration<Stadium>
{
    public void Configure(EntityTypeBuilder<Stadium> builder)
    {
        builder.ToTable("Stadiums");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Capacity)
            .IsRequired();

        builder.Property(s => s.ImageUrl)
            .HasMaxLength(500);

        builder.Property(s => s.FlagUrl)
            .HasMaxLength(500);

        builder.Property(s => s.Latitude)
            .HasPrecision(9, 6);

        builder.Property(s => s.Longitude)
            .HasPrecision(9, 6);

        // Indexes
        builder.HasIndex(s => s.Name);

        builder.HasIndex(s => s.City);

        builder.HasIndex(s => s.Country);

        // Relationships
        builder.HasMany(s => s.Matches)
            .WithOne(m => m.Stadium)
            .HasForeignKey(m => m.StadiumId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Made with Bob