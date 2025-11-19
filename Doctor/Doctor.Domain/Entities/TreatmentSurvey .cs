namespace Doctor.Domain.Entities
{
    public class TreatmentSurvey : BaseEntity
    {
        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public string? Anamnesis { get; set; }
        public DateTime? AnamnesisDate { get; set; }

        public ICollection<TreatmentSurveyFile> Files { get; set; } = new List<TreatmentSurveyFile>();
        public ICollection<TreatmentSurveyDiet> Diets { get; set; } = new List<TreatmentSurveyDiet>();
        public ICollection<TreatmentSurveyPrescription> Prescriptions { get; set; } = new List<TreatmentSurveyPrescription>();

        public string? PreviousDiseases { get; set; }     
        public string? MedicationUsage { get; set; }      

        public bool HasAllergy { get; set; }             
        public bool UsesAlcohol { get; set; }             
        public bool Smokes { get; set; }                  

        public string? PhysicalExamination { get; set; }
        public string? PlanNotes { get; set; }
        public string? PastSurgeries { get; set; }
        public string? HereditaryDiseases { get; set; }


        public DateTime? NextVisitDate { get; set; }
    }
}
