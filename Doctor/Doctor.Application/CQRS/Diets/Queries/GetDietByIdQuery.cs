using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diets.Queries
{
    public class GetDietByIdQuery : IRequest<Diet>
    {
        public int Id { get; set; }
    }

    public class GetDietByIdHandler : IRequestHandler<GetDietByIdQuery, Diet>
    {
        private readonly IGenericRepository<Diet> _repo;

        public GetDietByIdHandler(IGenericRepository<Diet> repo)
        {
            _repo = repo;
        }

        public async Task<Diet> Handle(GetDietByIdQuery request, CancellationToken cancellationToken)
        {
            var diet = await _repo.GetByIdAsync(request.Id);
            if (diet == null)
                throw new Exception("Diet tapılmadı.");

            return diet;
        }
    }
}
