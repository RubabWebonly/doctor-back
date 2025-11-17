using Application.Interfaces.Repositories;
using Doctor.Infrastructure.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.PhoneNumber == phone, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsTracking() 
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }



    }
}
