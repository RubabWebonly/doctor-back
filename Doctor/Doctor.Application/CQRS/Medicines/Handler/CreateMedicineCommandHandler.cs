using Doctor.Application.Interfaces;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

public class CreateMedicineCommandHandler : IRequestHandler<CreateMedicineCommand, int>
{
    private readonly IMedicineRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateMedicineCommandHandler(IMedicineRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<int> Handle(CreateMedicineCommand request, CancellationToken cancellationToken)
    {
        var entity = new Medicine
        {
            Name = request.Name,
            IsActive = true
        };

        await _repo.AddAsync(entity);
        await _uow.SaveChangesAsync();

        return entity.Id;
    }
}
