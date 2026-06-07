using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarasKino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Movies_IndexOnImdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Movies_ImdbId",
                table: "Movies",
                column: "ImdbId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_ImdbId",
                table: "Movies");
        }
    }
}
