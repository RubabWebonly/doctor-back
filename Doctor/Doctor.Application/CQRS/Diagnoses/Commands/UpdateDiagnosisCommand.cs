using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diagnoses.Commands
{
    public class UpdateDiagnosisCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateDiagnosisHandler : IRequestHandler<UpdateDiagnosisCommand, Unit>
    {
        private readonly IGenericRepository<Diagnosis> _repo;

        public UpdateDiagnosisHandler(IGenericRepository<Diagnosis> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = await _repo.GetByIdAsync(request.Id);
            if (diagnosis == null)
                throw new Exception("Diaqnoz tapılmadı.");

            diagnosis.Name = request.Name;
            diagnosis.IsActive = request.IsActive;
            diagnosis.UpdatedDate = DateTime.UtcNow;

            _repo.Update(diagnosis);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
