using Application.Common;

using MediatR;

namespace Application.CQRS.AppointmentSlots.Commands
{
    public class GenerateAppointmentSlotsCommand : IRequest<Result<string>>
    {
        public int Year { get; set; } = DateTime.Now.Year;
    }
}
