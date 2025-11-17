using Doctor.Domain.Entities;

namespace Doctor.Application.Interfaces.Repositories
{
    public interface ISystemSettingRepository : IGenericRepository<SystemSetting>
    {
        Task<SystemSetting?> GetSettingAsync();
        Task UpdateSettingAsync(SystemSetting setting);
    }
}
