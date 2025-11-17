using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Patients.Queries
{
    public class GetPatientStatsQuery : IRequest<IEnumerable<PatientStatDto>>
    {
        public string Range { get; set; } = "weekly"; // weekly / monthly
        public int Year { get; set; } = DateTime.Now.Year; // illə filtr
    }

    public class PatientStatDto
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }

    public class GetPatientStatsHandler : IRequestHandler<GetPatientStatsQuery, IEnumerable<PatientStatDto>>
    {
        private readonly IGenericRepository<Patient> _repo;

        public GetPatientStatsHandler(IGenericRepository<Patient> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PatientStatDto>> Handle(GetPatientStatsQuery request, CancellationToken cancellationToken)
        {
            var all = await _repo.GetAllAsync();

            if (request.Range.ToLower() == "weekly")
            {
                // 7 gün üçün həftəlik data
                var daysOfWeek = new[] { "B.e", "Ç.a", "Çər", "C.a", "Cüm", "Şənb", "Baz" };
                var last7days = DateTime.Now.AddDays(-6);

                var grouped = all
                    .Where(p => p.CreatedDate >= last7days)
                    .GroupBy(p => p.CreatedDate.ToString("ddd"))
                    .Select(g => new PatientStatDto
                    {
                        Label = ConvertDayName(g.Key),
                        Count = g.Count()
                    })
                    .ToList();

                // olmayan günlər üçün 0 əlavə et
                foreach (var day in daysOfWeek)
                {
                    if (!grouped.Any(x => x.Label == day))
                        grouped.Add(new PatientStatDto { Label = day, Count = 0 });
                }

                return grouped.OrderBy(x => Array.IndexOf(daysOfWeek, x.Label)).ToList();
            }
            else
            {
                var months = new[] { "Yan", "Fev", "Mar", "Apr", "May", "İyn", "İyl", "Avq", "Sen", "Okt", "Noy", "Dek" };

                var grouped = all
                    .Where(p => p.CreatedDate.Year == request.Year)
                    .GroupBy(p => p.CreatedDate.Month)
                    .Select(g => new PatientStatDto
                    {
                        Label = months[g.Key - 1],
                        Count = g.Count()
                    })
                    .ToList();


                foreach (var m in months)
                {
                    if (!grouped.Any(x => x.Label == m))
                        grouped.Add(new PatientStatDto { Label = m, Count = 0 });
                }

                return grouped.OrderBy(x => Array.IndexOf(months, x.Label)).ToList();
            }
        }

        private string ConvertDayName(string en)
        {
            return en switch
            {
                "Mon" => "B.e",
                "Tue" => "Ç.a",
                "Wed" => "Çər",
                "Thu" => "C.a",
                "Fri" => "Cüm",
                "Sat" => "Şənb",
                "Sun" => "Baz",
                _ => en
            };
        }
    }
}
