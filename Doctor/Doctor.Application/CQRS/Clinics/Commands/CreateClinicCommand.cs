using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Clinics.Commands
{
    public class CreateClinicCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }

    public class CreateClinicHandler : IRequestHandler<CreateClinicCommand, int>
    {
        private readonly IGenericRepository<Clinic> _repo;

        public CreateClinicHandler(IGenericRepository<Clinic> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateClinicCommand request, CancellationToken cancellationToken)
        {
            var clinic = new Clinic
            {
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone
            };

            await _repo.AddAsync(clinic);
            await _repo.SaveAsync();
            return clinic.Id;
        }
    }
}
