using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedDateToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PatientPrescriptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PatientDiets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 8, 6, 29, 14, 712, DateTimeKind.Utc).AddTicks(1657), "AQAAAAIAAYagAAAAEFjuy0TH4reX/AZzJ+ccyMsKl9qA9Oi2ucbCL0YdLyP3FJbv4gPAZ2zP1p7ezArXVA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "PatientPrescriptions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "PatientDiets");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 8, 5, 37, 53, 351, DateTimeKind.Utc).AddTicks(4128), "AQAAAAIAAYagAAAAEIiLJ2yKklwXw7C1x9yig0hDEpQPmdw0840QlfZiqs9uVOIqoGT0MlgBLmHdKXR5Aw==" });
        }
    }
}
