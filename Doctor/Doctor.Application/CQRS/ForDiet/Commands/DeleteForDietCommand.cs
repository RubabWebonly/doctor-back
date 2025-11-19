using MediatR;

namespace Doctor.Application.CQRS.ForDiets.Commands
{
    public class DeleteForDietCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
