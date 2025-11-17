using System;

namespace Doctor.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int? ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public string InitialDiagnosis { get; set; }
        public string DoctorReferral { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
