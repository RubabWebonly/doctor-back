using Application.Common;
using Doctor.Application.Interfaces.Repositories;
using MediatR;

namespace Application.CQRS.AppointmentSlots.Commands
{
    public class GenerateAppointmentSlotsCommandHandler
        : IRequestHandler<GenerateAppointmentSlotsCommand, Result<string>>
    {
        private readonly IAppointmentSlotRepository _slotRepository;
        private readonly ISystemSettingRepository _systemSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GenerateAppointmentSlotsCommandHandler(
            IAppointmentSlotRepository slotRepository,
            ISystemSettingRepository systemSettingRepository,
            IUnitOfWork unitOfWork)
        {
            _slotRepository = slotRepository;
            _systemSettingRepository = systemSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GenerateAppointmentSlotsCommand request, CancellationToken cancellationToken)
        {
            var year = request.Year;

            // 📌 1️⃣ System ayarlarını DB-dən götür
            var systemSetting = await _systemSettingRepository.GetSettingAsync();
            if (systemSetting == null)
            {
                return Result<string>.Fail("Sistem ayarları tapılmadı. Əvvəlcə /api/SystemSetting/update ilə təyin edin.");
            }

            // 📌 2️⃣ Slotları bu parametrlərə əsasən yarat
            await _slotRepository.GenerateSlotsAsync(
                year,
                systemSetting.SlotIntervalMinutes,
                systemSetting.WorkStartTime,
                systemSetting.WorkEndTime
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 📌 3️⃣ Cavab mesajı
            return Result<string>.Ok(
                $"{year}-ci il üçün {systemSetting.SlotIntervalMinutes} dəqiqəlik intervalla slotlar yaradıldı."
            );
        }
    }
}
