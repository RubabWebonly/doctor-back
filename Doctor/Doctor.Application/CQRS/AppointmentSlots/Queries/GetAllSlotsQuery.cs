using Application.Common;
using Application.CQRS.AppointmentSlots.DTOs;
using MediatR;

namespace Application.CQRS.AppointmentSlots.Queries
{
    public class GetAllSlotsQuery : IRequest<Result<List<AppointmentSlotDto>>> { }
}
