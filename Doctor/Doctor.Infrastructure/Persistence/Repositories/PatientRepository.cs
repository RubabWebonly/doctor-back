using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context) { }
    }
}
