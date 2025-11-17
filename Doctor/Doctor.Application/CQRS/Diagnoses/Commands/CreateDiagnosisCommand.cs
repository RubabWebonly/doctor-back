using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diagnoses.Commands
{
    public class CreateDiagnosisCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateDiagnosisHandler : IRequestHandler<CreateDiagnosisCommand, int>
    {
        private readonly IGenericRepository<Diagnosis> _repo;

        public CreateDiagnosisHandler(IGenericRepository<Diagnosis> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = new Diagnosis
            {
                Name = request.Name,
                IsActive = true
            };

            await _repo.AddAsync(diagnosis);
            await _repo.SaveAsync();

            return diagnosis.Id;
        }
    }
}
