using Doctor.Application.CQRS.Diets.Commands;
using Doctor.Application.CQRS.Diets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DietController : ControllerBase
{
    private readonly IMediator _mediator;

    public DietController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] CreateDietCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromForm] UpdateDietCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok("Diet yeniləndi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        await _mediator.Send(new DeleteDietCommand { Id = id });
        return Ok("Diet silindi.");
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllDietsQuery()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetDietByIdQuery { Id = id }));
}
