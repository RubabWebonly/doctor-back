using Doctor.Application.CQRS.ForDiets.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.ForDiets.Handlers
{
    public class DeleteForDietHandler : IRequestHandler<DeleteForDietCommand, bool>
    {
        private readonly IGenericRepository<ForDiet> _repo;
        private readonly IUnitOfWork _uow;

        public DeleteForDietHandler(IGenericRepository<ForDiet> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteForDietCommand request, CancellationToken ct)
        {
            var diet = await _repo.GetByIdAsync(request.Id);
            if (diet == null)
                return false;

            diet.IsDeleted = true;
            diet.DeletedDate = DateTime.UtcNow;

            _repo.Update(diet);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
