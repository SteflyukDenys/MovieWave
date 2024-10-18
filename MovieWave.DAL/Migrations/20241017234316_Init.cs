using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieWave.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MediaItemTypes",
                newName: "MediaItemName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaItemName",
                table: "MediaItemTypes",
                newName: "Name");
        }
    }
}
