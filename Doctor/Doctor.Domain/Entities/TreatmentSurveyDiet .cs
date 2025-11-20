using Doctor.Domain.Entities;

public class TreatmentSurveyDiet : BaseEntity
{
    public int TreatmentSurveyId { get; set; }
    public TreatmentSurvey TreatmentSurvey { get; set; }

    public int DietId { get; set; }
    public PatientDiet Diet { get; set; } // 🔥 DÜZGÜN OLAN
}
