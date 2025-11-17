using Doctor.Application.CQRS.PatientDiets.Queries;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.PatientDiets.Handlers
{
    public class GetAllPatientDietsHandler : IRequestHandler<GetAllPatientDietsQuery, object>
    {
        private readonly IGenericRepository<PatientDiet> _repo;

        public GetAllPatientDietsHandler(IGenericRepository<PatientDiet> repo)
        {
            _repo = repo;
        }

        public async Task<object> Handle(GetAllPatientDietsQuery request, CancellationToken ct)
        {
            var diets = await _repo.GetAllAsync();

            // Əgər soft delete varsa, filter et
            var filtered = diets.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate);

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
