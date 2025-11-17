using Doctor.Domain.Entities;
using Doctor.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Doctor.Application.CQRS.Patients.Commands
{
    public class CreatePatientCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int ClinicId { get; set; }
        public string InitialDiagnosis { get; set; }
        public string DoctorReferral { get; set; }

        // ✅ Yeni əlavə etdiyimiz property
        public bool IsActive { get; set; } = true;
    }

    public class CreatePatientHandler : IRequestHandler<CreatePatientCommand, int>
    {
        private readonly IPatientRepository _repo;

        public CreatePatientHandler(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                ClinicId = request.ClinicId,
                InitialDiagnosis = request.InitialDiagnosis,
                DoctorReferral = request.DoctorReferral,
                IsActive = request.IsActive, // ✅ əlavə olundu
                CreatedDate = DateTime.UtcNow
            };

            await _repo.AddAsync(patient);
            await _repo.SaveAsync();

            return patient.Id;
        }
    }
}
