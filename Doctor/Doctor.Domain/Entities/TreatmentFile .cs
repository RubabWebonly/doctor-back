
namespace Doctor.Domain.Entities
{
    public class TreatmentFile : BaseEntity
    {
        public int TreatmentId { get; set; }
        public Treatment Treatment { get; set; }

        public string FilePath { get; set; }
        public string FileType { get; set; }
    }
}
