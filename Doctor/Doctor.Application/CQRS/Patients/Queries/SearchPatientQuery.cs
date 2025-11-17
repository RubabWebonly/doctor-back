using Application.Common;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Patients.Queries
{
    public class SearchPatientQuery : IRequest<Result<List<Patient>>>
    {
        public string Query { get; set; } = string.Empty;
    }

    public class SearchPatientHandler : IRequestHandler<SearchPatientQuery, Result<List<Patient>>>
    {
        private readonly IPatientRepository _repo;

        public SearchPatientHandler(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<List<Patient>>> Handle(SearchPatientQuery request, CancellationToken cancellationToken)
        {
            var all = await _repo.GetAllAsync();

            // 🔍 Ad və ya nömrəyə görə axtarış
            var filtered = all
                .Where(p =>
                    (!string.IsNullOrEmpty(p.FullName) && p.FullName.Contains(request.Query, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(p.PhoneNumber) && p.PhoneNumber.Contains(request.Query))
                )
                .ToList();

            return Result<List<Patient>>.Ok(filtered);
        }
    }
}
