using Application.Common;
using MediatR;

namespace Application.CQRS.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<Result<string>>
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
