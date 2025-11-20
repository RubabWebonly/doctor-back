using Doctor.Domain.Entities;

public class TreatmentSurveyPrescription : BaseEntity
{
    public int TreatmentSurveyId { get; set; }
    public TreatmentSurvey TreatmentSurvey { get; set; }

    public int PrescriptionId { get; set; }
    public PatientPrescription Prescription { get; set; } // 🔥 DÜZGÜN OLAN
}
