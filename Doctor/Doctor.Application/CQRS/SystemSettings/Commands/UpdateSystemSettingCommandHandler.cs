using Application.Common;
using Doctor.Application.CQRS.SystemSettings.Commands;
using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using MediatR;

namespace Application.CQRS.SystemSettings.Commands
{
    public class UpdateSystemSettingCommandHandler
        : IRequestHandler<UpdateSystemSettingCommand, Result<string>>
    {
        private readonly ISystemSettingRepository _systemSettingRepository;
        private readonly IAppointmentSlotRepository _appointmentSlotRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSystemSettingCommandHandler(
            ISystemSettingRepository systemSettingRepository,
            IAppointmentSlotRepository appointmentSlotRepository,
            IUnitOfWork unitOfWork)
        {
            _systemSettingRepository = systemSettingRepository;
            _appointmentSlotRepository = appointmentSlotRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdateSystemSettingCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Mövcud system ayarını tap və ya yeni yarat
            var setting = await _systemSettingRepository.GetSettingAsync() ?? new SystemSetting();

            // 2️⃣ Yeni dəyərləri yaz
            setting.SlotIntervalMinutes = request.SlotIntervalMinutes;
            setting.WorkStartTime = request.WorkStartTime;
            setting.WorkEndTime = request.WorkEndTime;

            // 3️⃣ Yadda saxla
            await _systemSettingRepository.UpdateSettingAsync(setting);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4️⃣ Əlavə: Yeni slotları yarat
            await _appointmentSlotRepository.GenerateSlotsAsync(
                DateTime.Now.Year,
                setting.SlotIntervalMinutes,
                setting.WorkStartTime,
                setting.WorkEndTime
            );

            // 5️⃣ Nəticə
            return Result<string>.Ok(
                $"Sistem ayarları yeniləndi və {DateTime.Now.Year}-ci il üçün {setting.SlotIntervalMinutes}-dəqiqəlik slotlar yaradıldı."
            );
        }
    }
}
