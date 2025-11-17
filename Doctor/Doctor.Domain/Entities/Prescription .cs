
namespace Doctor.Domain.Entities
{
    public class Prescription : BaseEntity
    {
        public string Name { get; set; }
        public string? FilePath { get; set; }
    }
}
