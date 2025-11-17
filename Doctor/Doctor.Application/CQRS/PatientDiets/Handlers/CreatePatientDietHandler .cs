using Doctor.Application.CQRS.PatientDiets.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Services.Documents;
using Doctor.Domain.Entities;
using MediatR;
using QuestPDF.Fluent;

namespace Doctor.Application.CQRS.PatientDiets.Handlers
{
    public class CreatePatientDietHandler : IRequestHandler<CreatePatientDietCommand, object>
    {
        private readonly IGenericRepository<PatientDiet> _dietRepo;
        private readonly IUnitOfWork _uow;

        public CreatePatientDietHandler(IGenericRepository<PatientDiet> dietRepo, IUnitOfWork uow)
        {
            _dietRepo = dietRepo;
            _uow = uow;
        }

        public async Task<object> Handle(CreatePatientDietCommand req, CancellationToken ct)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "patient-diets");
            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}_{req.PatientFullName}_diet.pdf";
            var fullPath = Path.Combine(uploadFolder, fileName);
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates");

            // 🧾 PDF sənədi yarat
            var document = new PatientDietDocument(
                patientName: req.PatientFullName,
                doctorName: req.DoctorFullName,
                phone: req.PhoneNumber,
                date: req.Date,
                diagnosis: req.Diagnosis ?? string.Empty,
                diets: req.Diets,
                basePath: basePath
            );

            document.GeneratePdf(fullPath);
            await Task.Delay(100);

            var relativePath = File.Exists(fullPath) ? $"/uploads/patient-diets/{fileName}" : null;

            var entity = new PatientDiet
            {
                PatientId = req.PatientId,
                PatientFullName = req.PatientFullName,
                DoctorFullName = req.DoctorFullName,
                PhoneNumber = req.PhoneNumber,
                Date = req.Date,
                Diagnosis = req.Diagnosis,
                FilePath = relativePath,
                PdfName = fileName,
                Diets = req.Diets ?? new List<string>()
            };

            await _dietRepo.AddAsync(entity);
            await _uow.SaveChangesAsync(ct);

            return new { entity.Id, entity.PatientFullName, entity.FilePath, entity.PdfName };
        }
    }
}
