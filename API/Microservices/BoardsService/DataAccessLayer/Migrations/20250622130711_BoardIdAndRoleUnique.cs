using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class BoardIdAndRoleUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BoardRole_BoardId",
                table: "BoardRole");

            migrationBuilder.CreateIndex(
                name: "IX_BoardRole_BoardId_Role",
                table: "BoardRole",
                columns: new[] { "BoardId", "Role" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BoardRole_BoardId_Role",
                table: "BoardRole");

            migrationBuilder.CreateIndex(
                name: "IX_BoardRole_BoardId",
                table: "BoardRole",
                column: "BoardId");
        }
    }
}
