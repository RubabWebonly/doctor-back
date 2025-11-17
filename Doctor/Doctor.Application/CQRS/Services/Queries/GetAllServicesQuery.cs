using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Services.Queries
{
    public class GetAllServicesQuery : IRequest<IEnumerable<Service>> { }

    public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, IEnumerable<Service>>
    {
        private readonly IGenericRepository<Service> _repo;

        public GetAllServicesHandler(IGenericRepository<Service> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Service>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
