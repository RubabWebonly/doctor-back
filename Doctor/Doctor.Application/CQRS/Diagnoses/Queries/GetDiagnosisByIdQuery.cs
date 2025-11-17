using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diagnoses.Queries
{
    public class GetDiagnosisByIdQuery : IRequest<Diagnosis>
    {
        public int Id { get; set; }
    }

    public class GetDiagnosisByIdHandler : IRequestHandler<GetDiagnosisByIdQuery, Diagnosis>
    {
        private readonly IGenericRepository<Diagnosis> _repo;

        public GetDiagnosisByIdHandler(IGenericRepository<Diagnosis> repo)
        {
            _repo = repo;
        }

        public async Task<Diagnosis> Handle(GetDiagnosisByIdQuery request, CancellationToken cancellationToken)
        {
            var diagnosis = await _repo.GetByIdAsync(request.Id);
            if (diagnosis == null)
                throw new Exception("Diaqnoz tapılmadı.");

            return diagnosis;
        }
    }
}
