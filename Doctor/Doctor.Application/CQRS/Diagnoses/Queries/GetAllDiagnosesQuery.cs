using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diagnoses.Queries
{
    public class GetAllDiagnosesQuery : IRequest<IEnumerable<Diagnosis>> { }

    public class GetAllDiagnosesHandler : IRequestHandler<GetAllDiagnosesQuery, IEnumerable<Diagnosis>>
    {
        private readonly IGenericRepository<Diagnosis> _repo;

        public GetAllDiagnosesHandler(IGenericRepository<Diagnosis> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Diagnosis>> Handle(GetAllDiagnosesQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
