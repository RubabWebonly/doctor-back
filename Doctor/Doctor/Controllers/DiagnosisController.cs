using Doctor.Application.CQRS.Diagnoses.Commands;
using Doctor.Application.CQRS.Diagnoses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DiagnosisController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiagnosisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDiagnosisCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateDiagnosisCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok("Diaqnoz yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteDiagnosisCommand { Id = id });
        return Ok("Diaqnoz silindi.");
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllDiagnosesQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetDiagnosisByIdQuery { Id = id }));
}
