using Doctor.Application.CQRS.PatientDiets.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.PatientDiets.Handlers
{
    public class DeletePatientDietHandler : IRequestHandler<DeletePatientDietCommand, bool>
    {
        private readonly IGenericRepository<PatientDiet> _repo;
        private readonly IUnitOfWork _uow;

        public DeletePatientDietHandler(IGenericRepository<PatientDiet> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeletePatientDietCommand request, CancellationToken ct)
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
