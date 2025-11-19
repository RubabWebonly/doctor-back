using Doctor.Application.CQRS.ForPrescriptions.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.ForPrescriptions.Handlers
{
    public class DeleteForPrescriptionHandler : IRequestHandler<DeleteForPrescriptionCommand, bool>
    {
        private readonly IGenericRepository<ForPrescription> _repo;
        private readonly IUnitOfWork _uow;

        public DeleteForPrescriptionHandler(IGenericRepository<ForPrescription> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteForPrescriptionCommand request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;

            _repo.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
