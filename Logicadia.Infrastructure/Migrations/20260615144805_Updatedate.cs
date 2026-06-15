using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicadia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatedate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$qR7YVv.MhP2zM6uN6g6pDe8xP7lP2W8qZz8yY7fG4O/kS4rR9aO2G");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$RvXB4Srg3uWVLRcczSvZOefC0d38kiQ6XXum11mgZF9Edpjtdh1C2");
        }
    }
}
