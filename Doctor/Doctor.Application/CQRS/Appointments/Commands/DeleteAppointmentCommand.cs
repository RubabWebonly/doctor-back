using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.Appointments.Commands
{
    public class DeleteAppointmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentCommand, Unit>
    {
        private readonly IGenericRepository<Appointment> _repo;

        public DeleteAppointmentHandler(IGenericRepository<Appointment> repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _repo.GetByIdAsync(request.Id);
            if (appointment == null)
                throw new Exception("Randevu tapılmadı.");

            _repo.Delete(appointment);
            await _repo.SaveAsync();
            return Unit.Value;
        }
    }
}
