using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Treatments.Queries
{
    public class GetTreatmentStatsQuery : IRequest<IEnumerable<TreatmentStatDto>>
    {
        public string Range { get; set; } = "weekly"; // weekly / monthly
        public int Year { get; set; } = DateTime.Now.Year;
    }

    public class TreatmentStatDto
    {
        public string Diagnosis { get; set; }
        public decimal Percentage { get; set; }
    }

    public class GetTreatmentStatsHandler : IRequestHandler<GetTreatmentStatsQuery, IEnumerable<TreatmentStatDto>>
    {
        private readonly ITreatmentRepository _repo;

        public GetTreatmentStatsHandler(ITreatmentRepository repo)
        {
            _repo = repo;
        }


        public async Task<IEnumerable<TreatmentStatDto>> Handle(GetTreatmentStatsQuery request, CancellationToken cancellationToken)
        {
            // ✅ Repository artıq Include(Diagnosis) ilə gələcək
            var all = await _repo.GetAllWithDiagnosisAsync();

            IEnumerable<Treatment> filtered;

            if (request.Range.ToLower() == "weekly")
            {
                var fromDate = DateTime.Now.AddDays(-7);
                filtered = all.Where(t => t.CreatedDate >= fromDate);
            }
            else
            {
                filtered = all.Where(t => t.CreatedDate.Year == request.Year);
            }

            if (!filtered.Any())
                return new List<TreatmentStatDto>();

            var total = filtered.Count();

            var grouped = filtered
                .GroupBy(t => t.Diagnosis != null ? t.Diagnosis.Name : "Naməlum")
                .Select(g => new TreatmentStatDto
                {
                    Diagnosis = g.Key,
                    Percentage = Math.Round((decimal)g.Count() / total * 100, 1)
                })
                .OrderByDescending(x => x.Percentage)
                .ToList();

            return grouped;
        }
    }
}
