using Application.Common;
using Doctor.Application.CQRS.SystemSettings.Queries;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Doctor.Application.CQRS.SystemSettings.Handlers
{
    public class GetSystemSettingQueryHandler : IRequestHandler<GetSystemSettingQuery, Result<SystemSetting>>
    {
        private readonly ISystemSettingRepository _systemSettingRepository;

        public GetSystemSettingQueryHandler(ISystemSettingRepository systemSettingRepository)
        {
            _systemSettingRepository = systemSettingRepository;
        }

        public async Task<Result<SystemSetting>> Handle(GetSystemSettingQuery request, CancellationToken cancellationToken)
        {
            // Mövcud system ayarını DB-dən götür
            var setting = await _systemSettingRepository.GetSettingAsync();

            // Əgər heç nə tapılmadısa, default yaradıb yadda saxla
            if (setting == null)
            {
                setting = new SystemSetting
                {
                    SlotIntervalMinutes = 30,
                    WorkStartTime = new TimeSpan(9, 0, 0),
                    WorkEndTime = new TimeSpan(19, 0, 0)
                };

                await _systemSettingRepository.UpdateSettingAsync(setting);
            }

            return Result<SystemSetting>.Ok(setting);
        }
    }
}
