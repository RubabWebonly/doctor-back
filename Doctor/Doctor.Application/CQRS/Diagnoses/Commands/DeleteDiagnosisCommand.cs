using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Diagnoses.Commands
{
    public class DeleteDiagnosisCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteDiagnosisHandler : IRequestHandler<DeleteDiagnosisCommand, Unit>
    {
        private readonly IGenericRepository<Diagnosis> _repo;

        public DeleteDiagnosisHandler(IGenericRepository<Diagnosis> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteDiagnosisCommand request, CancellationToken cancellationToken)
        {
            var diagnosis = await _repo.GetByIdAsync(request.Id);
            if (diagnosis == null)
                throw new Exception("Diaqnoz tapılmadı.");

            _repo.Delete(diagnosis);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
