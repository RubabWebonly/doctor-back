using Doctor.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctor.Application.CQRS.Patients.Commands
{
    public class UpdatePatientCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int ClinicId { get; set; }
        public string InitialDiagnosis { get; set; }
        public string DoctorReferral { get; set; }

        // ✅ Əlavə property
        public bool IsActive { get; set; }
    }

    public class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand, Unit>
    {
        private readonly IPatientRepository _repo;

        public UpdatePatientHandler(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _repo.GetByIdAsync(request.Id);
            if (patient == null)
                throw new Exception("Pasiyent tapılmadı.");

            patient.FullName = request.FullName;
            patient.PhoneNumber = request.PhoneNumber;
            patient.BirthDate = request.BirthDate;
            patient.Gender = request.Gender;
            patient.ClinicId = request.ClinicId;
            patient.InitialDiagnosis = request.InitialDiagnosis;
            patient.DoctorReferral = request.DoctorReferral;
            patient.IsActive = request.IsActive; // ✅ update zamanı aktivlik statusu da dəyişir
            patient.UpdatedDate = DateTime.UtcNow;

            _repo.Update(patient);
            await _repo.SaveAsync();

            return Unit.Value;
        }
    }
}
