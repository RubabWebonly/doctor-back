using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Doctor.Application.CQRS.Patients.Commands
{
    public class DeletePatientCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeletePatientHandler : IRequestHandler<DeletePatientCommand, Unit>
    {
        private readonly IPatientRepository _repo;

        public DeletePatientHandler(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _repo.GetByIdAsync(request.Id);
            if (patient == null)
                throw new Exception("Pasiyent tapılmadı.");

            _repo.Delete(patient);
            await _repo.SaveAsync();
            return Unit.Value;
        }
    }
}
