using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDateTimeInTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkCompleted",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "WorkStarted",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "TicketClosed",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketClosed",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkCompleted",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkStarted",
                table: "Tickets",
                type: "datetime2",
                nullable: true);
        }
    }
}
