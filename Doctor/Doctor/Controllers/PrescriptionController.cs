using Doctor.Application.CQRS.Prescriptions.Commands;
using Doctor.Application.CQRS.Prescriptions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PrescriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] CreatePrescriptionCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromForm] UpdatePrescriptionCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok("Resept yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePrescriptionCommand { Id = id });
        return Ok("Resept silindi.");
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllPrescriptionsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetPrescriptionByIdQuery { Id = id }));
}
