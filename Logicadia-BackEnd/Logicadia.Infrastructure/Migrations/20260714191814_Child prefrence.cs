using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicadia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Childprefrence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteAnimal",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FavoriteColor",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LearningTopic",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredLanguage",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReadingLevel",
                table: "Children",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteAnimal",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "FavoriteColor",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "LearningTopic",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "PreferredLanguage",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "ReadingLevel",
                table: "Children");
        }
    }
}
