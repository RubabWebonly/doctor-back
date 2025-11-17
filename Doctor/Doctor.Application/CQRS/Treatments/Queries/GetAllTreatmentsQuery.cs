using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Treatments.Queries
{
    public class GetAllTreatmentsQuery : IRequest<IEnumerable<Treatment>> { }

    public class GetAllTreatmentsHandler : IRequestHandler<GetAllTreatmentsQuery, IEnumerable<Treatment>>
    {
        private readonly ITreatmentRepository _repo;

        public GetAllTreatmentsHandler(ITreatmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Treatment>> Handle(GetAllTreatmentsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
