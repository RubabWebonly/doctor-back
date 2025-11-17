using Application.Common;
using Application.CQRS.AppointmentSlots.DTOs;
using MediatR;

namespace Application.CQRS.AppointmentSlots.Queries
{
    public class GetAvailableSlotsQuery : IRequest<Result<List<AppointmentSlotDto>>>
    {
        public DateTime Date { get; set; }
    }
}
