using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Clinics.Queries
{
    public class GetClinicByIdQuery : IRequest<Clinic>
    {
        public int Id { get; set; }
    }

    public class GetClinicByIdHandler : IRequestHandler<GetClinicByIdQuery, Clinic>
    {
        private readonly IGenericRepository<Clinic> _repo;

        public GetClinicByIdHandler(IGenericRepository<Clinic> repo)
        {
            _repo = repo;
        }

        public async Task<Clinic> Handle(GetClinicByIdQuery request, CancellationToken cancellationToken)
        {
            var clinic = await _repo.GetByIdAsync(request.Id);
            if (clinic == null)
                throw new Exception("Klinika tapılmadı.");

            return clinic;
        }
    }
}
