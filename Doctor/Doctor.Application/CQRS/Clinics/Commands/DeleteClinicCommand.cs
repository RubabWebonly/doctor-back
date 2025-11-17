using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Clinics.Commands
{
    public class DeleteClinicCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteClinicHandler : IRequestHandler<DeleteClinicCommand, Unit>
    {
        private readonly IGenericRepository<Clinic> _repo;

        public DeleteClinicHandler(IGenericRepository<Clinic> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteClinicCommand request, CancellationToken cancellationToken)
        {
            var clinic = await _repo.GetByIdAsync(request.Id);
            if (clinic == null)
                throw new Exception("Klinika tapılmadı.");

            _repo.Delete(clinic);
            await _repo.SaveAsync();
            return Unit.Value;
        }
    }
}
