using Doctor.Application.Interfaces.Repositories;
using Doctor.Domain.Entities;
using Doctor.Domain.Enums;
using MediatR;

namespace Doctor.Application.CQRS.Appointments.Commands
{
    public class CreateAppointmentCommand : IRequest<int>
    {
        public int PatientId { get; set; }

        // ✅ Birdən çox xidmət üçün
        public List<int> ServiceIds { get; set; } = new();

        public int ClinicId { get; set; }
        public int AppointmentSlotId { get; set; }

        public string? Complaint { get; set; }
        public ReferralSource ReferralSource { get; set; } = ReferralSource.None;
        public PaymentType PaymentType { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<AppointmentService> _appointmentServiceRepo;
        private readonly IAppointmentSlotRepository _slotRepo;

        public CreateAppointmentHandler(
            IGenericRepository<Appointment> appointmentRepo,
            IGenericRepository<AppointmentService> appointmentServiceRepo,
            IAppointmentSlotRepository slotRepo)
        {
            _appointmentRepo = appointmentRepo;
            _appointmentServiceRepo = appointmentServiceRepo;
            _slotRepo = slotRepo;
        }

        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var slot = await _slotRepo.GetByIdAsync(request.AppointmentSlotId);
            if (slot == null)
                throw new Exception("Seçilmiş vaxt tapılmadı.");
            if (slot.IsBooked)
                throw new Exception("Bu vaxt artıq doludur.");

            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                ClinicId = request.ClinicId,
                AppointmentSlotId = slot.Id,
                Complaint = request.Complaint,
                ReferralSource = request.ReferralSource,
                PaymentType = request.PaymentType,
                RemainingAmount = request.RemainingAmount,
                Notes = request.Notes,
                Status = AppointmentStatus.Pending
            };

            // 🔹 Əlaqələri birbaşa Appointment-ə əlavə et
            foreach (var serviceId in request.ServiceIds)
            {
                appointment.AppointmentServices.Add(new AppointmentService
                {
                    ServiceId = serviceId
                });
            }

            // 🔹 Tək kontekstdə save et
            await _appointmentRepo.AddAsync(appointment);
            await _appointmentRepo.SaveAsync();

            slot.IsBooked = true;
            await _slotRepo.UpdateAsync(slot);

            return appointment.Id;
        }

    }
}
