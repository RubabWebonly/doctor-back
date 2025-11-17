using Application.Common;
using MediatR;

namespace Doctor.Application.CQRS.SystemSettings.Commands
{
    public class UpdateSystemSettingCommand : IRequest<Result<string>>
    {
        public int SlotIntervalMinutes { get; set; } = 30;
        public TimeSpan WorkStartTime { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan WorkEndTime { get; set; } = new TimeSpan(19, 0, 0);
    }
}
