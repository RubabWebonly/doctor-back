using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Doctor.Domain.Enums;
using MediatR;

namespace Doctor.Application.CQRS.Appointments.Commands
{
    public class UpdateAppointmentCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        // ✅ Birdən çox xidmət üçün
        public List<int>? ServiceIds { get; set; }

        public int? ClinicId { get; set; }
        public int? AppointmentSlotId { get; set; }

        public string? Complaint { get; set; }
        public ReferralSource? ReferralSource { get; set; }
        public PaymentType? PaymentType { get; set; }
        public decimal? RemainingAmount { get; set; }
        public AppointmentStatus? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, Unit>
    {
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<AppointmentService> _appointmentServiceRepo;
        private readonly IAppointmentSlotRepository _slotRepo;

        public UpdateAppointmentHandler(
            IGenericRepository<Appointment> appointmentRepo,
            IGenericRepository<AppointmentService> appointmentServiceRepo,
            IAppointmentSlotRepository slotRepo)
        {
            _appointmentRepo = appointmentRepo;
            _appointmentServiceRepo = appointmentServiceRepo;
            _slotRepo = slotRepo;
        }

        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(request.Id);
            if (appointment == null)
                throw new Exception("Randevu tapılmadı.");

            AppointmentSlot? oldSlot = null;
            AppointmentSlot? newSlot = null;

            // 🔹 Əgər yeni slot göndərilibsə (yəni vaxt dəyişibsə)
            if (request.AppointmentSlotId.HasValue && request.AppointmentSlotId.Value > 0)
            {
                oldSlot = await _slotRepo.GetByIdAsync(appointment.AppointmentSlotId);
                newSlot = await _slotRepo.GetByIdAsync(request.AppointmentSlotId.Value);

                if (newSlot == null)
                    throw new Exception("Yeni vaxt tapılmadı.");

                if (newSlot.IsBooked && newSlot.Id != appointment.AppointmentSlotId)
                    throw new Exception("Seçilmiş vaxt artıq doludur.");

                // köhnə slotu azad et
                if (oldSlot != null && oldSlot.Id != newSlot.Id)
                {
                    oldSlot.IsBooked = false;
                    await _slotRepo.UpdateAsync(oldSlot);
                }

                // yeni slotu rezerve et
                if (newSlot.Id != appointment.AppointmentSlotId)
                {
                    newSlot.IsBooked = true;
                    await _slotRepo.UpdateAsync(newSlot);
                    appointment.AppointmentSlotId = newSlot.Id;
                }
            }

            // 🔹 Xidmətləri yenilə
            if (request.ServiceIds != null && request.ServiceIds.Any())
            {
                // Köhnə əlaqələri sil
                var allRelations = await _appointmentServiceRepo.GetAllAsync();
                var oldRelations = allRelations
                    .OfType<AppointmentService>()
                    .Where(x => x.AppointmentId == appointment.Id)
                    .ToList();

                foreach (var rel in oldRelations)
                    _appointmentServiceRepo.Delete(rel);

                // Yeni xidmətlər əlavə et
                foreach (var serviceId in request.ServiceIds)
                {
                    var newRelation = new AppointmentService
                    {
                        AppointmentId = appointment.Id,
                        ServiceId = serviceId
                    };
                    await _appointmentServiceRepo.AddAsync(newRelation);
                }

                await _appointmentServiceRepo.SaveAsync();
            }

            // 🔹 Digər dəyişikliklər
            if (request.ClinicId.HasValue)
                appointment.ClinicId = request.ClinicId.Value;

            if (!string.IsNullOrEmpty(request.Complaint))
                appointment.Complaint = request.Complaint;

            if (request.ReferralSource.HasValue)
                appointment.ReferralSource = request.ReferralSource.Value;

            if (request.PaymentType.HasValue)
                appointment.PaymentType = request.PaymentType.Value;

            if (request.RemainingAmount.HasValue)
                appointment.RemainingAmount = request.RemainingAmount.Value;

            if (request.Status.HasValue)
                appointment.Status = request.Status.Value;

            if (!string.IsNullOrEmpty(request.Notes))
                appointment.Notes = request.Notes;

            appointment.UpdatedDate = DateTime.UtcNow;

            _appointmentRepo.Update(appointment);
            await _appointmentRepo.SaveAsync();

            return Unit.Value;
        }
    }
}
