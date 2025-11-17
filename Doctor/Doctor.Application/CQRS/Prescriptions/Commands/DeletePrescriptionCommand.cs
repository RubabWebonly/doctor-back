using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Prescriptions.Commands
{
    public class DeletePrescriptionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeletePrescriptionHandler : IRequestHandler<DeletePrescriptionCommand, Unit>
    {
        private readonly IGenericRepository<Prescription> _repo;
        private readonly IFileService _fileService;

        public DeletePrescriptionHandler(IGenericRepository<Prescription> repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var prescription = await _repo.GetByIdAsync(request.Id);
            if (prescription == null)
                throw new Exception("Resept tapılmadı.");

            // Əgər reseptin faylı varsa, onu da sil
            if (!string.IsNullOrWhiteSpace(prescription.FilePath))
            {
                _fileService.Delete(prescription.FilePath);
            }

            _repo.Delete(prescription);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
