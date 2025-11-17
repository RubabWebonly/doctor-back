using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Infrastructure.Persistence.Repositories
{
    public class AppointmentSlotRepository : GenericRepository<AppointmentSlot>, IAppointmentSlotRepository
    {
        private readonly AppDbContext _context;

        public AppointmentSlotRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Verilmiş tarix üçün boş (rezerv olunmamış) slotları qaytarır.
        /// </summary>
        public async Task<List<AppointmentSlot>> GetAvailableSlotsAsync(DateTime date)
        {
            var slots = await _context.AppointmentSlots
                .Where(x => x.Date.Date == date.Date && !x.IsBooked)
                .AsNoTracking()
                .ToListAsync();

            // SQLite TimeSpan-ları sıralaya bilmir, ona görə client-side sort edilir
            return slots.OrderBy(x => x.StartTime).ToList();
        }

        /// <summary>
        /// Verilmiş il üçün slotların artıq yaradılıb-yaradılmadığını yoxlayır.
        /// </summary>
        public async Task<bool> ExistsForYearAsync(int year)
        {
            return await _context.AppointmentSlots.AnyAsync(x => x.Date.Year == year);
        }

        /// <summary>
        /// Sistem ayarlarına əsasən verilmiş il üçün slotları yaradır.
        /// </summary>
        public async Task GenerateSlotsAsync(int year, int slotIntervalMinutes, TimeSpan workStart, TimeSpan workEnd)
        {
            // 1️⃣ Mövcud ilin rezerv olunmamış slotlarını sil
            var existingSlots = await _context.AppointmentSlots
                .Where(x => x.Date.Year == year && !x.IsBooked)
                .ToListAsync();

            if (existingSlots.Any())
                _context.AppointmentSlots.RemoveRange(existingSlots);

            // 2️⃣ Yeni slotlar siyahısını hazırla
            var newSlots = new List<AppointmentSlot>();
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = new DateTime(year, 12, 31);
            var interval = TimeSpan.FromMinutes(slotIntervalMinutes);

            for (var date = startOfYear; date <= endOfYear; date = date.AddDays(1))
            {
                // Bazar günlərini keçirik
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                for (var time = workStart; time < workEnd; time = time.Add(interval))
                {
                    newSlots.Add(new AppointmentSlot
                    {
                        Date = date,
                        StartTime = time,
                        EndTime = time.Add(interval),
                        IsBooked = false
                    });
                }
            }

            // 3️⃣ Yeni slotları əlavə et və yadda saxla
            await _context.AppointmentSlots.AddRangeAsync(newSlots);
            await _context.SaveChangesAsync();
        }
    }
}
