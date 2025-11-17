using Doctor.Application.CQRS.Services.Commands;
using Doctor.Application.CQRS.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateServiceCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateServiceCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok("Xidmət yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteServiceCommand { Id = id });
        return Ok("Xidmət silindi.");
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllServicesQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetServiceByIdQuery { Id = id }));
}
