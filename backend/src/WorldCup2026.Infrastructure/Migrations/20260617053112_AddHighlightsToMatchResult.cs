using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCup2026.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHighlightsToMatchResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Highlights",
                table: "MatchResults",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Highlights",
                table: "MatchResults");
        }
    }
}
