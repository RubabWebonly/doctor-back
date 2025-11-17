using Doctor.Application.CQRS.PatientPrescriptions.Queries;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.PatientPrescriptions.Handlers
{
    public class GetAllPatientPrescriptionsHandler : IRequestHandler<GetAllPatientPrescriptionsQuery, object>
    {
        private readonly IGenericRepository<PatientPrescription> _repo;

        public GetAllPatientPrescriptionsHandler(IGenericRepository<PatientPrescription> repo)
        {
            _repo = repo;
        }

        public async Task<object> Handle(GetAllPatientPrescriptionsQuery request, CancellationToken ct)
        {
            var prescriptions = await _repo.GetAllAsync();
            var filtered = prescriptions.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate);

            return filtered.Select(p => new
            {
                p.Id,
                p.PatientFullName,
                p.DoctorFullName,
                p.Date,
                p.FilePath,
                p.PdfName
            }).ToList();
        }
    }
}
