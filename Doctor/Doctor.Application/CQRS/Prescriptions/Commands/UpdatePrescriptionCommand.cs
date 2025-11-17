using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Doctor.Application.CQRS.Prescriptions.Commands
{
    public class UpdatePrescriptionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? File { get; set; }
    }

    public class UpdatePrescriptionHandler : IRequestHandler<UpdatePrescriptionCommand, Unit>
    {
        private readonly IGenericRepository<Prescription> _repo;
        private readonly IFileService _fileService;

        public UpdatePrescriptionHandler(IGenericRepository<Prescription> repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(UpdatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var prescription = await _repo.GetByIdAsync(request.Id);
            if (prescription == null)
                throw new Exception("Resept tapılmadı.");

            prescription.Name = request.Name;

            if (request.File != null)
                prescription.FilePath = await _fileService.SaveFileAsync(request.File, "uploads/prescriptions");

            prescription.UpdatedDate = DateTime.UtcNow;

            _repo.Update(prescription);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
