using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentSurveyDiet_Diets_DietId",
                table: "TreatmentSurveyDiet");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentSurveyPrescription_Prescriptions_PrescriptionId",
                table: "TreatmentSurveyPrescription");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 20, 11, 44, 47, 869, DateTimeKind.Utc).AddTicks(1969), "AQAAAAIAAYagAAAAEDcjpKFJB20zx7dfj/B/Xt/0BpKrDKXpY4ABI72RufROHNLS0Bx9ZsCe5HJ7k7aGHg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentSurveyDiet_PatientDiets_DietId",
                table: "TreatmentSurveyDiet",
                column: "DietId",
                principalTable: "PatientDiets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentSurveyPrescription_PatientPrescriptions_PrescriptionId",
                table: "TreatmentSurveyPrescription",
                column: "PrescriptionId",
                principalTable: "PatientPrescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentSurveyDiet_PatientDiets_DietId",
                table: "TreatmentSurveyDiet");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentSurveyPrescription_PatientPrescriptions_PrescriptionId",
                table: "TreatmentSurveyPrescription");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 19, 19, 58, 27, 188, DateTimeKind.Utc).AddTicks(4693), "AQAAAAIAAYagAAAAEEvKyq7KhTO5woMPfVkZG9YgQC5hJQPDsc8JIuXIzNshVy+mG7QZzOWx4Mqc29fBkw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentSurveyDiet_Diets_DietId",
                table: "TreatmentSurveyDiet",
                column: "DietId",
                principalTable: "Diets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentSurveyPrescription_Prescriptions_PrescriptionId",
                table: "TreatmentSurveyPrescription",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
