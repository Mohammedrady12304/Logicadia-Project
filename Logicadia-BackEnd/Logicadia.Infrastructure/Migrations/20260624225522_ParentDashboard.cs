using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicadia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ParentDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "UserProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "UserProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "UserProgress",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "UserAchievements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_ChildId",
                table: "UserProgress",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_LevelId",
                table: "UserProgress",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_StoryId",
                table: "UserProgress",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_ChildId",
                table: "UserAchievements",
                column: "ChildId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAchievements_Children_ChildId",
                table: "UserAchievements",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Children_ChildId",
                table: "UserProgress",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Levels_LevelId",
                table: "UserProgress",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Stories_StoryId",
                table: "UserProgress",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAchievements_Children_ChildId",
                table: "UserAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Children_ChildId",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Levels_LevelId",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Stories_StoryId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserProgress_ChildId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserProgress_LevelId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserProgress_StoryId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserAchievements_ChildId",
                table: "UserAchievements");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "UserProgress");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "UserProgress");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "UserProgress");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "UserAchievements");
        }
    }
}
