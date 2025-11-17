using Application.Common;
using Application.CQRS.AppointmentSlots.DTOs;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Application.CQRS.AppointmentSlots.Queries
{
    public class GetAvailableSlotsQueryHandler
        : IRequestHandler<GetAvailableSlotsQuery, Result<List<AppointmentSlotDto>>>
    {
        private readonly IAppointmentSlotRepository _slotRepository;

        public GetAvailableSlotsQueryHandler(IAppointmentSlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<Result<List<AppointmentSlotDto>>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await _slotRepository.GetAvailableSlotsAsync(request.Date);

            var dtoList = slots.Select(x => new AppointmentSlotDto
            {
                Id = x.Id,
                Date = x.Date,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                IsBooked = x.IsBooked
            }).ToList();

            return Result<List<AppointmentSlotDto>>.Ok(dtoList);
        }
    }
}
