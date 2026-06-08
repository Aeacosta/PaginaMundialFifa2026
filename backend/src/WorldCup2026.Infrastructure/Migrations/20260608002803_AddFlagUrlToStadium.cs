using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldCup2026.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagUrlToStadium : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlagUrl",
                table: "Stadiums",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlagUrl",
                table: "Stadiums");
        }
    }
}
