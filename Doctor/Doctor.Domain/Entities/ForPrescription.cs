using System;
using System.Collections.Generic;

namespace Doctor.Domain.Entities
{
    public class ForPrescription : BaseEntity
    {
        public int PatientId { get; set; }
        public string PatientFullName { get; set; } = default!;
        public string DoctorFullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }

        public string? FilePath { get; set; }
        public string? PdfName { get; set; }

        // Resept maddələri (dietlər yox!)
        public List<string>? Prescriptions { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
