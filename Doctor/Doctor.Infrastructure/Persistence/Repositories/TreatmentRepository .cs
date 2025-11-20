using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class TreatmentRepository : GenericRepository<Treatment>, ITreatmentRepository
    {
        private readonly AppDbContext _context;
        public TreatmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Treatment?> GetByIdAsync(int id)
        {
            return await _context.Treatments
                .Include(x => x.Patient)
                .Include(x => x.Service)
                .Include(x => x.Diagnosis)

                .Include(x => x.Surveys)
                    .ThenInclude(s => s.Files)

                .Include(x => x.Surveys)
                    .ThenInclude(s => s.Diets)
                        .ThenInclude(d => d.Diet)

                .Include(x => x.Surveys)
                    .ThenInclude(s => s.Prescriptions)
                        .ThenInclude(p => p.Prescription)

                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Treatment>> GetAllWithDiagnosisAsync()
        {
            return await _context.Treatments
                .Include(t => t.Diagnosis)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await _context.Treatments
                .Include(x => x.Patient)
                .Include(x => x.Service)
                .Include(x => x.Diagnosis)

                .Include(x => x.Surveys)
                    .ThenInclude(s => s.Files)

                .Include(x => x.Surveys)
                    .ThenInclude(s => s.Diets)
                        .ThenInclude(d => d.Diet)

                .Include(x => x.Surveys)
                    .ThenInclude(s => s.Prescriptions)
                        .ThenInclude(p => p.Prescription)

                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

    }
}
