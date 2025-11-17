using Doctor.Application.CQRS.Treatments.Commands;
using Doctor.Application.CQRS.Treatments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TreatmentController : ControllerBase
{
    private readonly IMediator _mediator;
    public TreatmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] CreateTreatmentCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromForm] UpdateTreatmentCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok("Müalicə yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteTreatmentCommand { Id = id });
        return Ok("Müalicə silindi.");
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllTreatmentsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetTreatmentByIdQuery { Id = id }));

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromQuery] string range = "weekly", [FromQuery] int year = 0)
    {
        if (year == 0) year = DateTime.Now.Year;
        var result = await _mediator.Send(new GetTreatmentStatsQuery { Range = range, Year = year });
        return Ok(result);
    }

}
