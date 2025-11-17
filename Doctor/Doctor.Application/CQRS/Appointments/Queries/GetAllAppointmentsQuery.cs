using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Application.CQRS.Appointments.Queries
{
    public class GetAllAppointmentsQuery : IRequest<IEnumerable<Appointment>> { }

    public class GetAllAppointmentsHandler : IRequestHandler<GetAllAppointmentsQuery, IEnumerable<Appointment>>
    {
        private readonly IGenericRepository<Appointment> _repo;

        public GetAllAppointmentsHandler(IGenericRepository<Appointment> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Appointment>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetAllAsync(query =>
                query
                    .Include(x => x.Patient)
                    .Include(x => x.Clinic)
                    .Include(x => x.AppointmentSlot)
                    .Include(x => x.AppointmentServices)
                        .ThenInclude(s => s.Service)
                    .AsNoTracking()
            );

            return result;
        }
    }
}
