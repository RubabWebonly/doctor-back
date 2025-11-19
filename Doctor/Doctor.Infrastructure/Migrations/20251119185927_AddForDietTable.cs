using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForDietTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForDiets",
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
                    Diets = table.Column<string>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForDiets", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 18, 59, 26, 700, DateTimeKind.Utc).AddTicks(7300), "AQAAAAIAAYagAAAAENAAJdaRP6esWp7kNwqGVRZyllzWc88A9QgYydJYbMajRv1lJOjNr5k7vUWc+FcPeA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForDiets");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 11, 57, 22, 83, DateTimeKind.Utc).AddTicks(6819), "AQAAAAIAAYagAAAAEHgylZreF0bLaFIWF7oSdM/noEk8LnMvROXAl94UfRQiLR9i1fINj3IgTc66cnj/Ug==" });
        }
    }
}
