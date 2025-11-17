using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Doctor.Application.CQRS.Diets.Commands
{
    public class UpdateDietCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? File { get; set; }
    }

    public class UpdateDietHandler : IRequestHandler<UpdateDietCommand, Unit>
    {
        private readonly IGenericRepository<Diet> _repo;
        private readonly IFileService _fileService;

        public UpdateDietHandler(IGenericRepository<Diet> repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(UpdateDietCommand request, CancellationToken cancellationToken)
        {
            var diet = await _repo.GetByIdAsync(request.Id);
            if (diet == null)
                throw new Exception("Diet tapılmadı.");

            diet.Name = request.Name;

            if (request.File != null)
                diet.FilePath = await _fileService.SaveFileAsync(request.File, "uploads/diets");

            diet.UpdatedDate = DateTime.UtcNow;
            _repo.Update(diet);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
