using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Clinics.Commands
{
    public class UpdateClinicCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateClinicHandler : IRequestHandler<UpdateClinicCommand, Unit>
    {
        private readonly IGenericRepository<Clinic> _repo;

        public UpdateClinicHandler(IGenericRepository<Clinic> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
        {
            var clinic = await _repo.GetByIdAsync(request.Id);
            if (clinic == null)
                throw new Exception("Klinika tapılmadı.");

            clinic.Name = request.Name;
            clinic.Address = request.Address;
            clinic.Phone = request.Phone;

            _repo.Update(clinic);
            await _repo.SaveAsync();
            return Unit.Value;
        }
    }
}
