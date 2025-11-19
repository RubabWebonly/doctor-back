using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Treatments.Commands
{
    public class DeleteTreatmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteTreatmentHandler : IRequestHandler<DeleteTreatmentCommand, Unit>
    {
        private readonly ITreatmentRepository _treatmentRepo;
        private readonly IGenericRepository<TreatmentSurvey> _surveyRepo;
        private readonly IGenericRepository<TreatmentSurveyFile> _fileRepo;
        private readonly IGenericRepository<TreatmentSurveyDiet> _dietRepo;           // 🔥 YENİ
        private readonly IGenericRepository<TreatmentSurveyPrescription> _presRepo;   // 🔥 YENİ
        private readonly IFileService _fileService;

        public DeleteTreatmentHandler(
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

        public async Task<Unit> Handle(DeleteTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = await _treatmentRepo.GetByIdAsync(request.Id);
            if (treatment == null)
                throw new Exception("Müalicə tapılmadı.");

            // ==== Əlaqəli survey-ləri tap ====
            var allSurveys = await _surveyRepo.GetAllAsync();
            var relatedSurveys = allSurveys
                .Where(x => x.TreatmentId == treatment.Id)
                .ToList();

            if (relatedSurveys.Any())
            {
                // Faylları, dietləri, reseptləri hamısını bir dəfə çəkirik
                var allFiles = await _fileRepo.GetAllAsync();
                var allDiets = await _dietRepo.GetAllAsync();
                var allPrescriptions = await _presRepo.GetAllAsync();

                foreach (var survey in relatedSurveys)
                {
                    // ==== Fayllar ====
                    var files = allFiles
                        .Where(f => f.TreatmentSurveyId == survey.Id)
                        .ToList();

                    foreach (var file in files)
                    {
                        if (!string.IsNullOrWhiteSpace(file.FilePath))
                            _fileService.Delete(file.FilePath);

                        _fileRepo.Delete(file);
                    }

                    // ==== Dietlər ====
                    var diets = allDiets
                        .Where(d => d.TreatmentSurveyId == survey.Id)
                        .ToList();

                    foreach (var d in diets)
                        _dietRepo.Delete(d);

                    // ==== Reseptlər ====
                    var prescriptions = allPrescriptions
                        .Where(p => p.TreatmentSurveyId == survey.Id)
                        .ToList();

                    foreach (var p in prescriptions)
                        _presRepo.Delete(p);

                    // ==== Survey-in özünü sil ====
                    _surveyRepo.Delete(survey);
                }

                await _fileRepo.SaveAsync();
                await _dietRepo.SaveAsync();
                await _presRepo.SaveAsync();
                await _surveyRepo.SaveAsync();
            }

            // Əsas Treatment-i sil
            _treatmentRepo.Delete(treatment);
            await _treatmentRepo.SaveAsync();

            return Unit.Value;
        }
    }
}
