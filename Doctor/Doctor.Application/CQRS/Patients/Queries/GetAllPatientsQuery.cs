using Doctor.Domain.Entities;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Doctor.Application.CQRS.Patients.Queries
{
    public class GetAllPatientsQuery : IRequest<IEnumerable<Patient>> { }

    public class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, IEnumerable<Patient>>
    {
        private readonly IPatientRepository _repo;

        public GetAllPatientsHandler(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Patient>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
