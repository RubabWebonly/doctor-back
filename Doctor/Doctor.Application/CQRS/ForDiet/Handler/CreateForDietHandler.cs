using Doctor.Application.CQRS.ForDiets.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Services.Documents;
using Doctor.Domain.Entities;
using MediatR;
using QuestPDF.Fluent;

namespace Doctor.Application.CQRS.ForDiets.Handlers
{
    public class CreateForDietHandler : IRequestHandler<CreateForDietCommand, object>
    {
        private readonly IGenericRepository<ForDiet> _repo;
        private readonly IUnitOfWork _uow;

        public CreateForDietHandler(IGenericRepository<ForDiet> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<object> Handle(CreateForDietCommand req, CancellationToken ct)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "for-diets");
            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}_{req.PatientFullName}_fordiet.pdf";
            var fullPath = Path.Combine(uploadFolder, fileName);
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates");

            // ✔ Eyni PDF template istifadə olunur
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

            var relativePath = File.Exists(fullPath) ? $"/uploads/for-diets/{fileName}" : null;

            var entity = new ForDiet
            {
                PatientId = req.PatientId,
                PatientFullName = req.PatientFullName,
                DoctorFullName = req.DoctorFullName,
                PhoneNumber = req.PhoneNumber,
                Date = req.Date,
                Diagnosis = req.Diagnosis,
                FilePath = relativePath,
                PdfName = req.PdfName,
                Diets = req.Diets ?? new List<string>()
            };

            await _repo.AddAsync(entity);
            await _uow.SaveChangesAsync(ct);

            return new { entity.Id, entity.PatientFullName, entity.FilePath, entity.PdfName };
        }
    }
}
