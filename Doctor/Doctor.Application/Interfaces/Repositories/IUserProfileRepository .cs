using Doctor.Domain.Entities;

namespace Doctor.Application.Interfaces.Repositories
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        Task<UserProfile?> GetByEmailAsync(string email);
    }
}
