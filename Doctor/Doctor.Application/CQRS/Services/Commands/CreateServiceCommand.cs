using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Services.Commands
{
    public class CreateServiceCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, int>
    {
        private readonly IGenericRepository<Service> _repo;

        public CreateServiceHandler(IGenericRepository<Service> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new Service
            {
                Name = request.Name,
                IsActive = true
            };

            await _repo.AddAsync(service);
            await _repo.SaveAsync();

            return service.Id;
        }
    }
}
