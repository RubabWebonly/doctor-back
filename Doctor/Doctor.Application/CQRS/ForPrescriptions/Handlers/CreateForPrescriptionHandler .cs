using Doctor.Application.CQRS.ForPrescriptions.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Services.Documents;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;

namespace Doctor.Application.CQRS.ForPrescriptions.Handlers
{
    public class CreateForPrescriptionHandler : IRequestHandler<CreateForPrescriptionCommand, object>
    {
        private readonly IGenericRepository<ForPrescription> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;

        public CreateForPrescriptionHandler(
            IGenericRepository<ForPrescription> repo,
            IUnitOfWork uow,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _uow = uow;
            _env = env;
        }

        public async Task<object> Handle(CreateForPrescriptionCommand req, CancellationToken ct)
        {
            // 🔥 DÜZGÜN PATH → həmişə wwwroot-a düşəcək
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "for-prescriptions");
            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}_{req.PatientFullName}_prescription.pdf";
            var fullPath = Path.Combine(uploadFolder, fileName);

            // 🔥 Template-lər üçün də WebRootPath istifadə edirik
            var templateBase = Path.Combine(_env.WebRootPath, "templates");

            var document = new PatientPrescriptionDocument(
                patientName: req.PatientFullName,
                doctorName: req.DoctorFullName,
                phone: req.PhoneNumber,
                date: req.Date,
                diagnosis: req.Diagnosis ?? "",
                items: req.Prescriptions,
                basePath: templateBase
            );

            document.GeneratePdf(fullPath);

            var relativePath = File.Exists(fullPath)
                ? $"/uploads/for-prescriptions/{fileName}"
                : null;

            var entity = new ForPrescription
            {
                PatientId = req.PatientId,
                PatientFullName = req.PatientFullName,
                DoctorFullName = req.DoctorFullName,
                PhoneNumber = req.PhoneNumber,
                Date = req.Date,
                Diagnosis = req.Diagnosis,
                FilePath = relativePath,
                PdfName = req.PdfName,
                Prescriptions = req.Prescriptions ?? new List<string>()
            };

            await _repo.AddAsync(entity);
            await _uow.SaveChangesAsync(ct);

            return new
            {
                entity.Id,
                entity.PatientFullName,
                entity.FilePath,
                entity.PdfName
            };
        }
    }
}
