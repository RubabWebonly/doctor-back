
namespace Doctor.Domain.Entities
{
    public class TreatmentPrescription : BaseEntity
    {
        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
    }
}
