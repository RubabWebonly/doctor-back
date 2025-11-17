using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Clinics.Queries
{
    public class GetAllClinicsQuery : IRequest<IEnumerable<Clinic>> { }

    public class GetAllClinicsHandler : IRequestHandler<GetAllClinicsQuery, IEnumerable<Clinic>>
    {
        private readonly IGenericRepository<Clinic> _repo;

        public GetAllClinicsHandler(IGenericRepository<Clinic> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Clinic>> Handle(GetAllClinicsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
