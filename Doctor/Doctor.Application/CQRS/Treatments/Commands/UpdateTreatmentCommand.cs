using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Doctor.Application.CQRS.Treatments.Commands
{
    public class UpdateTreatmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }
        public int DiagnosisId { get; set; }

        public string? Complaint { get; set; }   // 🔥 YENİ
        public string? Notes { get; set; }

        // Simptomlar
        public bool HasNausea { get; set; }
        public bool HasVomiting { get; set; }
        public bool HasItching { get; set; }
        public bool HasHeartburn { get; set; }
        public bool HasDiarrhea { get; set; }
        public bool HasJaundice { get; set; }
        public bool HasConstipation { get; set; }
        public bool HasAbdominalPain { get; set; }

        // Anketlər (birdən çox ola bilər)
        public List<TreatmentSurveyUpdateDto> Surveys { get; set; } = new();
    }

    public class TreatmentSurveyUpdateDto
    {
        public int? Id { get; set; } // mövcud anket üçün

        public string? Anamnesis { get; set; }
        public DateTime? AnamnesisDate { get; set; }

        public List<IFormFile>? NewFiles { get; set; }
        public List<string>? ExistingFiles { get; set; }

        public List<int>? SelectedDietIds { get; set; }
        public List<int>? SelectedPrescriptionIds { get; set; }

        public string? PreviousDiseases { get; set; }
        public string? MedicationUsage { get; set; }

        public string? PastSurgeries { get; set; }         // 🔥 YENİ
        public string? HereditaryDiseases { get; set; }    // 🔥 YENİ

        public bool HasAllergy { get; set; }
        public bool UsesAlcohol { get; set; }
        public bool Smokes { get; set; }

        public string? PhysicalExamination { get; set; }
        public string? PlanNotes { get; set; }

        public DateTime? NextVisitDate { get; set; }
    }

    public class UpdateTreatmentHandler : IRequestHandler<UpdateTreatmentCommand, Unit>
    {
        private readonly ITreatmentRepository _treatmentRepo;
        private readonly IGenericRepository<TreatmentSurvey> _surveyRepo;
        private readonly IGenericRepository<TreatmentSurveyFile> _fileRepo;
        private readonly IGenericRepository<TreatmentSurveyDiet> _dietRepo;
        private readonly IGenericRepository<TreatmentSurveyPrescription> _presRepo;
        private readonly IFileService _fileService;

        public UpdateTreatmentHandler(
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

        public async Task<Unit> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
        {
            // ============================
            // 1) TREATMENT UPDATE
            // ============================
            var treatment = await _treatmentRepo.GetByIdAsync(request.Id);
            if (treatment == null)
                throw new Exception("Müalicə tapılmadı.");

            treatment.ServiceId = request.ServiceId;
            treatment.DiagnosisId = request.DiagnosisId;
            treatment.Complaint = request.Complaint;
            treatment.Notes = request.Notes;
            treatment.UpdatedDate = DateTime.UtcNow;

            treatment.HasNausea = request.HasNausea;
            treatment.HasVomiting = request.HasVomiting;
            treatment.HasItching = request.HasItching;
            treatment.HasHeartburn = request.HasHeartburn;
            treatment.HasDiarrhea = request.HasDiarrhea;
            treatment.HasJaundice = request.HasJaundice;
            treatment.HasConstipation = request.HasConstipation;
            treatment.HasAbdominalPain = request.HasAbdominalPain;

            _treatmentRepo.Update(treatment);
            await _treatmentRepo.SaveAsync();

            // ============================
            // 2) BÜTÜN MÖVCUD SURVEY-LƏRİ ÇƏK
            // ============================
            var existingSurveys = (await _surveyRepo.GetAllAsync())
                .Where(x => x.TreatmentId == treatment.Id)
                .ToList();

            // ============================
            // 3) HANSI SURVEY SİLİNMƏLİDİR?
            // ============================
            var idsFromFront = request.Surveys
                .Where(x => x.Id.HasValue)
                .Select(x => x.Id.Value)
                .ToList();

            var surveysToDelete = existingSurveys
                .Where(s => !idsFromFront.Contains(s.Id))
                .ToList();

            foreach (var del in surveysToDelete)
            {
                // files sil
                var files = (await _fileRepo.GetAllAsync())
                    .Where(f => f.TreatmentSurveyId == del.Id)
                    .ToList();

                foreach (var f in files)
                {
                    _fileService.Delete(f.FilePath ?? "");
                    _fileRepo.Delete(f);
                }

                // diet sil
                var diets = (await _dietRepo.GetAllAsync())
                    .Where(d => d.TreatmentSurveyId == del.Id)
                    .ToList();
                foreach (var d in diets) _dietRepo.Delete(d);

                // pres sil
                var pres = (await _presRepo.GetAllAsync())
                    .Where(p => p.TreatmentSurveyId == del.Id)
                    .ToList();
                foreach (var p in pres) _presRepo.Delete(p);

                _surveyRepo.Delete(del);
            }

            await _surveyRepo.SaveAsync();

            // ============================
            // 4) SURVEY UPDATE / CREATE
            // ============================
            foreach (var sDto in request.Surveys)
            {
                TreatmentSurvey survey;

                if (sDto.Id.HasValue)
                {
                    // UPDATE
                    survey = existingSurveys.First(x => x.Id == sDto.Id.Value);
                    survey.Anamnesis = sDto.Anamnesis;
                    survey.AnamnesisDate = sDto.AnamnesisDate;
                    survey.PreviousDiseases = sDto.PreviousDiseases;
                    survey.MedicationUsage = sDto.MedicationUsage;
                    survey.PastSurgeries = sDto.PastSurgeries;
                    survey.HereditaryDiseases = sDto.HereditaryDiseases;
                    survey.HasAllergy = sDto.HasAllergy;
                    survey.UsesAlcohol = sDto.UsesAlcohol;
                    survey.Smokes = sDto.Smokes;
                    survey.PhysicalExamination = sDto.PhysicalExamination;
                    survey.PlanNotes = sDto.PlanNotes;
                    survey.NextVisitDate = sDto.NextVisitDate;

                    _surveyRepo.Update(survey);
                    await _surveyRepo.SaveAsync();
                }
                else
                {
                    // CREATE NEW SURVEY
                    survey = new TreatmentSurvey
                    {
                        TreatmentId = treatment.Id,
                        Anamnesis = sDto.Anamnesis,
                        AnamnesisDate = sDto.AnamnesisDate,
                        PreviousDiseases = sDto.PreviousDiseases,
                        MedicationUsage = sDto.MedicationUsage,
                        PastSurgeries = sDto.PastSurgeries,
                        HereditaryDiseases = sDto.HereditaryDiseases,
                        HasAllergy = sDto.HasAllergy,
                        UsesAlcohol = sDto.UsesAlcohol,
                        Smokes = sDto.Smokes,
                        PhysicalExamination = sDto.PhysicalExamination,
                        PlanNotes = sDto.PlanNotes,
                        NextVisitDate = sDto.NextVisitDate
                    };

                    await _surveyRepo.AddAsync(survey);
                    await _surveyRepo.SaveAsync();
                }

                // ============================
                // FILE UPDATE  
                // ============================
                var allFiles = (await _fileRepo.GetAllAsync())
                    .Where(f => f.TreatmentSurveyId == survey.Id)
                    .ToList();

                var filesToKeep = sDto.ExistingFiles ?? new List<string>();

                var filesToRemove = allFiles
                    .Where(f => !filesToKeep.Contains(f.FilePath ?? ""))
                    .ToList();

                foreach (var f in filesToRemove)
                {
                    _fileService.Delete(f.FilePath ?? "");
                    _fileRepo.Delete(f);
                }

                await _fileRepo.SaveAsync();

                // NEW FILES
                if (sDto.NewFiles != null)
                {
                    foreach (var nf in sDto.NewFiles)
                    {
                        var path = await _fileService.UploadAsync(nf, "treatment-surveys");

                        await _fileRepo.AddAsync(new TreatmentSurveyFile
                        {
                            TreatmentSurveyId = survey.Id,
                            FilePath = path,
                            FileType = Path.GetExtension(nf.FileName)
                        });
                    }

                    await _fileRepo.SaveAsync();
                }

                // ============================
                // DIET UPDATE
                // ============================
                var oldDiets = (await _dietRepo.GetAllAsync())
                    .Where(d => d.TreatmentSurveyId == survey.Id);

                foreach (var d in oldDiets) _dietRepo.Delete(d);
                await _dietRepo.SaveAsync();

                if (sDto.SelectedDietIds != null)
                {
                    foreach (var id in sDto.SelectedDietIds)
                    {
                        if (id > 0)
                            await _dietRepo.AddAsync(new TreatmentSurveyDiet
                            {
                                TreatmentSurveyId = survey.Id,
                                DietId = id
                            });
                    }
                    await _dietRepo.SaveAsync();
                }

                // ============================
                // PRESCRIPTION UPDATE
                // ============================
                var oldPres = (await _presRepo.GetAllAsync())
                    .Where(p => p.TreatmentSurveyId == survey.Id);

                foreach (var p in oldPres) _presRepo.Delete(p);
                await _presRepo.SaveAsync();

                if (sDto.SelectedPrescriptionIds != null)
                {
                    foreach (var id in sDto.SelectedPrescriptionIds)
                    {
                        if (id > 0)
                            await _presRepo.AddAsync(new TreatmentSurveyPrescription
                            {
                                TreatmentSurveyId = survey.Id,
                                PrescriptionId = id
                            });
                    }
                    await _presRepo.SaveAsync();
                }
            }

            return Unit.Value;
        }
    }


}
