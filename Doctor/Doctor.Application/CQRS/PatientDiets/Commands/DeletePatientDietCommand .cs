using MediatR;

namespace Doctor.Application.CQRS.PatientDiets.Commands
{
    public class DeletePatientDietCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
