using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Application.CQRS.Appointments.Queries
{
    public class GetAppointmentStatsQuery : IRequest<IEnumerable<AppointmentStatDto>>
    {
        public string Range { get; set; } = "weekly"; // weekly / monthly
        public int Year { get; set; } = DateTime.Now.Year;
    }

    public class AppointmentStatDto
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }

    public class GetAppointmentStatsHandler : IRequestHandler<GetAppointmentStatsQuery, IEnumerable<AppointmentStatDto>>
    {
        private readonly IGenericRepository<Appointment> _repo;

        public GetAppointmentStatsHandler(IGenericRepository<Appointment> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<AppointmentStatDto>> Handle(GetAppointmentStatsQuery request, CancellationToken cancellationToken)
        {
            // Appointment-ları slot ilə birlikdə götürürük
            var all = await _repo.GetAllAsync(q => q.Include(a => a.AppointmentSlot));

            if (request.Range.ToLower() == "weekly")
            {
                var daysOfWeek = new[] { "B.e", "Ç.a", "Çər", "C.a", "Cüm", "Şənb", "Baz" };
                var last7days = DateTime.Now.Date.AddDays(-6);

                var grouped = all
                    .Where(a => a.AppointmentSlot != null && a.AppointmentSlot.Date.Date >= last7days)
                    .GroupBy(a => a.AppointmentSlot.Date.DayOfWeek)
                    .Select(g => new AppointmentStatDto
                    {
                        Label = ConvertDayName(g.Key),
                        Count = g.Count()
                    })
                    .ToList();

                // Boş günləri sıfırla əlavə et
                foreach (var day in daysOfWeek)
                {
                    if (!grouped.Any(x => x.Label == day))
                        grouped.Add(new AppointmentStatDto { Label = day, Count = 0 });
                }

                return grouped.OrderBy(x => Array.IndexOf(daysOfWeek, x.Label)).ToList();
            }
            else
            {
                var months = new[] { "Yan", "Fev", "Mar", "Apr", "May", "İyn", "İyl", "Avq", "Sen", "Okt", "Noy", "Dek" };

                var grouped = all
                    .Where(a => a.AppointmentSlot != null && a.AppointmentSlot.Date.Year == request.Year)
                    .GroupBy(a => a.AppointmentSlot.Date.Month)
                    .Select(g => new AppointmentStatDto
                    {
                        Label = months[g.Key - 1],
                        Count = g.Count()
                    })
                    .ToList();

                // Boş ayları sıfırla əlavə et
                foreach (var m in months)
                {
                    if (!grouped.Any(x => x.Label == m))
                        grouped.Add(new AppointmentStatDto { Label = m, Count = 0 });
                }

                return grouped.OrderBy(x => Array.IndexOf(months, x.Label)).ToList();
            }
        }

        private string ConvertDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "B.e",
                DayOfWeek.Tuesday => "Ç.a",
                DayOfWeek.Wednesday => "Çər",
                DayOfWeek.Thursday => "C.a",
                DayOfWeek.Friday => "Cüm",
                DayOfWeek.Saturday => "Şənb",
                DayOfWeek.Sunday => "Baz",
                _ => ""
            };
        }
    }
}
