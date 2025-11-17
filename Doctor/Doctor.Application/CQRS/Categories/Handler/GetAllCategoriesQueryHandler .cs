using Doctor.Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    private readonly ICategoryRepository _repo;

    public GetAllCategoriesQueryHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
