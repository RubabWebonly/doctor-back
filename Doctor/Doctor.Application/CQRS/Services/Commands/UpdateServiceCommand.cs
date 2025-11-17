using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Services.Commands
{
    public class UpdateServiceCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, Unit>
    {
        private readonly IGenericRepository<Service> _repo;

        public UpdateServiceHandler(IGenericRepository<Service> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _repo.GetByIdAsync(request.Id);
            if (service == null)
                throw new Exception("Xidmət tapılmadı.");

            service.Name = request.Name;
            service.IsActive = request.IsActive;
            service.UpdatedDate = DateTime.UtcNow;

            _repo.Update(service);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
