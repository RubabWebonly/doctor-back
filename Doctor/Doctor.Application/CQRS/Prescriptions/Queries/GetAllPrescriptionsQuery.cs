using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Prescriptions.Queries
{
    public class GetAllPrescriptionsQuery : IRequest<IEnumerable<Prescription>> { }

    public class GetAllPrescriptionsHandler : IRequestHandler<GetAllPrescriptionsQuery, IEnumerable<Prescription>>
    {
        private readonly IGenericRepository<Prescription> _repo;

        public GetAllPrescriptionsHandler(IGenericRepository<Prescription> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Prescription>> Handle(GetAllPrescriptionsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
