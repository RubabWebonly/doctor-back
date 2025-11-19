using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSimptom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HereditaryDiseases",
                table: "TreatmentSurvey",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PastSurgeries",
                table: "TreatmentSurvey",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complaint",
                table: "Treatments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 11, 57, 22, 83, DateTimeKind.Utc).AddTicks(6819), "AQAAAAIAAYagAAAAEHgylZreF0bLaFIWF7oSdM/noEk8LnMvROXAl94UfRQiLR9i1fINj3IgTc66cnj/Ug==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HereditaryDiseases",
                table: "TreatmentSurvey");

            migrationBuilder.DropColumn(
                name: "PastSurgeries",
                table: "TreatmentSurvey");

            migrationBuilder.DropColumn(
                name: "Complaint",
                table: "Treatments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 17, 7, 34, 52, 938, DateTimeKind.Utc).AddTicks(9843), "AQAAAAIAAYagAAAAEOh/d8Po4HIJ9KRhHggEdPzPrtXVUALpzAadAEuccwUiRl3sZm+Jyk45U8f6Mtz6eA==" });
        }
    }
}
