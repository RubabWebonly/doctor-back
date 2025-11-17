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
        private readonly IFileService _fileService;

        public DeleteTreatmentHandler(
            ITreatmentRepository treatmentRepo,
            IGenericRepository<TreatmentSurvey> surveyRepo,
            IGenericRepository<TreatmentSurveyFile> fileRepo,
            IFileService fileService)
        {
            _treatmentRepo = treatmentRepo;
            _surveyRepo = surveyRepo;
            _fileRepo = fileRepo;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(DeleteTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = await _treatmentRepo.GetByIdAsync(request.Id);
            if (treatment == null)
                throw new Exception("Müalicə tapılmadı.");

            // Anketləri sil
            var surveys = await _surveyRepo.GetAllAsync();
            var related = surveys.Where(x => x.TreatmentId == treatment.Id).ToList();

            foreach (var s in related)
            {
                var files = (await _fileRepo.GetAllAsync())
                    .Where(f => f.TreatmentSurveyId == s.Id).ToList();

                foreach (var file in files)
                {
                    if (!string.IsNullOrWhiteSpace(file.FilePath))
                        _fileService.Delete(file.FilePath);
                    _fileRepo.Delete(file);
                }
                _surveyRepo.Delete(s);
            }

            await _fileRepo.SaveAsync();
            await _surveyRepo.SaveAsync();

            _treatmentRepo.Delete(treatment);
            await _treatmentRepo.SaveAsync();

            return Unit.Value;
        }
    }
}
