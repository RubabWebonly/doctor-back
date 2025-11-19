using Doctor.Application.CQRS.ForDiets.Queries;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.ForDiets.Handlers
{
    public class GetAllForDietsHandler : IRequestHandler<GetAllForDietsQuery, object>
    {
        private readonly IGenericRepository<ForDiet> _repo;

        public GetAllForDietsHandler(IGenericRepository<ForDiet> repo)
        {
            _repo = repo;
        }

        public async Task<object> Handle(GetAllForDietsQuery request, CancellationToken ct)
        {
            var diets = await _repo.GetAllAsync();
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
