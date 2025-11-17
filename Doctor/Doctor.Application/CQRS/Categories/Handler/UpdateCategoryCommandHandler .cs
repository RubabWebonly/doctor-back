using Application.Common;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    private readonly ICategoryRepository _repo;

    public UpdateCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null)
            return Result<bool>.Fail("Kateqoriya tapılmadı!");

        entity.Name = request.Name;

        _repo.Update(entity);
        await _repo.SaveAsync();

        return Result<bool>.Ok(true);
    }
}
