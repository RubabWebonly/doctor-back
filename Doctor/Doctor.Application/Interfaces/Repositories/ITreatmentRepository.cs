using Doctor.Domain.Entities;

namespace Doctor.Application.Interfaces.Repositories
{
    public interface ITreatmentRepository : IGenericRepository<Treatment>
    {
        Task<IEnumerable<Treatment>> GetAllWithDiagnosisAsync();
    }
}
