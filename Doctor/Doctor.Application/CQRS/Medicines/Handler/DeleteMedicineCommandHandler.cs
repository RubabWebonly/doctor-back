using Doctor.Application.Interfaces;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

public class DeleteMedicineCommandHandler : IRequestHandler<DeleteMedicineCommand, bool>
{
    private readonly IMedicineRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteMedicineCommandHandler(IMedicineRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return false;

        entity.IsActive = false;

        _repo.Update(entity);
        await _uow.SaveChangesAsync();
        return true;
    }
}
