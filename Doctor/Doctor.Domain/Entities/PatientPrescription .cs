using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Entities
{
    public class PatientPrescription : BaseEntity
    {
        public int PatientId { get; set; }
        public string PatientFullName { get; set; } = default!;
        public string DoctorFullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public string? FilePath { get; set; }
        public string? PdfName { get; set; }
        public List<string>? Diets { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
