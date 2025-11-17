using Application.Common;
using Application.CQRS.AppointmentSlots.DTOs;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Application.CQRS.AppointmentSlots.Queries
{
    public class GetAllSlotsQueryHandler
        : IRequestHandler<GetAllSlotsQuery, Result<List<AppointmentSlotDto>>>
    {
        private readonly IAppointmentSlotRepository _slotRepository;

        public GetAllSlotsQueryHandler(IAppointmentSlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<Result<List<AppointmentSlotDto>>> Handle(GetAllSlotsQuery request, CancellationToken cancellationToken)
        {
            var slots = await _slotRepository.GetAllAsync();
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
