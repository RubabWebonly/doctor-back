using MediatR;

namespace Doctor.Application.CQRS.PatientPrescriptions.Commands
{
    public class DeletePatientPrescriptionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
