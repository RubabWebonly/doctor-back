using Doctor.Application.CQRS.PatientPrescriptions.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Services.Documents;
using Doctor.Domain.Entities;
using MediatR;
using QuestPDF.Fluent;

namespace Doctor.Application.CQRS.PatientPrescriptions.Handlers
{
    public class CreatePatientPrescriptionHandler : IRequestHandler<CreatePatientPrescriptionCommand, object>
    {
        private readonly IGenericRepository<PatientPrescription> _repo;
        private readonly IUnitOfWork _uow;

        public CreatePatientPrescriptionHandler(IGenericRepository<PatientPrescription> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<object> Handle(CreatePatientPrescriptionCommand req, CancellationToken ct)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "patient-prescriptions");
            Directory.CreateDirectory(uploadFolder);

            var fileName = $"{Guid.NewGuid()}_{req.PatientFullName}_prescription.pdf";
            var fullPath = Path.Combine(uploadFolder, fileName);
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates");

            // 🧾 PDF sənədi yarat
            var document = new PatientPrescriptionDocument(
                patientName: req.PatientFullName,
                doctorName: req.DoctorFullName,
                phone: req.PhoneNumber,
                date: req.Date,
                diagnosis: req.Diagnosis ?? string.Empty,
                  items: req.Diets,
                basePath: basePath
            );

            document.GeneratePdf(fullPath);
            await Task.Delay(100);

            var relativePath = File.Exists(fullPath) ? $"/uploads/patient-prescriptions/{fileName}" : null;

            var entity = new PatientPrescription
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
