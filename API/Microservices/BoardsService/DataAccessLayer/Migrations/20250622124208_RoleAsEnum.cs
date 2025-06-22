using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RoleAsEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedRoles",
                table: "BoardRole");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "BoardRole",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "BoardRole");

            migrationBuilder.AddColumn<int>(
                name: "AllowedRoles",
                table: "BoardRole",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
