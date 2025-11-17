using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Treatments.Queries
{
    public class GetTreatmentByIdQuery : IRequest<Treatment>
    {
        public int Id { get; set; }
    }

    public class GetTreatmentByIdHandler : IRequestHandler<GetTreatmentByIdQuery, Treatment>
    {
        private readonly ITreatmentRepository _repo;

        public GetTreatmentByIdHandler(ITreatmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Treatment> Handle(GetTreatmentByIdQuery request, CancellationToken cancellationToken)
        {
            var treatment = await _repo.GetByIdAsync(request.Id);
            if (treatment == null)
                throw new Exception("Müalicə tapılmadı.");
            return treatment;
        }
    }
}
