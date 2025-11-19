using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForPresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForPrescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    PatientFullName = table.Column<string>(type: "TEXT", nullable: false),
                    DoctorFullName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Diagnosis = table.Column<string>(type: "TEXT", nullable: true),
                    FilePath = table.Column<string>(type: "TEXT", nullable: true),
                    PdfName = table.Column<string>(type: "TEXT", nullable: true),
                    Prescriptions = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForPrescriptions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 19, 58, 27, 188, DateTimeKind.Utc).AddTicks(4693), "AQAAAAIAAYagAAAAEEvKyq7KhTO5woMPfVkZG9YgQC5hJQPDsc8JIuXIzNshVy+mG7QZzOWx4Mqc29fBkw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForPrescriptions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 18, 59, 26, 700, DateTimeKind.Utc).AddTicks(7300), "AQAAAAIAAYagAAAAENAAJdaRP6esWp7kNwqGVRZyllzWc88A9QgYydJYbMajRv1lJOjNr5k7vUWc+FcPeA==" });
        }
    }
}
