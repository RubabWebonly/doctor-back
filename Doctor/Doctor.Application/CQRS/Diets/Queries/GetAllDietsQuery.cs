using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diets.Queries
{
    public class GetAllDietsQuery : IRequest<IEnumerable<Diet>> { }

    public class GetAllDietsHandler : IRequestHandler<GetAllDietsQuery, IEnumerable<Diet>>
    {
        private readonly IGenericRepository<Diet> _repo;

        public GetAllDietsHandler(IGenericRepository<Diet> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Diet>> Handle(GetAllDietsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
