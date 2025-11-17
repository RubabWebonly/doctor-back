
namespace Doctor.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    }
}
