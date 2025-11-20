using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Doctor.Infrastructure.Persistence;
using Doctor.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

public class PatientDietRepository : GenericRepository<PatientDiet>, IPatientDietRepository
{
    private readonly AppDbContext _context;

    public PatientDietRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<PatientDiet>> GetAllWithFilesAsync()
    {
        return await _context.PatientDiets
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
}
