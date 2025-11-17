using Doctor.Application.Interfaces.Repositories;
using Doctor.Application.Interfaces.Services;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Application.CQRS.Diets.Commands
{
    public class DeleteDietCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteDietHandler : IRequestHandler<DeleteDietCommand, Unit>
    {
        private readonly IGenericRepository<Diet> _repo;
        private readonly IFileService _fileService;

        public DeleteDietHandler(IGenericRepository<Diet> repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(DeleteDietCommand request, CancellationToken cancellationToken)
        {
            var diet = await _repo.GetByIdAsync(request.Id);
            if (diet == null)
                throw new Exception("Diet tapılmadı.");

            if (!string.IsNullOrWhiteSpace(diet.FilePath))
                _fileService.Delete(diet.FilePath);

            _repo.Delete(diet);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
