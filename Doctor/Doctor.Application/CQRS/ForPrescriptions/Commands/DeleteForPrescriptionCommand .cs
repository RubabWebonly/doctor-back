using MediatR;

namespace Doctor.Application.CQRS.ForPrescriptions.Commands
{
    public class DeleteForPrescriptionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
