
namespace Doctor.Domain.Entities
{
    public class Diagnosis : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
