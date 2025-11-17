using Application.Common;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly ICategoryRepository _repo;

    public DeleteCategoryCommandHandler(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null)
            return Result<bool>.Fail("Kateqoriya tapılmadı!");

        _repo.Delete(entity);
        await _repo.SaveAsync();

        return Result<bool>.Ok(true);
    }
}
