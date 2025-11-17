using Doctor.Application.Interfaces;
using Doctor.Domain.Entities;
using Doctor.Infrastructure.Persistence;
using Doctor.Infrastructure.Persistence.Repositories;

public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
{
    public MedicineRepository(AppDbContext context) : base(context)
    {
    }
}
