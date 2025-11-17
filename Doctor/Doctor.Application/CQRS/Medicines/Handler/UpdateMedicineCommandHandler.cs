using Doctor.Application.Interfaces;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

public class UpdateMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand, bool>
{
    private readonly IMedicineRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateMedicineCommandHandler(IMedicineRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.IsActive = request.IsActive;

        _repo.Update(entity);
        await _uow.SaveChangesAsync();
        return true;
    }
}
