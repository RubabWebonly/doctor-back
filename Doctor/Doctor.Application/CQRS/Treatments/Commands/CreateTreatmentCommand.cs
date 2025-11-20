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

        public string? Complaint { get; set; }
        public string? Notes { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public bool HasNausea { get; set; }
        public bool HasVomiting { get; set; }
        public bool HasItching { get; set; }
        public bool HasHeartburn { get; set; }
        public bool HasDiarrhea { get; set; }
        public bool HasJaundice { get; set; }
        public bool HasConstipation { get; set; }
        public bool HasAbdominalPain { get; set; }

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

        public string? PastSurgeries { get; set; }
        public string? HereditaryDiseases { get; set; }

        public bool HasAllergy { get; set; }
        public bool UsesAlcohol { get; set; }
        public bool Smokes { get; set; }

        public string? PhysicalExamination { get; set; }
        public string? PlanNotes { get; set; }

        public DateTime? NextVisitDate { get; set; }
    }

    // ============================ CREATE HANDLER ===============================
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
            // ===================================================================
            // 🔥 1) MAIN TREATMENT YARADILIR
            // ===================================================================

            var treatment = new Treatment
            {
                PatientId = request.PatientId,
                ServiceId = request.ServiceId,
                DiagnosisId = request.DiagnosisId,
                Complaint = request.Complaint,
                Notes = request.Notes,
                Date = request.Date == default ? DateTime.UtcNow : request.Date,

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

            // Survey yoxdursa — qurtar
            if (request.Surveys == null || request.Surveys.Count == 0)
                return treatment.Id;


            // ===================================================================
            // 🔥 2) SURVEY-LƏRİN YARADILMASI
            // ===================================================================
            foreach (var s in request.Surveys)
            {
                var survey = new TreatmentSurvey
                {
                    TreatmentId = treatment.Id,
                    Anamnesis = s.Anamnesis,
                    AnamnesisDate = s.AnamnesisDate,

                    PreviousDiseases = s.PreviousDiseases,
                    MedicationUsage = s.MedicationUsage,
                    PastSurgeries = s.PastSurgeries,
                    HereditaryDiseases = s.HereditaryDiseases,

                    HasAllergy = s.HasAllergy,
                    UsesAlcohol = s.UsesAlcohol,
                    Smokes = s.Smokes,

                    PhysicalExamination = s.PhysicalExamination,
                    PlanNotes = s.PlanNotes,
                    NextVisitDate = s.NextVisitDate
                };

                await _surveyRepo.AddAsync(survey);
                await _surveyRepo.SaveAsync();


                // ===================================================================
                // 🔥 3) FILES — FAYLLARIN YÜKLƏNMƏSİ
                // ===================================================================
                if (s.Files is not null && s.Files.Count > 0)
                {
                    foreach (var file in s.Files)
                    {
                        var path = await _fileService.UploadAsync(file, "treatment-surveys");

                        var surveyFile = new TreatmentSurveyFile
                        {
                            TreatmentSurveyId = survey.Id,
                            FilePath = path,
                            FileType = Path.GetExtension(file.FileName)
                        };

                        await _fileRepo.AddAsync(surveyFile);
                    }

                    await _fileRepo.SaveAsync();
                }


                // ===================================================================
                // 🔥 4) DIETS
                // ===================================================================
                if (s.SelectedDietIds != null)
                {
                    foreach (var dietId in s.SelectedDietIds)
                    {
                        if (dietId <= 0) continue;

                        var diet = new TreatmentSurveyDiet
                        {
                            TreatmentSurveyId = survey.Id,
                            DietId = dietId
                        };

                        await _dietRepo.AddAsync(diet);
                    }

                    await _dietRepo.SaveAsync();
                }


                // ===================================================================
                // 🔥 5) PRESCRIPTIONS
                // ===================================================================
                if (s.SelectedPrescriptionIds != null)
                {
                    foreach (var presId in s.SelectedPrescriptionIds)
                    {
                        if (presId <= 0) continue;

                        var pres = new TreatmentSurveyPrescription
                        {
                            TreatmentSurveyId = survey.Id,
                            PrescriptionId = presId
                        };

                        await _presRepo.AddAsync(pres);
                    }

                    await _presRepo.SaveAsync();
                }
            }

            return treatment.Id;
        }
    }
}
