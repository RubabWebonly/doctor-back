using Doctor.Application.CQRS.ForPrescriptions.Queries;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.ForPrescriptions.Handlers
{
    public class GetAllForPrescriptionsHandler : IRequestHandler<GetAllForPrescriptionsQuery, object>
    {
        private readonly IGenericRepository<ForPrescription> _repo;

        public GetAllForPrescriptionsHandler(IGenericRepository<ForPrescription> repo)
        {
            _repo = repo;
        }

        public async Task<object> Handle(GetAllForPrescriptionsQuery request, CancellationToken ct)
        {
            var list = await _repo.GetAllAsync();
            var filtered = list.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate);

            return filtered.Select(d => new
            {
                d.Id,
                d.PatientFullName,
                d.DoctorFullName,
                d.Date,
                d.FilePath,
                d.PdfName
            }).ToList();
        }
    }
}
