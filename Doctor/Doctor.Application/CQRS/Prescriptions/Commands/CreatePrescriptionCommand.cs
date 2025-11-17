using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Doctor.Application.CQRS.Prescriptions.Commands
{
    public class CreatePrescriptionCommand : IRequest<int>
    {
        public string Name { get; set; }
        public IFormFile? File { get; set; }
    }

    public class CreatePrescriptionHandler : IRequestHandler<CreatePrescriptionCommand, int>
    {
        private readonly IGenericRepository<Prescription> _repo;
        private readonly IFileService _fileService;

        public CreatePrescriptionHandler(IGenericRepository<Prescription> repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<int> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            string? filePath = null;
            if (request.File != null)
                filePath = await _fileService.SaveFileAsync(request.File, "uploads/prescriptions");

            var prescription = new Prescription
            {
                Name = request.Name,
                FilePath = filePath
            };

            await _repo.AddAsync(prescription);
            await _repo.SaveAsync();

            return prescription.Id;
        }
    }
}
