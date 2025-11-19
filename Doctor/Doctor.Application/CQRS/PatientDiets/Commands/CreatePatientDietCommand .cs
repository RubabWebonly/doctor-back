using MediatR;

namespace Doctor.Application.CQRS.PatientDiets.Commands
{
    public class CreatePatientDietCommand : IRequest<object>
    {
        public int PatientId { get; set; }
        public string PatientFullName { get; set; } = default!;
        public string DoctorFullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public string PdfName { get; set; } = default!;
        public List<string>? Diets { get; set; }
    }
}
