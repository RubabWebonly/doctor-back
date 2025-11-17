using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Prescriptions.Queries
{
    public class GetPrescriptionByIdQuery : IRequest<Prescription>
    {
        public int Id { get; set; }
    }

    public class GetPrescriptionByIdHandler : IRequestHandler<GetPrescriptionByIdQuery, Prescription>
    {
        private readonly IGenericRepository<Prescription> _repo;

        public GetPrescriptionByIdHandler(IGenericRepository<Prescription> repo)
        {
            _repo = repo;
        }

        public async Task<Prescription> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
        {
            var prescription = await _repo.GetByIdAsync(request.Id);
            if (prescription == null)
                throw new Exception("Resept tapılmadı.");

            return prescription;
        }
    }
}
