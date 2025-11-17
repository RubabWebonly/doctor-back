using Doctor.Domain.Entities;

namespace Doctor.Application.Interfaces.Repositories
{
    public interface IAppointmentSlotRepository : IGenericRepository<AppointmentSlot>
    {
        Task<List<AppointmentSlot>> GetAvailableSlotsAsync(DateTime date);
        Task<bool> ExistsForYearAsync(int year);
        Task GenerateSlotsAsync(int year, int slotIntervalMinutes, TimeSpan workStart, TimeSpan workEnd);
    }
}
