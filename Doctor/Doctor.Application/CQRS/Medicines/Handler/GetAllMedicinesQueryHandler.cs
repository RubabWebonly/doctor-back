using Doctor.Application.Interfaces;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllMedicinesQueryHandler : IRequestHandler<GetAllMedicinesQuery, IEnumerable<Medicine>>
{
    private readonly IMedicineRepository _repo;

    public GetAllMedicinesQueryHandler(IMedicineRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Medicine>> Handle(GetAllMedicinesQuery request, CancellationToken cancellationToken)
    {
        return await _repo.Query()
                          .Where(x => x.IsActive)
                          .ToListAsync();
    }
}
