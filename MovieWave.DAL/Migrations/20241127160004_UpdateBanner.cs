using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieWave.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_MediaItems_MediaItemId",
                table: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Banners_MediaItemId",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "MediaItemId",
                table: "Banners");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Banners",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Banners",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Banners");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Banners",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaItemId",
                table: "Banners",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banners_MediaItemId",
                table: "Banners",
                column: "MediaItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_MediaItems_MediaItemId",
                table: "Banners",
                column: "MediaItemId",
                principalTable: "MediaItems",
                principalColumn: "Id");
        }
    }
}
