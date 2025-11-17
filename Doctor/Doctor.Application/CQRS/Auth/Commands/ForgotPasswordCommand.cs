using Application.Common;
using MediatR;

namespace Application.CQRS.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Result<string>>
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
