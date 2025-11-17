using Doctor.Application.CQRS.Treatments.Commands;
using Doctor.Infrastructure.Persistence;
using FluentValidation;
using System.Linq;

public class UpdateTreatmentCommandValidator : AbstractValidator<UpdateTreatmentCommand>
{
    private readonly AppDbContext _context;

    public UpdateTreatmentCommandValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .Must(id => _context.Treatments.Any(t => t.Id == id))
            .WithMessage("Müalicə tapılmadı.");

        RuleFor(x => x.ServiceId)
            .Must(id => _context.Services.Any(s => s.Id == id))
            .WithMessage("Xidmət tapılmadı.");

        RuleFor(x => x.DiagnosisId)
            .Must(id => _context.Diagnoses.Any(d => d.Id == id))
            .WithMessage("Diaqnoz tapılmadı.");
    }
}
