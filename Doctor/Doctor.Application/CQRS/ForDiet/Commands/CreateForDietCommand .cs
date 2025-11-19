using MediatR;
using System;
using System.Collections.Generic;

namespace Doctor.Application.CQRS.ForDiets.Commands
{
    public class CreateForDietCommand : IRequest<object>
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
