using Doctor.Application.CQRS.Treatments.Commands;
using Doctor.Infrastructure.Persistence;
using FluentValidation;
using System.Linq;

namespace Doctor.Application.Validators
{
    public class CreateTreatmentCommandValidator : AbstractValidator<CreateTreatmentCommand>
    {
        private readonly AppDbContext _context;

        public CreateTreatmentCommandValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.PatientId)
                .Must(id => _context.Patients.Any(p => p.Id == id))
                .WithMessage("Pasiyent tapılmadı.");

            RuleFor(x => x.ServiceId)
                .Must(id => _context.Services.Any(s => s.Id == id))
                .WithMessage("Xidmət tapılmadı.");

            RuleFor(x => x.DiagnosisId)
                .Must(id => _context.Diagnoses.Any(d => d.Id == id))
                .WithMessage("Diaqnoz tapılmadı.");

            RuleFor(x => x.Notes)
                .MaximumLength(2000)
                .WithMessage("Qeyd maksimum 2000 simvol ola bilər.");
        }
    }
}
