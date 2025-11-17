using Doctor.Domain.Entities;

public class Treatment : BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public int ServiceId { get; set; }
    public Service Service { get; set; }

    public int DiagnosisId { get; set; }
    public Diagnosis Diagnosis { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public bool HasNausea { get; set; }
    public bool HasVomiting { get; set; }
    public bool HasItching { get; set; }
    public bool HasHeartburn { get; set; }
    public bool HasDiarrhea { get; set; }
    public bool HasJaundice { get; set; }
    public bool HasConstipation { get; set; }
    public bool HasAbdominalPain { get; set; }

    public ICollection<TreatmentSurvey> Surveys { get; set; } = new List<TreatmentSurvey>();
}
