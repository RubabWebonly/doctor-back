using Doctor.Application.CQRS.PatientDiets.Queries;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.PatientDiets.Handlers
{
    public class GetAllPatientDietsHandler : IRequestHandler<GetAllPatientDietsQuery, object>
    {
        private readonly IPatientDietRepository _repo;

        public GetAllPatientDietsHandler(IPatientDietRepository repo)
        {
            _repo = repo;
        }

        public async Task<object> Handle(GetAllPatientDietsQuery request, CancellationToken ct)
        {
            var diets = await _repo.GetAllWithFilesAsync();

            return diets.Select(d => new
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
