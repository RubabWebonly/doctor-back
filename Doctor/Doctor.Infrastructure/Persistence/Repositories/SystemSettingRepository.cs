using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class SystemSettingRepository : GenericRepository<SystemSetting>, ISystemSettingRepository
    {
        private readonly AppDbContext _context;

        public SystemSettingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SystemSetting?> GetSettingAsync()
        {
            return await _context.SystemSettings.FirstOrDefaultAsync();
        }

        public async Task UpdateSettingAsync(SystemSetting setting)
        {
            var existing = await _context.SystemSettings.FirstOrDefaultAsync();

            if (existing == null)
            {
                await _context.SystemSettings.AddAsync(setting);
            }
            else
            {
                existing.SlotIntervalMinutes = setting.SlotIntervalMinutes;
                existing.WorkStartTime = setting.WorkStartTime;
                existing.WorkEndTime = setting.WorkEndTime;
            }

            await _context.SaveChangesAsync();
        }
    }
}
