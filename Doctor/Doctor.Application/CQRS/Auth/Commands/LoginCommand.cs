using Application.Common;
using Application.Common.DTOs;
using MediatR;

public class LoginCommand : IRequest<Result<LoginResponseDto>>
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
