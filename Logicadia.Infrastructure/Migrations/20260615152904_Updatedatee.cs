using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicadia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatedatee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "RoleId", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@logicadia.com", "Admin", "$2a$11$qR7YVv.MhP2zM6uN6g6pDe8xP7lP2W8qZz8yY7fG4O/kS4rR9aO2G", 1, null });
        }
    }
}
