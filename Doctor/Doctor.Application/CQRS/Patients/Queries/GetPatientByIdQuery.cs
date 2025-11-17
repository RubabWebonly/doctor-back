using Doctor.Domain.Entities;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Doctor.Application.CQRS.Patients.Queries
{
    public class GetPatientByIdQuery : IRequest<Patient>
    {
        public int Id { get; set; }
    }

    public class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, Patient>
    {
        private readonly IPatientRepository _repo;

        public GetPatientByIdHandler(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<Patient> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _repo.GetByIdAsync(request.Id);
            return patient!;
        }
    }
}
