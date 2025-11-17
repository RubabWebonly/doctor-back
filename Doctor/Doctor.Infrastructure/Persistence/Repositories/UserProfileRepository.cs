using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        private readonly AppDbContext _context;
        public UserProfileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByEmailAsync(string email)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
