using Doctor.Application.CQRS.PatientPrescriptions.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.PatientPrescriptions.Handlers
{
    public class DeletePatientPrescriptionHandler : IRequestHandler<DeletePatientPrescriptionCommand, bool>
    {
        private readonly IGenericRepository<PatientPrescription> _repo;
        private readonly IUnitOfWork _uow;

        public DeletePatientPrescriptionHandler(IGenericRepository<PatientPrescription> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeletePatientPrescriptionCommand request, CancellationToken ct)
        {
            var prescription = await _repo.GetByIdAsync(request.Id);
            if (prescription == null)
                return false;

            prescription.IsDeleted = true;
            prescription.DeletedDate = DateTime.UtcNow;

            _repo.Update(prescription);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}
