using MediatR;

namespace Doctor.Application.CQRS.PatientPrescriptions.Commands
{
    public class CreatePatientPrescriptionCommand : IRequest<object>
    {
        public int PatientId { get; set; }
        public string PatientFullName { get; set; } = default!;
        public string DoctorFullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public List<string>? Diets { get; set; }
    }
}
