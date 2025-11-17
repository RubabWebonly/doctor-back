using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Services.Queries
{
    public class GetServiceByIdQuery : IRequest<Service>
    {
        public int Id { get; set; }
    }

    public class GetServiceByIdHandler : IRequestHandler<GetServiceByIdQuery, Service>
    {
        private readonly IGenericRepository<Service> _repo;

        public GetServiceByIdHandler(IGenericRepository<Service> repo)
        {
            _repo = repo;
        }

        public async Task<Service> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _repo.GetByIdAsync(request.Id);
            if (service == null)
                throw new Exception("Xidmət tapılmadı.");

            return service;
        }
    }
}
