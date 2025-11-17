using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Doctor.Application.CQRS.Treatments.Commands
{
    public class CreateTreatmentCommand : IRequest<int>
    {
        public int PatientId { get; set; }
        public int ServiceId { get; set; }
        public int DiagnosisId { get; set; }
        public string? Notes { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Simptomlar
        public bool HasNausea { get; set; }
        public bool HasVomiting { get; set; }
        public bool HasItching { get; set; }
        public bool HasHeartburn { get; set; }
        public bool HasDiarrhea { get; set; }
        public bool HasJaundice { get; set; }
        public bool HasConstipation { get; set; }
        public bool HasAbdominalPain { get; set; }

        // Birdən çox müayinə anketi (survey)
        public List<TreatmentSurveyDto> Surveys { get; set; } = new();
    }

    public class TreatmentSurveyDto
    {
        public string? Anamnesis { get; set; }
        public DateTime? AnamnesisDate { get; set; }

        public List<IFormFile>? Files { get; set; }

        public List<int>? SelectedDietIds { get; set; }
        public List<int>? SelectedPrescriptionIds { get; set; }

        public string? PreviousDiseases { get; set; }
        public string? MedicationUsage { get; set; }

        public bool HasAllergy { get; set; }
        public bool UsesAlcohol { get; set; }
        public bool Smokes { get; set; }

        public string? PhysicalExamination { get; set; }
        public string? PlanNotes { get; set; }

        public DateTime? NextVisitDate { get; set; }
    }

    public class CreateTreatmentHandler : IRequestHandler<CreateTreatmentCommand, int>
    {
        private readonly ITreatmentRepository _treatmentRepo;
        private readonly IGenericRepository<TreatmentSurvey> _surveyRepo;
        private readonly IGenericRepository<TreatmentSurveyFile> _fileRepo;
        private readonly IGenericRepository<TreatmentSurveyDiet> _dietRepo;
        private readonly IGenericRepository<TreatmentSurveyPrescription> _presRepo;
        private readonly IFileService _fileService;

        public CreateTreatmentHandler(
            ITreatmentRepository treatmentRepo,
            IGenericRepository<TreatmentSurvey> surveyRepo,
            IGenericRepository<TreatmentSurveyFile> fileRepo,
            IGenericRepository<TreatmentSurveyDiet> dietRepo,
            IGenericRepository<TreatmentSurveyPrescription> presRepo,
            IFileService fileService)
        {
            _treatmentRepo = treatmentRepo;
            _surveyRepo = surveyRepo;
            _fileRepo = fileRepo;
            _dietRepo = dietRepo;
            _presRepo = presRepo;
            _fileService = fileService;
        }

        public async Task<int> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
        {
            // === Əsas müayinə (üst hissə) ===
            var treatment = new Treatment
            {
                PatientId = request.PatientId,
                ServiceId = request.ServiceId,
                DiagnosisId = request.DiagnosisId,
                Notes = request.Notes,
                Date = request.Date,
                HasNausea = request.HasNausea,
                HasVomiting = request.HasVomiting,
                HasItching = request.HasItching,
                HasHeartburn = request.HasHeartburn,
                HasDiarrhea = request.HasDiarrhea,
                HasJaundice = request.HasJaundice,
                HasConstipation = request.HasConstipation,
                HasAbdominalPain = request.HasAbdominalPain
            };

            await _treatmentRepo.AddAsync(treatment);
            await _treatmentRepo.SaveAsync();

            // === Hər survey üçün ayrıca anket yarat ===
            foreach (var s in request.Surveys)
            {
                var survey = new TreatmentSurvey
                {
                    TreatmentId = treatment.Id,
                    Anamnesis = s.Anamnesis,
                    AnamnesisDate = s.AnamnesisDate,
                    PreviousDiseases = s.PreviousDiseases,
                    MedicationUsage = s.MedicationUsage,
                    HasAllergy = s.HasAllergy,
                    UsesAlcohol = s.UsesAlcohol,
                    Smokes = s.Smokes,
                    PhysicalExamination = s.PhysicalExamination,
                    PlanNotes = s.PlanNotes,
                    NextVisitDate = s.NextVisitDate
                };

                await _surveyRepo.AddAsync(survey);
                await _surveyRepo.SaveAsync();

                // Fayllar
                if (s.Files != null)
                {
                    foreach (var file in s.Files)
                    {
                        var path = await _fileService.UploadAsync(file, "treatment-surveys");
                        await _fileRepo.AddAsync(new TreatmentSurveyFile
                        {
                            TreatmentSurveyId = survey.Id,
                            FilePath = path,
                            FileType = Path.GetExtension(file.FileName)
                        });
                    }
                    await _fileRepo.SaveAsync();
                }

                // Dietlər
                if (s.SelectedDietIds != null)
                {
                    foreach (var id in s.SelectedDietIds)
                    {
                        await _dietRepo.AddAsync(new TreatmentSurveyDiet
                        {
                            TreatmentSurveyId = survey.Id,
                            DietId = id
                        });
                    }
                    await _dietRepo.SaveAsync();
                }

                // Reseptlər
                if (s.SelectedPrescriptionIds != null)
                {
                    foreach (var id in s.SelectedPrescriptionIds)
                    {
                        await _presRepo.AddAsync(new TreatmentSurveyPrescription
                        {
                            TreatmentSurveyId = survey.Id,
                            PrescriptionId = id
                        });
                    }
                    await _presRepo.SaveAsync();
                }
            }

            return treatment.Id;
        }
    }
}
