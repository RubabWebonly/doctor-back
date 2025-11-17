using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Services.Commands
{
    public class DeleteServiceCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, Unit>
    {
        private readonly IGenericRepository<Service> _repo;

        public DeleteServiceHandler(IGenericRepository<Service> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _repo.GetByIdAsync(request.Id);
            if (service == null)
                throw new Exception("Xidmət tapılmadı.");

            _repo.Delete(service);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
