using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePatientDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "PatientPrescriptions",
                newName: "PdfName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PatientPrescriptions",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "PatientDiets",
                newName: "PdfName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PatientDiets",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PatientPrescriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "PatientPrescriptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diets",
                table: "PatientPrescriptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorFullName",
                table: "PatientPrescriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientFullName",
                table: "PatientPrescriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PatientDiets",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "PatientDiets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diets",
                table: "PatientDiets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorFullName",
                table: "PatientDiets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientFullName",
                table: "PatientDiets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 8, 5, 37, 53, 351, DateTimeKind.Utc).AddTicks(4128), "AQAAAAIAAYagAAAAEIiLJ2yKklwXw7C1x9yig0hDEpQPmdw0840QlfZiqs9uVOIqoGT0MlgBLmHdKXR5Aw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PatientPrescriptions");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "PatientPrescriptions");

            migrationBuilder.DropColumn(
                name: "Diets",
                table: "PatientPrescriptions");

            migrationBuilder.DropColumn(
                name: "DoctorFullName",
                table: "PatientPrescriptions");

            migrationBuilder.DropColumn(
                name: "PatientFullName",
                table: "PatientPrescriptions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "PatientDiets");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "PatientDiets");

            migrationBuilder.DropColumn(
                name: "Diets",
                table: "PatientDiets");

            migrationBuilder.DropColumn(
                name: "DoctorFullName",
                table: "PatientDiets");

            migrationBuilder.DropColumn(
                name: "PatientFullName",
                table: "PatientDiets");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "PatientPrescriptions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PdfName",
                table: "PatientPrescriptions",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "PatientDiets",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PdfName",
                table: "PatientDiets",
                newName: "Notes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAIAAYagAAAAEICMCXEMs9Qymw7Atq0UCN4T7CFQh1quoOcPkXvq+BYTWD33pyxEzKP67+AFj2hCcg==" });
        }
    }
}
