using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Doctor.Application.CQRS.Diets.Commands
{
    public class CreateDietCommand : IRequest<int>
    {
        public string Name { get; set; }
        public IFormFile? File { get; set; }
    }

    public class CreateDietHandler : IRequestHandler<CreateDietCommand, int>
    {
        private readonly IGenericRepository<Diet> _repo;
        private readonly IFileService _fileService;

        public CreateDietHandler(IGenericRepository<Diet> repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<int> Handle(CreateDietCommand request, CancellationToken cancellationToken)
        {
            string? filePath = null;

            if (request.File != null)
                filePath = await _fileService.SaveFileAsync(request.File, "uploads/diets");

            var diet = new Diet
            {
                Name = request.Name,
                FilePath = filePath
            };

            await _repo.AddAsync(diet);
            await _repo.SaveAsync();

            return diet.Id;
        }
    }
}
