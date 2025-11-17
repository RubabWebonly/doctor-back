
namespace Doctor.Domain.Entities
{
    public class Clinic : BaseEntity
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
