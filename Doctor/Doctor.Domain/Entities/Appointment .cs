
using Doctor.Domain.Enums;

namespace Doctor.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
        public int ClinicId { get; set; }   
        public Clinic Clinic { get; set; }

        public int AppointmentSlotId { get; set; }
        public AppointmentSlot AppointmentSlot { get; set; }

        public string? Complaint { get; set; }
        public ReferralSource ReferralSource { get; set; } = ReferralSource.None;
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public PaymentType PaymentType { get; set; } = PaymentType.Cash;
        public decimal? RemainingAmount { get; set; }

        public string? Notes { get; set; }
    }
}
