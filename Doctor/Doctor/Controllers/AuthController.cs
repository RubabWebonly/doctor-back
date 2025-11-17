using Application.CQRS.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd)
    {
        var result = await _sender.Send(cmd);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }


    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand cmd)
    {
        var result = await _sender.Send(cmd);
        return Ok(result);
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand cmd)
    {
        var result = await _sender.Send(cmd);
        return Ok(result);
    }



}
