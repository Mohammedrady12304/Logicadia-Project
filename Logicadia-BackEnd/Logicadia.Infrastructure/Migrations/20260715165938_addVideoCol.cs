using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicadia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addVideoCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Stories");
        }
    }
}
