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
            // 🔹 Əsas Treatment məlumatlarını tap
            var treatment = await _treatmentRepo.GetByIdAsync(request.Id);
            if (treatment == null)
                throw new Exception("Müalicə tapılmadı.");

            // 🔹 Əsas müayinə hissəsini yenilə
            treatment.ServiceId = request.ServiceId;
            treatment.DiagnosisId = request.DiagnosisId;
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

            // 🔹 Mövcud anketləri tap
            var allSurveys = await _surveyRepo.GetAllAsync();
            var treatmentSurveys = allSurveys.Where(s => s.TreatmentId == treatment.Id).ToList();

            foreach (var surveyDto in request.Surveys)
            {
                TreatmentSurvey survey;

                // mövcud anket varsa
                if (surveyDto.Id.HasValue)
                {
                    survey = treatmentSurveys.FirstOrDefault(s => s.Id == surveyDto.Id.Value);
                    if (survey == null) continue;

                    survey.Anamnesis = surveyDto.Anamnesis;
                    survey.AnamnesisDate = surveyDto.AnamnesisDate;
                    survey.PreviousDiseases = surveyDto.PreviousDiseases;
                    survey.MedicationUsage = surveyDto.MedicationUsage;
                    survey.HasAllergy = surveyDto.HasAllergy;
                    survey.UsesAlcohol = surveyDto.UsesAlcohol;
                    survey.Smokes = surveyDto.Smokes;
                    survey.PhysicalExamination = surveyDto.PhysicalExamination;
                    survey.PlanNotes = surveyDto.PlanNotes;
                    survey.NextVisitDate = surveyDto.NextVisitDate;

                    _surveyRepo.Update(survey);
                    await _surveyRepo.SaveAsync();
                }
                else
                {
                    // yeni anket əlavə et
                    survey = new TreatmentSurvey
                    {
                        TreatmentId = treatment.Id,
                        Anamnesis = surveyDto.Anamnesis,
                        AnamnesisDate = surveyDto.AnamnesisDate,
                        PreviousDiseases = surveyDto.PreviousDiseases,
                        MedicationUsage = surveyDto.MedicationUsage,
                        HasAllergy = surveyDto.HasAllergy,
                        UsesAlcohol = surveyDto.UsesAlcohol,
                        Smokes = surveyDto.Smokes,
                        PhysicalExamination = surveyDto.PhysicalExamination,
                        PlanNotes = surveyDto.PlanNotes,
                        NextVisitDate = surveyDto.NextVisitDate
                    };
                    await _surveyRepo.AddAsync(survey);
                    await _surveyRepo.SaveAsync();
                }

                // 🔹 Faylları yenilə
                var allFiles = await _fileRepo.GetAllAsync();
                var filesForSurvey = allFiles.Where(f => f.TreatmentSurveyId == survey.Id).ToList();

                var filesToRemove = filesForSurvey
                    .Where(f => surveyDto.ExistingFiles == null || !surveyDto.ExistingFiles.Contains(f.FilePath))
                    .ToList();

                foreach (var f in filesToRemove)
                {
                    _fileService.Delete(f.FilePath);
                    _fileRepo.Delete(f);
                }
                await _fileRepo.SaveAsync();

                if (surveyDto.NewFiles != null)
                {
                    foreach (var file in surveyDto.NewFiles)
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

                // 🔹 Diet və reseptləri yenilə
                var allDiets = await _dietRepo.GetAllAsync();
                var oldDiets = allDiets.Where(d => d.TreatmentSurveyId == survey.Id);
                foreach (var d in oldDiets)
                    _dietRepo.Delete(d);
                await _dietRepo.SaveAsync();

                if (surveyDto.SelectedDietIds != null)
                {
                    foreach (var id in surveyDto.SelectedDietIds)
                        await _dietRepo.AddAsync(new TreatmentSurveyDiet
                        {
                            TreatmentSurveyId = survey.Id,
                            DietId = id
                        });
                    await _dietRepo.SaveAsync();
                }

                var allPres = await _presRepo.GetAllAsync();
                var oldPres = allPres.Where(p => p.TreatmentSurveyId == survey.Id);
                foreach (var p in oldPres)
                    _presRepo.Delete(p);
                await _presRepo.SaveAsync();

                if (surveyDto.SelectedPrescriptionIds != null)
                {
                    foreach (var id in surveyDto.SelectedPrescriptionIds)
                        await _presRepo.AddAsync(new TreatmentSurveyPrescription
                        {
                            TreatmentSurveyId = survey.Id,
                            PrescriptionId = id
                        });
                    await _presRepo.SaveAsync();
                }
            }

            return Unit.Value;
        }
    }
}
