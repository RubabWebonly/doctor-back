using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentSlotTableSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SlotIntervalMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkStartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    WorkEndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 9, 11, 57, 13, 575, DateTimeKind.Utc).AddTicks(8506), "AQAAAAIAAYagAAAAEMwvHubzpKrtwyuKfLZk6gJ/JqtRXliQAKluoyegtxxUlc0XzyJ7uoLXhTam82lh0w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 9, 11, 12, 41, 98, DateTimeKind.Utc).AddTicks(5811), "AQAAAAIAAYagAAAAEFiL0YcmwgEaVUpcyv8fM9SM3hJ8V3i8c2WLidML4r0TKN1DNshtKkB8n8t2087fTA==" });
        }
    }
}
