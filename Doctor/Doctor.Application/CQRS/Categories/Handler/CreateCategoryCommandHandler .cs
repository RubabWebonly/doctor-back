using Application.Common;
using Doctor.Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    private readonly ICategoryRepository _repo;

    public CreateCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name
        };

        await _repo.AddAsync(entity);
        await _repo.SaveAsync();

        return Result<int>.Ok(entity.Id);
    }
}
