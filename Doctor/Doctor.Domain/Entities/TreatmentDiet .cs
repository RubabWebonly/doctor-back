
namespace Doctor.Domain.Entities
{
    public class TreatmentDiet : BaseEntity
    {
        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public int DietId { get; set; }
        public Diet Diet { get; set; }
    }
}
