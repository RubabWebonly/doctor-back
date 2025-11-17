using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Application.CQRS.Appointments.Queries
{
    public class GetAppointmentByIdQuery : IRequest<Appointment>
    {
        public int Id { get; set; }
    }

    public class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, Appointment>
    {
        private readonly IGenericRepository<Appointment> _repo;

        public GetAppointmentByIdHandler(IGenericRepository<Appointment> repo)
        {
            _repo = repo;
        }

        public async Task<Appointment> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _repo.GetAllAsync(q =>
                q.Include(x => x.Patient)
                 .Include(x => x.Clinic)
                 .Include(x => x.AppointmentSlot)
                 .Include(x => x.AppointmentServices)
                    .ThenInclude(a => a.Service)
            );

            var appointment = query.FirstOrDefault(x => x.Id == request.Id);
            if (appointment == null)
                throw new Exception("Randevu tapılmadı.");

            return appointment;
        }

    }
}
